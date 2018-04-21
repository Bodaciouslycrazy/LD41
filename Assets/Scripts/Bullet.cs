using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Damageable {

    private float Timeout = 5f;

    [SerializeField]
    private AudioClip destroySound;

	// Use this for initialization
	void Start () {
        Timeout = Time.time + Timeout;
        MaxHealth = 1;
        Health = 1;
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

    public override void Kill()
    {
        //AudioSource.PlayClipAtPoint(destroySound, transform.position);
        SoundMaker2D.Singleton.PlayClipAtPoint(destroySound, transform.position);
        base.Kill();
    }
}
