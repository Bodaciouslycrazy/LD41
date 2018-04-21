using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Damageable, IBeatListener {

    [SerializeField]
    private float MaxSpeed = 8f;

    [SerializeField]
    [Range(0, .5f)]
    private float BeatWindow = .2f;

    protected BeatGenerator generator;

    private bool Fired = false;

    [Header("Sounds")]
    [SerializeField]
    private AudioClip soundFire;
    [SerializeField]
    private AudioClip soundMissBeat;

	// Use this for initialization
	void Start () {
        generator = BeatGenerator.GetSingleton();
        generator.AddListener(this);
	}
	
	// Update is called once per frame
	void Update () {

        //Get input axies
        float horz = Input.GetAxis("Horizontal");
        float vert = Input.GetAxis("Vertical");

        //Calculate movement
        //if input is keyboard
        float xCirc = horz * Mathf.Sqrt(1f - (Mathf.Pow(vert, 2) / 2f) );
        float yCirc = vert * Mathf.Sqrt(1f - (Mathf.Pow(horz, 2) / 2f) );

        GetComponent<Rigidbody2D>().velocity = new Vector2(xCirc, yCirc) * MaxSpeed;


        //Check for firing on beat
        bool blue = Input.GetKeyDown(KeyCode.LeftArrow);
        bool green = Input.GetKeyDown(KeyCode.DownArrow);
        bool red = Input.GetKeyDown(KeyCode.RightArrow);

        if( Fired == false && (blue || green || red ) )
        {
            //Consume shot, even if it wasn't on beat.
            Fired = true;

            //Check that shot was on beat
            double window = (60.0 / generator.GetBPM()) * BeatWindow;

            if( AudioSettings.dspTime > generator.GetNextBeat() - window || AudioSettings.dspTime < generator.GetLastBeat() + window)
            {
                //Fire on time!
                GetComponent<AudioSource>().PlayOneShot(soundFire, 1f);
            }
            else
            {
                //Missed the fire window...
                GetComponent<AudioSource>().PlayOneShot(soundMissBeat, 1f);
            }
        }
	}

    public void OnBeat()
    {

        /*
        oldPos = targetPos;
        targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        easeStart = Time.time;
        easeEnd = easeStart + (60f / (float)generator.GetBPM() ) * 0.8f;
        */
    }

    public void OnUpbeat()
    {
        Fired = false;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Bullet")
        {
            Destroy(collision.gameObject);

            //Hurt self
            Damage(1);
        }
    }

    //Methods from Damageable
    public override int Damage(int amt)
    {
        //play sound?

        return base.Damage(amt);
    }

    public override void Kill()
    {
        //play sound?

        generator.RemoveListener(this);

        base.Kill();
    }
}
