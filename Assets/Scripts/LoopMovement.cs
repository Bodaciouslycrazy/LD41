using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopMovement : MonoBehaviour {

	public float Multiplier = 1f;

	public float MaxY = 5f;
	public float MinY = -5f;

	public void Move(float dist)
	{
		dist *= Multiplier;

		transform.position += new Vector3(0, dist, 0);

		if(transform.position.y > MaxY)
		{
			transform.position -= new Vector3(0, MaxY - MinY, 0);
		}
		else if(transform.position.y < MinY)
		{
			transform.position += new Vector3(0, MaxY - MinY, 0);
		}
	}
}
