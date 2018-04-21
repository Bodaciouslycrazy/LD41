using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    private float Timeout = 5f;

	// Use this for initialization
	void Start () {
        Timeout = Time.time + Timeout;
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.time > Timeout)
        {
            Destroy(gameObject);
        }
	}

    public void SetMotion(Vector2 vel)
    {
        GetComponent<Rigidbody2D>().velocity = vel;
    }
}
