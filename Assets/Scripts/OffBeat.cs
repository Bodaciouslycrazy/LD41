using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffBeat : MonoBehaviour {

    public float Timeout = .5f;
    public float Speed = 1f;
	
	// Update is called once per frame
	void Update () {
        Timeout -= Time.deltaTime;

        if(Timeout < 0)
        {
            Destroy(gameObject);
        }
        else
        {
            transform.position = (Vector2)transform.position + (Vector2.down * Speed * Time.deltaTime);
        }
	}
}
