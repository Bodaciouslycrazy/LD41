using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour {

    public static bool IsGameOver = false;

    [SerializeField]
    private Image GameOverImage;
    [SerializeField]
    private Image RestartImage;

	// Use this for initialization
	void Start () {
        GameOverImage.enabled = false;
        RestartImage.enabled = false;

	}
	
	// Update is called once per frame
	void Update () {
        if (!IsGameOver)
            return;

        GameOverImage.enabled = true;
        RestartImage.enabled = true;

        if(Input.GetKeyDown("return") || Input.GetKeyDown("enter"))
        {
            Debug.Log("RESTARTING");

            IsGameOver = false;
            SceneManager.LoadScene("Test");
        }
	}

}
