using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Damageable {

    private const float BoundLeft = -10f;
    private const float BoundRight = 10f;
    private const float BoundUp = 7f;
    private const float BoundDown = -7f;

    [SerializeField]
    private AudioClip destroySound;


    private float debugSpeed = 0;

    //Fixed update called less, will cause less lag.
    private void FixedUpdate()
    {

        Vector2 pos = transform.position;

        if (pos.x < BoundLeft || pos.x > BoundRight || pos.y < BoundDown || pos.y > BoundUp)
        {
            Destroy(gameObject);
        }
    }

    public void SetMotion(Vector2 vel)
    {
        debugSpeed = vel.magnitude;
        GetComponent<Rigidbody2D>().velocity = vel;
    }

    public void SetMotion(float ang, float speed)
    {
        SetMotion( new Vector2(Mathf.Cos(ang), Mathf.Sin(ang)) * speed);
    }

    public override void Kill()
    {
        //AudioSource.PlayClipAtPoint(destroySound, transform.position);
        SoundMaker2D.Singleton.PlayClipAtPoint(destroySound, transform.position);
        base.Kill();
    }
}
