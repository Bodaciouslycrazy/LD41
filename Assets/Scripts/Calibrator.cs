using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calibrator : MonoBehaviour, IBeatListener{

	public AudioClip song;
	private bool tappedThisBeat = false;

	private List<double> TapVariance;
	//private List<int> TapBeats;

	private BeatGenerator bg;
	
	// Use this for initialization
	void Start () {
		bg = BeatGenerator.GetSingleton();
		BeatGenerator.GetSingleton().AddListener(this);
		TapVariance = new List<double>();

		bg.StartSong(130.0f, song);
		bg.SetMetronome(true);
		//TapBeats = new List<int>();
	}
	
	// Update is called once per frame
	void Update () {

		if(!tappedThisBeat && Input.GetKeyDown("down"))
		{
			tappedThisBeat = true;
			double diff = AudioSettings.dspTime - bg.GetBeatAbsoluteTime(bg.GetNextUpbeatIndex());
			TapVariance.Add(diff);

			Debug.Log("Avg delay with " + TapVariance.Count + " samples: " + CalcAvg());
		}
	}

	public void OnBeat()
	{
		
	}

	public void OnUpbeat()
	{
		if(!tappedThisBeat)
		{
			//RESET CALIBRATION
			TapVariance.Clear();
		}

		tappedThisBeat = false;
	}

	public double CalcAvg()
	{
		double sum = 0;
		for(int i = 0; i < TapVariance.Count; i++)
		{
			sum += TapVariance[i];
		}

		sum = sum / TapVariance.Count;
		return sum;
	}
}
