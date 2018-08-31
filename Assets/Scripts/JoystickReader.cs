using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class JoystickReader : MonoBehaviour {

	private TextMeshProUGUI text; 

	// Use this for initialization
	void Start () {
		text = GetComponent<TextMeshProUGUI>();
	}
	
	// Update is called once per frame
	void Update () {
		text.text = "X: " + Input.GetAxis("Horizontal") + "\nY: " + Input.GetAxis("Vertical");
	}
}
