using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Damageable, IBeatListener {

    public static Player Singleton;

    [SerializeField]
    private float MaxSpeed = 8f;

    [SerializeField]
    [Range(0, .5f)]
    private float BeatWindow = .2f;
    [SerializeField]
    private float BeatSync = 0f;

    protected BeatGenerator generator;

    private bool Fired = false;

    [Header("Sounds")]
    [SerializeField]
    private AudioClip soundFire;
    [SerializeField]
    private AudioClip soundMissBeat;
    [SerializeField]
    private AudioClip soundDamage;
    [SerializeField]
    private AudioClip soundKill;
    //[SerializeField]
    //private AudioClip soundWrongColor;

    [Header("Prefs")]
    [SerializeField]
    private GameObject LazerShootPref;
    [SerializeField]
    private GameObject LazerBeamPref;
    [SerializeField]
    private GameObject LazerHitPref;
    [SerializeField]
    private GameObject OffBeatPref;

	// Use this for initialization
	void Start () {
        if(Singleton == null)
        {
            Singleton = this;
        }
        else
        {
            Destroy(gameObject);
        }

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
        int col = GetColorShot();

        if( Fired == false && col != -1 )
        {
            //Consume shot, even if it wasn't on beat.
            Fired = true;

            //Check that shot was on beat
            double window = (60.0 / generator.GetBPM()) * BeatWindow;

            if( AudioSettings.dspTime > generator.GetNextBeat() - window || AudioSettings.dspTime < generator.GetLastBeat() + window)
            {
                //Fire on time!
                GetComponent<AudioSource>().PlayOneShot(soundFire, 1f);

                //Do the raycast
                RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position, new Vector2(0, 1), 50f);
                Vector2 startLazer = (Vector2)transform.position + new Vector2(0, .5f);
                float lazDist = hit.distance == 0 ? 50 : hit.distance - .5f;
                Vector2 endLazer = (Vector2)transform.position + new Vector2(0, lazDist);
                ShootLazer(col, startLazer, endLazer);

                if (hit.transform != null)
                {
                    Damageable other = hit.transform.gameObject.GetComponent<Damageable>();
                    if(other != null && other.GetColor() == col)
                    {
                        other.Damage(1);
                    }
                }
            }
            else
            {
                //Missed the fire window...
                GetComponent<AudioSource>().PlayOneShot(soundMissBeat, 1f);
                Instantiate(OffBeatPref, transform.position + new Vector3(0,2,0), Quaternion.identity);
            }
        }
	}

    private int GetColorShot()
    {
        bool blue = Input.GetKeyDown(KeyCode.LeftArrow);
        bool green = Input.GetKeyDown(KeyCode.DownArrow);
        bool red = Input.GetKeyDown(KeyCode.RightArrow);

        if (blue)
            return Damageable.BLUE;
        else if (green)
            return Damageable.GREEN;
        else if (red)
            return Damageable.RED;
        else
            return -1;
    }

    private void ShootLazer(int num, Vector2 start, Vector2 end)
    {
        //Shake Camera
        CameraShake.ShakeCamera(.1f);

        //Insatntiate lazer
        Color c = new Color();
        if (num == Damageable.RED)
            c = Color.red;
        else if (num == Damageable.GREEN)
            c = Color.green;
        else if (num == Damageable.BLUE)
            c = Color.blue;
        else
            c = Color.white;

        GameObject obj = Instantiate(LazerShootPref, start, transform.rotation);
        obj.GetComponent<SpriteRenderer>().color = c;

        obj = Instantiate(LazerHitPref, end, transform.rotation);
        obj.GetComponent<SpriteRenderer>().color = c;

        obj = Instantiate(LazerBeamPref, Vector2.Lerp(start, end, 0.5f), transform.rotation);
        obj.GetComponent<Transform>().localScale = new Vector3(1, Vector2.Distance(start, end) + .75f  , 1);
        obj.GetComponent<SpriteRenderer>().color = c;
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

        SoundMaker2D.Singleton.PlayClipAtPoint(soundDamage, transform.position, 1f);

        CameraShake.ShakeCamera(.2f);

        return base.Damage(amt);
    }

    public override void Kill()
    {
        //play sound?

        SoundMaker2D.Singleton.PlayClipAtPoint(soundKill, transform.position, 1f);

        generator.RemoveListener(this);
        Singleton = null;

        GameOver.IsGameOver = true;

        base.Kill();
    }
}
