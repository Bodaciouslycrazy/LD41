using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {

    private Vector3 Center;

    private static float Intensity = 0;
    private static float Length = 2f;
    private static float StartTime = 0;

	// Use this for initialization
	void Start () {
        Center = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		
        if(Time.time < StartTime + Length)
        {
            float perc = Intensity * Mathf.Pow( 0.5f , 10 * (Time.time-StartTime) );

            float radius = Random.Range(0f, perc);
            float angle = Random.Range(0f, 2 * Mathf.PI);

            Vector3 disp = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;
            transform.position = Center + disp;
        }
        else
        {
            Intensity = 0;
            transform.position = Center;
        }
	}

    public static void ShakeCamera(float its)
    {
        float perc = Intensity * Mathf.Pow(0.5f, 3 * (Time.time - StartTime));

        if (its > perc)
        {
            Intensity = its;
            StartTime = Time.time;
        }
    }
}
