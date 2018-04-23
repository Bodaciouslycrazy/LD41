using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShip : Enemy, IBeatListener, IPositionable
{

    [SerializeField]
    private GameObject BulletPref;
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
    private int ConsecutiveShots = 0;

    private Rigidbody2D rb;

    // Use this for initialization
    new void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //Random.InitState((int)System.DateTime.Now.Ticks);
        //CurrentBeat = Random.Range(0, 4);
        SetColor(1);

        base.Start();
        BeatGenerator.GetSingleton().AddListener(this);
    }

    public override void SetColor(int col)
    {
        Color spriteCol = new Color(0, 0, 0);

        switch (col)
        {
            case 0:
                spriteCol.b = .85f;
                break;
            case 1:
                spriteCol.g = .85f;
                break;
            case 2:
                spriteCol.r = .85f;
                break;
        }

        transform.GetChild(0).GetComponent<SpriteRenderer>().color = spriteCol;

        base.SetColor(col);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FixedUpdate()
    {

        float dx = WaveDistance * Mathf.Sin(Time.time * WaveSpeed * 2 * Mathf.PI);

        Vector2 realTarget = TargetPos + new Vector2(dx, 0);

        rb.MovePosition(Vector2.MoveTowards(transform.position, realTarget, MoveSpeed * Time.fixedDeltaTime));
    }


    IEnumerator Fire()
    {
        GetComponent<AudioSource>().clip = soundFire;
        GetComponent<AudioSource>().Play();

        float speed = 3f;
        float ang = -Mathf.PI / 2f;
        if (Player.Singleton != null)
        {
            float dx = Player.Singleton.transform.position.x - transform.position.x;
            float dy = Player.Singleton.transform.position.y - transform.position.y;
            ang = Mathf.Atan2(dy, dx);
        }

        GameObject newBullet = Instantiate(BulletPref, transform.position, Quaternion.identity);
        newBullet.GetComponent<Bullet>().SetMotion(ang, speed);
        newBullet = Instantiate(BulletPref, (Vector2)transform.position + new Vector2(1,0), Quaternion.identity);
        newBullet.GetComponent<Bullet>().SetMotion(ang, speed);
        newBullet = Instantiate(BulletPref, (Vector2)transform.position + new Vector2(-1,0), Quaternion.identity);
        newBullet.GetComponent<Bullet>().SetMotion(ang, speed);

        yield break;
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
            StartCoroutine(Fire());
        }
    }

    public void OnUpbeat()
    {
        if(!ShotThisBeat && ConsecutiveShots > 0)
        {
            ConsecutiveShots = 0;

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

    public override int Damage(int amt, int damColor)
    {
        if (damColor == GetColor())
        {
            ShotThisBeat = true;
            ConsecutiveShots++;

            if (ConsecutiveShots == 1)
                SoundMaker2D.Singleton.PlayClipAtPoint(soundComboOne, transform.position, 1f);
            else if (ConsecutiveShots == 2)
                SoundMaker2D.Singleton.PlayClipAtPoint(soundComboTwo, transform.position, 1f);
            else if (ConsecutiveShots == 3)
                SoundMaker2D.Singleton.PlayClipAtPoint(soundComboThree, transform.position, 1f);


            if (ConsecutiveShots >= 3)
            {
                //ACTUALLY DO DAMAGE
                ConsecutiveShots = 0;

                base.Damage(amt, damColor);

                ShowHealth(Health);
            }
        }
        else
        {
            ShotThisBeat = false;
            ConsecutiveShots = 0;

            SoundMaker2D.Singleton.PlayClipAtPoint(soundComboEnd, transform.position);
        }


        SetColor(Random.Range(0, 3));
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
