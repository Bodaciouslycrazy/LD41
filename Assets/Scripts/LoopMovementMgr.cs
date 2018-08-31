using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopMovementMgr : MonoBehaviour, IBeatListener {

	public AnimationCurve SpeedAfterBeat;

	public LoopMovement[] Moving;

	private float BeatTime = 5f;

	// Use this for initialization
	void Start () {
		BeatGenerator.GetSingleton().AddListener(this);
	}
	
	// Update is called once per frame
	void Update () {
		BeatTime += Time.deltaTime;

		float dist = SpeedAfterBeat.Evaluate(BeatTime) * Time.deltaTime;

		for(int i = 0; i < Moving.Length; i++)
		{
			Moving[i].Move(dist);
		}
	}

	public void OnBeat()
	{
		BeatTime = 0;
	}

	public void OnUpbeat()
	{
		
	}
}
