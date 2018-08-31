using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingObject : Bullet, IPositionable
{
	public void SetTarget(Vector2 targ)
	{
		SetMotion(Random.Range(-135, -45), 2.5f, 2.5f, 0, 0);
	}
}
