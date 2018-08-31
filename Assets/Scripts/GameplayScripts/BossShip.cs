using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BulletSpawner))]
public class BossShip : Enemy, IBeatListener, IPositionable
{
    [SerializeField]
    private SpriteRenderer[] HealthSprites;

    [Header("Sounds")]
    [SerializeField]
    private AudioClip soundComboOne;
    [SerializeField]
    private AudioClip soundComboTwo;
    [SerializeField]
    private AudioClip soundComboThree;
    [SerializeField]
    private AudioClip soundComboEnd;
    [SerializeField]
    private AudioClip soundDestroy;
    [SerializeField]
    private AudioClip soundFire;

    private int BeatCount = 2;
    private int CurrentBeat = 0;

    [Header("Speeds")]
    [SerializeField]
    private float MoveSpeed = 5f;
    [SerializeField]
    private float WaveSpeed = 1f;
    [SerializeField]
    private float WaveDistance = 1f;
    private Vector2 TargetPos;

    private bool ShotThisBeat = false;

    private Rigidbody2D rb;

    // Use this for initialization
    new void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        SetColor( ColorEnum.GREEN );

        base.Start();
        BeatGenerator.GetSingleton().AddListener(this);
    }

    public override void SetColor(ColorEnum col)
    {
        Color spriteCol = new Color(0, 0, 0);

        switch (col)
        {
            case ColorEnum.BLUE:
                spriteCol.b = .85f;
                break;
            case ColorEnum.GREEN:
                spriteCol.g = .85f;
                break;
            case ColorEnum.RED:
                spriteCol.r = .85f;
                break;
        }

		transform.GetChild(0).GetComponent<SpriteRenderer>().color = spriteCol;
        base.SetColor(col);
    }

    public void FixedUpdate()
    {

        float dx = WaveDistance * Mathf.Sin(Time.time * WaveSpeed * 2 * Mathf.PI);

        Vector2 realTarget = TargetPos + new Vector2(dx, 0);

        rb.MovePosition(Vector2.MoveTowards(transform.position, realTarget, MoveSpeed * Time.fixedDeltaTime));
    }

    public void SetTarget(Vector2 targ)
    {
        TargetPos = targ;
    }

    public void OnBeat()
    {
        CurrentBeat += 1;

        if (CurrentBeat >= BeatCount)
        {
            CurrentBeat = 0;
			//Fire!!!
			GetComponent<BulletSpawner>().Fire();
        }
    }

    public void OnUpbeat()
    {
        if(!ShotThisBeat && Health < MaxHealth)
        {
            Health = MaxHealth;
            ShowHealth(Health);

            //Play bad sound.
            SoundMaker2D.Singleton.PlayClipAtPoint(soundComboEnd, transform.position);
        }

        ShotThisBeat = false;
    }

    private void ShowHealth(int h)
    {
        for(int i = 0; i < HealthSprites.Length; i++)
        {
            HealthSprites[i].enabled = (i <= h - 1);
        }
    }

    public override int Damage(int amt, ColorEnum damColor)
    {
        if (damColor == GetColor())
        {
            ShotThisBeat = true;
            Health--;

            if (Health == 2)
                SoundMaker2D.Singleton.PlayClipAtPoint(soundComboOne, transform.position, 1f);
            else if (Health == 1)
                SoundMaker2D.Singleton.PlayClipAtPoint(soundComboTwo, transform.position, 1f);
            else if (Health == 0)
                SoundMaker2D.Singleton.PlayClipAtPoint(soundComboThree, transform.position, 1f);

            ShowHealth(Health);

            if (Health <= 0)
                Kill();
        }
        else
        {
            ShotThisBeat = false;
            Health = MaxHealth;
            ShowHealth(Health);

            SoundMaker2D.Singleton.PlayClipAtPoint(soundComboEnd, transform.position);
        }


        SetColor( (ColorEnum)Random.Range(0, 3));
        return Health;
    }

    public override void Kill()
    {
        SoundMaker2D.Singleton.PlayClipAtPoint(soundDestroy, transform.position, 1f);
        BeatGenerator.GetSingleton().RemoveListener(this);

        CameraShake.ShakeCamera(1f);

        base.Kill();
    }
}
