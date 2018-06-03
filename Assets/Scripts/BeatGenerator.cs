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
    protected bool PlayMetronome = true;


    //********Private variables.********

    //protected double TempoStart = 0;
    protected int NextBeat = 0;
    protected int NextUpbeat = 0;
    protected bool running = false;

    protected float BeatDelay = .0f;
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
        MusicPlayer.PlayScheduled(AudioSettings.dspTime);
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
            MetronomePlayer.PlayScheduled(GetBeatAbsoluteTime(NextBeat) - BeatDelay);
        }
    }

    public bool IsInWindow(float adjust = 0f)
    {
        //Which beat do I compare to?
        //int compBeat = NextBeat;
        int compBeat = NextUpbeat;
        float curTime = MusicPlayer.time + adjust + BeatDelay;
        
        /*
        if (Mathf.Abs((float)(curTime - GetBeatAbsoluteTime(compBeat - 1))) < Mathf.Abs((float)(curTime - GetBeatAbsoluteTime(compBeat))))
            compBeat -= 1;
            */
        

        double diff = Mathf.Abs(curTime - GetBeatAbsoluteTime(compBeat));


        TimingPanel.Singleton.AddTick( (curTime - GetBeatAbsoluteTime(compBeat)) / (60f / BPM) );

        return (diff <= (60.0 / BPM) * (Window / 2.0));
    }

    #region GettersAndSetters

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
        return (beat * 60f / BPM) + BeatDelay;
    }

    public float GetUpbeatAbsoluteTime(int upbeat)
    {
        return GetBeatAbsoluteTime(upbeat) + ((60f / BPM) / 2f);
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
