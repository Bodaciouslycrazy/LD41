using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIState : MonoBehaviour {

    [Header("References")]
    [SerializeField]
    private Image RestartRef;
    //[SerializeField]
    //private Image StartGameRef;
    [SerializeField]
    private Image MainScreen;
    [SerializeField]
    private Image ByBodieScreen;

    [Header("Sprites")]
    [SerializeField]
    private Sprite TitleSprite;
    [SerializeField]
    private Sprite WinSprite;
    [SerializeField]
    private Sprite LoseSprite;

    public static UIState Singleton;

    private float TimeBucket = 0f;
    public float StartDelay = 2f;


    public enum State
    {
        TITLE,
        PLAYING,
        LOSE,
        WIN
    }
    private State CurState;

	// Use this for initialization
	void Start () {
		if(Singleton != null)
        {
            Destroy(Singleton);
        }

        Singleton = this;

        SetState(State.TITLE);
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKey("escape"))
            Application.Quit();

        if ((CurState == State.LOSE || CurState == State.WIN) && (Input.GetKeyDown("return") || Input.GetKeyDown("enter")))
        {
            Player.Freeze = true;
            EnemySpawner.Running = false;
            SceneManager.LoadScene("Test");
        }

        TimeBucket += Time.deltaTime;
        if (CurState == State.TITLE && TimeBucket > StartDelay)
        {
            UIState.Singleton.SetState(UIState.State.PLAYING);
            EnemySpawner.Running = true;
            Player.Freeze = false;
        }
    }

    public void SetState(State newState)
    {
        if (CurState == State.WIN || CurState == State.LOSE)
            return;

        CurState = newState;
        switch(CurState)
        {
            case State.TITLE:
                MainScreen.sprite = TitleSprite;
                MainScreen.enabled = true;
                RestartRef.enabled = false;
                //StartGameRef.enabled = true;
                ByBodieScreen.enabled = true;
                break;
            case State.PLAYING:
                MainScreen.enabled = false;
                RestartRef.enabled = false;
                //StartGameRef.enabled = false;
                ByBodieScreen.enabled = false;
                break;
            case State.LOSE:
                MainScreen.sprite = LoseSprite;
                MainScreen.enabled = true;
                RestartRef.enabled = true;
                //StartGameRef.enabled = false;
                ByBodieScreen.enabled = false;
                break;
            case State.WIN:
                MainScreen.sprite = WinSprite;
                MainScreen.enabled = true;
                RestartRef.enabled = true;
                //StartGameRef.enabled = false;
                ByBodieScreen.enabled = false;
                break;
        }
    }
}
