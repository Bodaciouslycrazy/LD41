using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct SpawnData
{
	public enum WaveEnum
	{
		SPAWN,
		DELAY,
		STORY_EVENT,
		WAIT_FOR_EMPTY,
		SYNC
	}


	public WaveEnum Type;

	public GameObject Prefab;
	public Vector2 Target;
	public string Tag;
}

