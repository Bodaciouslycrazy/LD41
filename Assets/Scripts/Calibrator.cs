using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Calibrator : MonoBehaviour, IBeatListener{

	public AudioClip song;

	public TextMeshProUGUI Countdown;
	public TextMeshProUGUI BeatDelayDisplay;

	private bool tappedThisBeat = false;

	private List<float> TapDisplacement;
	//private List<int> TapBeats;

	private BeatGenerator bg;
	
	// Use this for initialization
	void Start () {
		bg = BeatGenerator.GetSingleton();
		bg.AddListener(this);
		TapDisplacement = new List<float>();

		//bg.StartSong( bg.GetBPM(), bg.mainSong);
		//bg.SetMetronome(true);
		//TapBeats = new List<int>();
	}
	
	// Update is called once per frame
	void Update () {
		if(!tappedThisBeat && Input.GetAxis("FireGreen") >= 1f)
		{
			tappedThisBeat = true;
			float diff = bg.GetSongPosition() - bg.GetBeatAbsoluteTime(bg.GetNextUpbeatIndex());
			TapDisplacement.Add(diff);

			Debug.Log("SAMPLES:\t" + TapDisplacement.Count + "\nVARIENCE:\t" + CalcVarFromAvg() + "\nAVG:\t" + CalcAvg());
		}

		if( TapDisplacement.Count >= 24 )
		{
			FinishCalibration();
		}
	}

	public void StartCalibration()
	{
		TapDisplacement.Clear();
		bg.StartSong(bg.GetBPM(), song);
	}

	public void FinishCalibration()
	{
		//Stop the beat generator?
		bg.StopSong();
	}

	public void OnBeat()
	{
		int beatnum = bg.GetNextBeatIndex();

		string countdown = "";

		switch(beatnum)
		{
			case 1:
				countdown = "3";
				break;
			case 2:
				countdown = "2";
				break;
			case 3:
				countdown = "1";
				break;
			case 4:
				countdown = "GO!";
				break;
			default:
				countdown = "";
				break;
		}

		Countdown.text = countdown;
	}

	public void OnUpbeat()
	{
		if(!tappedThisBeat)
		{
			//RESET CALIBRATION?
			//TapDisplacement.Clear();
		}

		tappedThisBeat = false;
	}

	public float CalcAvg()
	{
		float sum = 0;
		for(int i = 0; i < TapDisplacement.Count; i++)
		{
			sum += TapDisplacement[i];
		}

		sum = sum / TapDisplacement.Count;
		return sum;
	}

	public float CalcVarFromAvg()
	{
		float avg = CalcAvg();

		float sum = 0;
		for(int i = 0; i < TapDisplacement.Count; i++)
		{
			sum += Mathf.Abs(avg - TapDisplacement[i]);
		}

		sum = sum / TapDisplacement.Count;
		return sum;
	}

	public void ChangeBeatDelay(float val)
	{
		/*
		float newBeatDelay = bg.GetBeatDelay() + val;
		bg.SetBeatDelay(newBeatDelay);

		BeatDelayDisplay.SetText( string.Format("{0:0.00}", newBeatDelay ));
		*/
	}
}
