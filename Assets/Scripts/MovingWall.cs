using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingWall : MonoBehaviour {

    public float Speed = 2f;

    private float dist = 32;
    private float minY = -16;

    // Use this for initialization
    void Start()
    {
        //startY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position -= new Vector3(0, Speed * Time.deltaTime, 0);

        if (transform.position.y < minY)
        {
            transform.position += new Vector3(0, dist, 0);
        }
    }
}
