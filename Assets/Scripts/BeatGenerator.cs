using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BeatGenerator : MonoBehaviour {

    protected static BeatGenerator Singleton;

    
    [Header("Scene References")]
    [SerializeField]
    private AudioSource MetronomePlayer;
    [SerializeField]
    private AudioSource MusicPlayer;

    [Header("Variables")]
    [SerializeField]
    public AudioClip mainSong;
    [SerializeField]
    protected float BPM = 160f;
    [SerializeField]
    [Range(0, .5f)]
    protected float Window = .2f;
	[SerializeField]
	protected bool running = false;
	[SerializeField]
    protected bool PlayMetronome = true;



    //********Private variables.********

    //protected double TempoStart = 0;
    protected int NextBeat = 0;
    protected int NextUpbeat = 0;

	//protected float BeatDelay = 0f;
	//protected float WindowAdjust = 0f;
    //protected double AudioDelay = 0;

    protected List<IBeatListener> Listeners;

    
    protected bool tickPlayed = false;


    private void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    // Use this for initialization
    void Start () {
        Listeners = new List<IBeatListener>();
        MetronomePlayer = GetComponent<AudioSource>();

        //Set up timing
        //nextBeat = AudioSettings.dspTime;
        //nextUpbeat = nextBeat + ((60.0 / BPM) / 2.0);
        //StartSong(120.0, mainSong, 2, -.03);
	}

    public static BeatGenerator GetSingleton()
    {
        return Singleton;
    }


    public void StartSong( float bpm, AudioClip song)
    {
        BPM = bpm;

        running = true;
        NextBeat = 0;
        NextUpbeat = 0;

        //BeatDelay = bdelay;
        //AudioDelay = adelay;

        //TempoStart = AudioSettings.dspTime + BeatDelay;

        MusicPlayer.clip = song;
        MusicPlayer.PlayScheduled(AudioSettings.dspTime + 0.1);
    }

	public void StopSong()
	{
		running = false;
		MusicPlayer.Stop();
	}
	

	// Update is called once per frame
	void Update () {
        if (!running)
            return;

        //Turn on the metronome.
        if(Input.GetKeyDown("m"))
        {
            PlayMetronome = !PlayMetronome;
        }

        
		
        if(!MusicPlayer.isPlaying && !PlayMetronome)
        {
            //Start song again!
            StartSong(BPM, MusicPlayer.clip);
        }
		
        

        //Check if the next beat has been played
        if( MusicPlayer.time >= GetBeatAbsoluteTime(NextBeat))
        {
            tickPlayed = true;
            CallBeat();
        }

        //Check if the next upbeat has been played
        if ( MusicPlayer.time >= GetUpbeatAbsoluteTime(NextUpbeat))
        {
            CallUpbeat();
        }

        //move the beat to the next beat
        while( MusicPlayer.time >= GetBeatAbsoluteTime(NextBeat))
        {
            NextBeat++;
        }

        //move the upbeat to the next upbeat
        while( MusicPlayer.time >= GetUpbeatAbsoluteTime(NextUpbeat))
        {
            NextUpbeat++;
        }
	}

    protected void CallBeat()
    {
        for(int i = 0; i < Listeners.Count; i++)
        {
            Listeners[i].OnBeat();
        }

		
    }

    protected void CallUpbeat()
    {
        for (int i = 0; i < Listeners.Count; i++)
        {
            Listeners[i].OnUpbeat();
        }

        if(PlayMetronome)
        {
			float playDistance = GetBeatAbsoluteTime(NextBeat) - MusicPlayer.time;

            MetronomePlayer.PlayScheduled(AudioSettings.dspTime +  playDistance);
			//Debug.Log("met schedulued: " + (GetBeatAbsoluteTime(NextBeat) - BeatDelay));
        }
    }

    public bool IsInWindow()
    {
		float inputDelay = .07f;

        int compBeat = NextUpbeat;
		float pressTime = MusicPlayer.time - inputDelay; // + WindowAdjust;

		float beatTime = GetBeatAbsoluteTime(compBeat);

		float windowSize = (60f / BPM) * Window;

		TimingPanel.Singleton.AddTick((pressTime - beatTime) / (60f / BPM));

		return (beatTime - (windowSize / 2f) <= pressTime) && (pressTime <= beatTime + (windowSize / 2f));
    }

	#region PauseFunctions

	void OnPauseGame()
	{
		running = false;

		MusicPlayer.Pause();
	}

	void OnResumeGame()
	{
		running = true;

		MusicPlayer.UnPause();
	}

	#endregion

	#region GettersAndSetters

	public float GetSongPosition()
	{
		return MusicPlayer.time;
	}

	public int GetNextBeatIndex()
    {
        return NextBeat;
    }

    public int GetNextUpbeatIndex()
    {
        return NextUpbeat;
    }

    public float GetBeatAbsoluteTime(int beat)
    {
        return (beat * 60f / BPM);
    }

    public float GetUpbeatAbsoluteTime(int upbeat)
    {
        return GetBeatAbsoluteTime(upbeat) + ((60f / BPM) / 2f);
    }

	public float GetSongCurrentTime()
	{
		return MusicPlayer.time;
	}

	public float GetWindow()
	{
		return Window;
	}

    public float GetBPM()
    {
        return BPM;
    }

    public void SetBPM(float bpm)
    {
        BPM = bpm;
    }

    public void AddListener( IBeatListener lsn )
    {
        Listeners.Add(lsn);
    }

    public void RemoveListener( IBeatListener lsn )
    {
        Listeners.Remove(lsn);
    }

    public void SetMetronome(bool met)
    {
        PlayMetronome = met;
    }

    #endregion
}
