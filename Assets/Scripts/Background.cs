using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour {

    public float speed;
    public float lowestPoint;
    public float spawnPoint;

    private bool spawned = false;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        transform.position = (Vector2)transform.position - new Vector2(0, speed * Time.deltaTime);

        if (!spawned && transform.position.y < spawnPoint)
        {
            Instantiate(gameObject, transform.position + new Vector3(0, 8, 0), transform.rotation);
            spawned = true;
        }

        if(transform.position.y < lowestPoint)
        {
            Destroy(gameObject);
        }
	}
}
