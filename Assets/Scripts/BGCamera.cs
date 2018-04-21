using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGCamera : MonoBehaviour {

    public float Speed = .5f;

    private float dist = 8;
    private float startY = 0;

	// Use this for initialization
	void Start () {
        startY = transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
        transform.position += new Vector3(0, Speed * Time.deltaTime, 0);

        if(transform.position.y - startY > dist)
        {
            transform.position -= new Vector3(0, dist, 0);
        }
	}
}
