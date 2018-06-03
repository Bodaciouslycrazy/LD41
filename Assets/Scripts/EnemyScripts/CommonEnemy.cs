using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BulletSpawner))]
public class CommonEnemy : Enemy, IBeatListener, IPositionable
{
	[Header("Behavior")]
	[SerializeField]
	private int BeatCount = 4;
	[SerializeField]
	private float MoveSpeed = 5f;

	[Header("Sounds")]
	[SerializeField]
	private AudioClip soundDamage;
	[SerializeField]
	private AudioClip soundDestroy;
	[SerializeField]
	private AudioClip soundFire;
	[SerializeField]
	private AudioClip soundDeflect;

	//Private vars
	private int CurrentBeat = 0;
	private Vector2 TargetPos;
	private Rigidbody2D rb;

	// Use this for initialization
	public override void Start()
	{
		rb = GetComponent<Rigidbody2D>();

		base.Start();
		BeatGenerator.GetSingleton().AddListener(this);
	}

	void Update()
	{

	}

	public void FixedUpdate()
	{
		rb.MovePosition(Vector2.MoveTowards(transform.position, TargetPos, MoveSpeed * Time.fixedDeltaTime));
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
			SoundMaker2D.Singleton.PlayClipAtPoint(soundFire, transform.position);
			GetComponent<BulletSpawner>().Fire();
		}
	}

	public void OnUpbeat()
	{

	}

	public override int Damage(int amt, ColorEnum damColor)
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
