using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallShip : Enemy, IBeatListener, IPositionable
{

    [SerializeField]
    private GameObject BulletPref;

    [Header("Sounds")]
    [SerializeField]
    private AudioClip soundDamage;
    [SerializeField]
    private AudioClip soundDestroy;
    [SerializeField]
    private AudioClip soundFire;
    [SerializeField]
    private AudioClip soundDeflect;

    private int BeatCount = 4;
    private int CurrentBeat = 0;

    [SerializeField]
    private float MoveSpeed = 5f;
    private Vector2 TargetPos;

    private static int NEXT_COLOR = 1;

    private Rigidbody2D rb;

    // Use this for initialization
    new void Start()
    {
        //Random.InitState((int)System.DateTime.Now.Ticks);
        //CurrentBeat = Random.Range(0, 4);
        SetColor(NEXT_COLOR);
        NEXT_COLOR = (NEXT_COLOR + 1) % 3;

        rb = GetComponent<Rigidbody2D>();

        base.Start();
        BeatGenerator.GetSingleton().AddListener(this);
    }

    public override void SetColor(int col)
    {
        Color spriteCol = new Color(0,0,0);

        switch(col)
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
        /*
        Vector2 oldPos = transform.position;

        GetComponent<Rigidbody2D>().MovePosition(Vector2.MoveTowards(transform.position, TargetPos, MoveSpeed * Time.deltaTime));

        Vector2 newPos = transform.position;

        if (Vector2.Distance(oldPos, newPos) < MoveSpeed * Time.deltaTime - .05f)
            transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(1, 0, 1);
        */
    }

    public void FixedUpdate()
    {
        rb.MovePosition(Vector2.MoveTowards(transform.position, TargetPos, MoveSpeed * Time.fixedDeltaTime));
    }


    IEnumerator Fire()
    {
        GetComponent<AudioSource>().clip = soundFire;
        GetComponent<AudioSource>().Play();

        int numShots = 3;
        float dir = -Mathf.PI / 2f;
        float spread = Mathf.PI * 3f / 8f;
        float speed = 3f;


        for (int i = 0; i < numShots; i++)
        {

            float ang = i / (1f * numShots - 1);
            ang *= spread;
            ang -= spread / 2f;

            ang = dir + ang;

            GameObject newBullet = Instantiate(BulletPref, transform.position, Quaternion.identity);
            newBullet.GetComponent<Bullet>().SetMotion(ang, speed);


        }

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

    }


    public override int Damage(int amt, int damColor)
    {
        if (damColor == GetColor())
        {
            GetComponent<AudioSource>().clip = soundDamage;
        }
        else
        {
            GetComponent<AudioSource>().clip = soundDeflect;
        }

        GetComponent<AudioSource>().Play();

        return base.Damage(amt, damColor);
    }

    public override void Kill()
    {
        SoundMaker2D.Singleton.PlayClipAtPoint(soundDestroy, transform.position, 1f);
        BeatGenerator.GetSingleton().RemoveListener(this);


        base.Kill();
    }
}
