using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueShip : Enemy, IBeatListener, IPositionable
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

    private int BeatCount = 6;
    private int CurrentBeat = 0;

    [SerializeField]
    private float MoveSpeed = 5f;
    private Vector2 TargetPos;

    private Rigidbody2D rb;

    // Use this for initialization
    new void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //Random.InitState((int)System.DateTime.Now.Ticks);
        //CurrentBeat = Random.Range(0, 4);

        base.Start();
        BeatGenerator.GetSingleton().AddListener(this);
    }

    // Update is called once per frame
    void Update()
    {

        //GetComponent<Rigidbody2D>().MovePosition(Vector2.MoveTowards(transform.position, TargetPos, MoveSpeed * Time.deltaTime));
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
        float ang = -Mathf.PI / 2f;
        if (Player.Singleton != null)
        {
            float dx = Player.Singleton.transform.position.x - transform.position.x;
            float dy = Player.Singleton.transform.position.y - transform.position.y;
            ang = Mathf.Atan2(dy, dx);
        }
        //float dir = -Mathf.PI / 2f;
        float speed = 4f;


        for (int i = 0; i < numShots; i++)
        {
            GameObject newBullet = Instantiate(BulletPref, transform.position, transform.rotation);
            newBullet.GetComponent<Bullet>().SetMotion(ang, speed);

            yield return new WaitForSeconds(.35f);
        }
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
