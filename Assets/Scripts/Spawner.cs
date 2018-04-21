using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    [SerializeField]
    protected float spawnTime = .3f;
    protected float timeBucket = 0f;

    [SerializeField]
    protected float velocity = 2f;

    [SerializeField]
    protected GameObject bulletPref;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        timeBucket += Time.deltaTime;

        while(timeBucket > 0)
        {
            GameObject obj = Instantiate(bulletPref, transform.position, transform.rotation);

            float ang = Random.Range(0, Mathf.PI);

            Vector2 vector = new Vector2(Mathf.Cos(ang), -Mathf.Sin(ang));

            obj.GetComponent<Bullet>().SetMotion(vector * velocity);


            timeBucket -= spawnTime;
        }
	}
}
