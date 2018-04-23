﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenShip :  Enemy, IBeatListener, IPositionable {

    [SerializeField]
    private GameObject BulletPref;

    [Header("Sounds")]
    [SerializeField]
    private AudioClip soundDamage;
    [SerializeField]
    private AudioClip soundDestroy;
    [SerializeField]
    private AudioClip soundFire;

    private int BeatCount = 6;
    private int CurrentBeat = 0;

    [SerializeField]
    private float MoveSpeed = 5f;
    private Vector2 TargetPos;

    // Use this for initialization
    new void Start()
    {
        //Random.InitState((int)System.DateTime.Now.Ticks);
        CurrentBeat = Random.Range(0, 4);

        base.Start();
        BeatGenerator.GetSingleton().AddListener(this);
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Rigidbody2D>().MovePosition(Vector2.MoveTowards(transform.position, TargetPos, MoveSpeed * Time.deltaTime));
    }


    IEnumerator Fire()
    {
        GetComponent<AudioSource>().clip = soundFire;
        GetComponent<AudioSource>().Play();

        int numShots = 2;
        int numPellets = 4;
        float dir = -Mathf.PI / 2f;
        float spread = Mathf.PI * 3f / 8f;
        float speed = 4f;


        for (int i = 0; i < numShots; i++)
        {
            
            for(int j = 0; j < numPellets; j++)
            {
                float ang = j / (1f * numPellets - 1);
                ang *= spread;
                ang -= spread / 2f;

                ang = dir + ang;

                GameObject newBullet = Instantiate(BulletPref, transform.position, transform.rotation);
                newBullet.GetComponent<Bullet>().SetMotion(ang, speed);
            }

            yield return new WaitForSeconds(.5f);
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


    public override int Damage(int amt)
    {
        GetComponent<AudioSource>().clip = soundDamage;
        GetComponent<AudioSource>().Play();

        return base.Damage(amt);
    }

    public override void Kill()
    {
        SoundMaker2D.Singleton.PlayClipAtPoint(soundDestroy, transform.position, 1f);
        BeatGenerator.GetSingleton().RemoveListener(this);


        base.Kill();
    }
}
