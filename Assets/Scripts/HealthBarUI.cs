using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour {

    public Damageable PlayerRef;


	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        float perc = PlayerRef.GetHealth() * 1f / PlayerRef.GetMaxHealth();

        GetComponent<Image>().fillAmount = perc;
	}

}
