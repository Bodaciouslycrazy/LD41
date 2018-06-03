using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSyncer : MonoBehaviour {

	private List<SpawnController> Controllers;

	// Use this for initialization
	void Start () {
		Controllers = new List<SpawnController>(GetComponents<SpawnController>());

		/*
		for(int i = 0; i < Controllers.Count; i++)
		{
			Controllers[i].Master = this;
		}
		*/
	}

	// Update is called once per frame
	public void FixedUpdate()
	{
		for(int i = 0; i < Controllers.Count; i++)
		{
			if(Controllers[i].SyncWait != -1 && Controllers[ Controllers[i].SyncWait ].SyncWait == i)
			{
				//Two have ben synced together.
				Controllers[Controllers[i].SyncWait].ForceNextWave();
				Controllers[i].ForceNextWave();
			}
		}
	}
}
