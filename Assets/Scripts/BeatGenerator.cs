using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BeatGenerator : MonoBehaviour {

    protected static BeatGenerator Singleton;

    [SerializeField]
    private AudioClip mainSong;

    [SerializeField]
    protected double BPM = 160.0;
    protected double nextBeat = 0;
    protected double nextUpbeat = 0;

    [SerializeField]
    private AudioSource MetronomePlayer;
    [SerializeField]
    private AudioSource MusicPlayer;


    protected List<IBeatListener> Listeners;

    [SerializeField]
    protected bool PlayMetronome = true;
    protected bool tickPlayed = false;

    public bool running = true;


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
        StartSong(120.0, mainSong, 2, -.03);
	}

    public void StartSong( double bpm, AudioClip song, double delay, double offset)
    {
        BPM = bpm;

        nextBeat = AudioSettings.dspTime + delay;
        nextUpbeat = nextBeat + ((60.0 / BPM) / 2.0);

        MusicPlayer.clip = song;
        MusicPlayer.PlayScheduled(nextBeat + offset);
    }

    public static BeatGenerator GetSingleton()
    {
        return Singleton;
    }
	
	// Update is called once per frame
	void Update () {
        if (!running)
            return;

        if(!MusicPlayer.isPlaying)
        {
            //Start song again!
            StartSong(BPM, MusicPlayer.clip, .01, 0);
        }

        if(AudioSettings.dspTime >= nextBeat)
        {
            tickPlayed = true;
            CallBeat();

            //Calculate next tick, and send a schedule for the audio player.
            nextBeat += (60.0 / BPM);
        }

        if (AudioSettings.dspTime >= nextUpbeat)
        {
            CallUpbeat();

            //Calculate next tick, and send a schedule for the audio player.
            nextUpbeat += (60.0 / BPM);
        }

        if (PlayMetronome && tickPlayed && !MetronomePlayer.isPlaying)
        {
            tickPlayed = false;
            MetronomePlayer.PlayScheduled(nextBeat);
        }
	}

    protected void CallBeat()
    {
        //Debug.Log("Calling " + Listeners.Count + " listeners.");
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
    }

    public double GetNextBeat()
    {
        return nextBeat;
    }

    public double GetLastBeat()
    {
        return nextBeat - (60.0 / BPM);
    }

    public double GetNextUpbeat()
    {
        return nextUpbeat;
    }

    public double GetLastUpbeat()
    {
        return nextUpbeat - (60.0 / BPM);
    }

    public double GetBPM()
    {
        return BPM;
    }

    public void SetBPM(double bpm)
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
}
