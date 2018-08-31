using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : Damageable {

    private const float BoundLeft = -10f;
    private const float BoundRight = 10f;
    private const float BoundUp = 7f;
    private const float BoundDown = -7f;

	protected float Angle;
	protected float StartSpeed;
	protected float EndSpeed;
	protected float AccelStart;
	protected float AccelEnd;
	protected float AliveTime = 0;

	protected Rigidbody2D rbody;

    [SerializeField]
    private AudioClip destroySound;
	//[SerializeField]
	//private bool log = false;

	void Start()
	{
		rbody = GetComponent<Rigidbody2D>();
	}

	//Fixed update called less, will cause less lag.
	private void FixedUpdate()
    {

        Vector2 pos = transform.position;

        if (pos.x < BoundLeft || pos.x > BoundRight || pos.y < BoundDown || pos.y > BoundUp)
        {
            Destroy(gameObject);
        }


		//Calculate new velocity every frame.
		AliveTime += Time.deltaTime;
		float curSpeed = 0;

		if(AliveTime < AccelStart)
		{
			curSpeed = StartSpeed;
		}
		else if(AliveTime > AccelStart && AliveTime < AccelEnd)
		{
			curSpeed = Mathf.Lerp(StartSpeed, EndSpeed, ((AliveTime - AccelStart) / (AccelEnd - AccelStart)) );
		}
		else
		{
			curSpeed = EndSpeed;
		}

		SetSpeed(curSpeed);
    }

	public void SetMotion(float ang, float ss, float es, float acc, float accdel)
	{
		Angle = ang;
		StartSpeed = ss;
		EndSpeed = es;
		AccelStart = accdel;
		AccelEnd = accdel + ( (es - ss) / acc);

		//SetSpeed(StartSpeed);
	}

	protected void SetSpeed(float speed)
	{
		rbody.velocity = new Vector2(Mathf.Cos(Angle * Mathf.Deg2Rad), Mathf.Sin(Angle * Mathf.Deg2Rad)) * speed;
	}

    public override void Kill()
    {
        //AudioSource.PlayClipAtPoint(destroySound, transform.position);
        //SoundMaker2D.Singleton.PlayClipAtPoint(destroySound, transform.position);
        base.Kill();
    }
}
