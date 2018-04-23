using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShip : Enemy, IBeatListener, IPositionable
{

    [SerializeField]
    private GameObject BulletPref;

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

    [SerializeField]
    private float MoveSpeed = 5f;
    [SerializeField]
    private float WaveSpeed = 1f;
    [SerializeField]
    private float WaveDistance = 1f;
    private Vector2 TargetPos;

    private bool ShotThisBeat = false;
    private int ConsecutiveShots = 0;

    // Use this for initialization
    new void Start()
    {
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
        float dx = WaveDistance * Mathf.Sin(Time.time * WaveSpeed * 2 * Mathf.PI);

        Vector2 realTarget = TargetPos + new Vector2(dx, 0);

        GetComponent<Rigidbody2D>().MovePosition(Vector2.MoveTowards(transform.position, realTarget, MoveSpeed * Time.deltaTime));
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
            GetComponent<AudioSource>().clip = soundComboEnd;
            GetComponent<AudioSource>().Play();
        }

        ShotThisBeat = false;
    }


    public override int Damage(int amt)
    {
        ShotThisBeat = true;
        ConsecutiveShots++;
        SetColor(Random.Range(0, 3));

        if (ConsecutiveShots == 1)
            GetComponent<AudioSource>().clip = soundComboOne;
        else if (ConsecutiveShots == 2)
            GetComponent<AudioSource>().clip = soundComboTwo;
        else if (ConsecutiveShots == 3)
            GetComponent<AudioSource>().clip = soundComboThree;

        GetComponent<AudioSource>().Play();

        if( ConsecutiveShots >= 3)
        {
            //ACTUALLY DO DAMAGE
            ConsecutiveShots = 0;

            Health--;
            if (Health <= 0)
                Kill();
        }

        //return base.Damage(amt);
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
