using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour {

	public List<SpawnData> SpawnList = new List<SpawnData>();
	//public SpawnSyncer Master;
	public int SyncWait = -1;

	private List<GameObject> SpawnedObjects;

	private int curWave = 0;
	private float curWaveStart = 0;

	private Dictionary<string, SpawnEvent> Events;

	private bool RepeatCommand = true;


	// Use this for initialization
	void Start () {
		SpawnedObjects = new List<GameObject>();

		curWaveStart = Time.time;

		Events = new Dictionary<string, SpawnEvent>()
		{
			{"SPAWN", Spawn },
			{"DELAY", Delay },
			{"WAIT_FOR_EMPTY", WaitForEmpty },
			{"SYNC", Sync },
			{"STORY_EVENT", StoryEvent }
		};
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void FixedUpdate()
	{
		while( RunWave(curWave) == true)
		{
			ForceNextWave();
		}
	}

	public void ForceNextWave()
	{
		curWave++;
		curWaveStart = Time.time;
		RepeatCommand = true;
		SyncWait = -1;
	}

	private bool RunWave(int index)
	{
		if(index >= SpawnList.Count)
		{
			Debug.Log("Final wave complete.");
			this.enabled = false;
			return false;
		}

		try
		{
			if (RepeatCommand)
				return Events[SpawnList[curWave].Type.ToString()](SpawnList[curWave]);
			else
				return false;
		}
		catch(System.Exception e)
		{
			Debug.LogWarning("Could not trigger event " + curWave);
			Debug.LogWarning(e.ToString());
			return false;
		}
	}

	#region Commands

	private delegate bool SpawnEvent(SpawnData data);

	private bool Spawn( SpawnData data )
	{
		SpawnedObjects.Add(SpawnShip(data.Prefab, data.Target));

		return true;
	}

	private bool Delay( SpawnData data )
	{
		float thresh = float.Parse(data.Tag);

		return (Time.time - curWaveStart > thresh);
	}

	private bool WaitForEmpty( SpawnData data)
	{
		for(int i = 0; i < SpawnedObjects.Count; i++)
		{
			if(SpawnedObjects[i] == null)
			{
				SpawnedObjects.RemoveAt(i);
				i--;
			}
			else
			{
				return false;
			}
		}

		return true;
	}

	private bool Sync( SpawnData data)
	{
		RepeatCommand = false;

		SyncWait = int.Parse(data.Tag);

		return false;
	}

	private bool StoryEvent(SpawnData data)
	{
		if(data.Tag.Equals("win"))
		{
			UIState.Singleton.SetState(UIState.State.WIN);
		}

		return true;
	}

	#endregion

	public static GameObject SpawnShip(GameObject ship, Vector2 targ)
	{
		Vector2 spawnLoc = new Vector2(targ.x, 6);

		GameObject spawned = Instantiate(ship, spawnLoc, Quaternion.identity);
		spawned.GetComponent<IPositionable>().SetTarget(targ);

		return spawned;
	}
}
