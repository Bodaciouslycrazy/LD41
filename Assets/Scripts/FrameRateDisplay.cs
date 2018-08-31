using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FrameRateDisplay : MonoBehaviour {

	TextMeshProUGUI disp;

	// Use this for initialization
	void Start () {
		disp = GetComponent<TextMeshProUGUI>();
	}
	
	// Update is called once per frame
	void Update () {
		float fr = 1f / Time.deltaTime;
		disp.text = string.Format("{0:00.0}", fr);
	}
}
