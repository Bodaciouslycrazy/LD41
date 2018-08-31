using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour {

	private static PauseManager Singleton;

	private bool Paused = false;

	// Use this for initialization
	void Start () {
		if (Singleton != null)
			Destroy(Singleton);

		Singleton = this;
	}

	private void Update()
	{
		if(Input.GetKeyDown("escape"))
		{
			Paused = !Paused;

			if (Paused)
				PauseAll();
			else
				ResumeAll();
		}
	}

	public bool IsPaused()
	{
		return Paused;
	}

	private void PauseAll()
	{
		Time.timeScale = 0;

		GameObject[] objects = (GameObject[])FindObjectsOfType(typeof(GameObject));
		foreach (GameObject obj in objects)
		{
			obj.SendMessage("OnPauseGame", SendMessageOptions.DontRequireReceiver);
		}
	}

	private void ResumeAll()
	{
		Time.timeScale = 1;

		GameObject[] objects = (GameObject[])FindObjectsOfType(typeof(GameObject));
		foreach (GameObject obj in objects)
		{
			obj.SendMessage("OnResumeGame", SendMessageOptions.DontRequireReceiver);
		}
	}

	private void OnDestroy()
	{
		ResumeAll();
	}

	#region Static Methods

	public static PauseManager GetSingleton()
	{
		return Singleton;
	}

	#endregion
}
