using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour {

	public enum TargetModeEnum
	{
		Normal,
		TargetPlayer,
		TrackPlayer
	}

	[Header("Spawner vars")]
	[SerializeField]
	protected TargetModeEnum TargetMode;
	[SerializeField]
	protected float FireRate = 3f;
	[SerializeField]
	protected float SpinSpeed = 0;
	[SerializeField]
	protected int TotalShots = 1;
	[SerializeField]
	protected int BulletsPerShot = 3;
	[SerializeField]
	protected float Spread = 45f;
	[SerializeField]
	protected float DeltaSpread = 0f;

	[Header("Bullet vars")]
	[SerializeField]
	protected GameObject BulletPref;
	[SerializeField]
	protected float SpawnDistance = 0.5f;
	[SerializeField]
	protected float StartVelocity = 3f;
	[SerializeField]
	protected float EndVelocity = 3f;
	[SerializeField]
	protected float Acceleration = 0f;
	[SerializeField]
	protected float AccelDelay = 0f;

	//Private vars
	protected bool IsFiring = false;
	protected int ShotsLeft = 0;
	protected float ShotTimer = 0;
	protected float BaseAngle = -90;
	protected float BaseSpread = 0;
	
	// Update is called once per frame
	void Update ()
	{
		ShotTimer -= Time.deltaTime;

		while (ShotTimer < 0 && ShotsLeft > 0)
		{
			ShotTimer += (1f / FireRate);

			if(TargetMode == TargetModeEnum.TrackPlayer)
			{
				AngleTowardPlayer();
			}
			else if(TargetMode == TargetModeEnum.Normal)
			{
				BaseAngle += (1f / FireRate) * SpinSpeed;
			}

			ShootVolley();

			BaseSpread += DeltaSpread * (1f / FireRate);

			ShotsLeft--;
			if (ShotsLeft == 0)
				IsFiring = false;
		}
	}

	public void Fire()
	{
		if(!IsFiring)
		{
			IsFiring = true;
			ShotsLeft = TotalShots;
			ShotTimer = 0f;
			BaseSpread = Spread;

			if(TargetMode == TargetModeEnum.TargetPlayer)
			{
				AngleTowardPlayer();
			}
		}
	}

	protected void ShootVolley()
	{
		float dangle = BaseSpread / BulletsPerShot;

		for(int i = 0; i < BulletsPerShot; i++)
		{
			//Generate custom angle
			float myAng = (i * dangle) + BaseAngle - (BaseSpread / 2) + (dangle / 2);
			SpawnBullet(myAng);
		}
	}

	protected void SpawnBullet(float ang)
	{
		Vector2 disp = new Vector2(Mathf.Cos(ang * Mathf.Deg2Rad), Mathf.Sin(ang * Mathf.Deg2Rad)) * SpawnDistance;

		GameObject obj = Instantiate(BulletPref, (Vector2)transform.position + disp, Quaternion.identity);
		Bullet bullet = obj.GetComponent<Bullet>();

		bullet.SetMotion(ang, StartVelocity, EndVelocity, Acceleration, AccelDelay);
	}

	protected void AngleTowardPlayer()
	{
		if (Player.Singleton != null)
		{
			float dx = Player.Singleton.transform.position.x - transform.position.x;
			float dy = Player.Singleton.transform.position.y - transform.position.y;
			BaseAngle = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg;
		}
		else
			BaseAngle = -90f;
	}
}
