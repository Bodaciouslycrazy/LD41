using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Damageable, IBeatListener {

	public static Player Singleton;

	[SerializeField]
	private float MaxSpeed = 8f;
	[SerializeField]
	private bool Invincible = false;

	//[SerializeField]
	//private float BeatSync = 0f;

	protected BeatGenerator generator;

	public static bool Freeze = true;
	private bool Paused = false;

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

	[Header("Obj References")]
	[SerializeField]
	private AudioSource ASRocketSound;
	[SerializeField]
	private AudioSource ASLaserSound;


	private bool InputBlue = false;
	private bool InputGreen = false;
	private bool InputRed = false;



	/// <summary>
	/// Sets up the singleton.
	/// Adds the player to the beat generator.
	/// </summary>
	void Start () {
		if(Singleton == null)
		{
			Singleton = this;
		}
		else
		{
			Destroy(gameObject);
		}

		Application.targetFrameRate = 60;
		//QualitySettings.maxQueuedFrames = 0;
		generator = BeatGenerator.GetSingleton();
		generator.AddListener(this);
	}


	/// <summary>
	/// Manages all the movement from WASD.
	/// Does square to circle calculation thingey. Probably don't need to do this with joysticks.
	/// </summary>
	public void FixedUpdate()
	{
		if (Freeze || Paused)
			return;

		//Get input axies
		float horz = Input.GetAxis("Horizontal");
		float vert = Input.GetAxis("Vertical");

		//Calculate movement
		//if input is keyboard
		float xCirc = horz * Mathf.Sqrt(1f - (Mathf.Pow(vert, 2) / 2f));
		float yCirc = vert * Mathf.Sqrt(1f - (Mathf.Pow(horz, 2) / 2f));

		Vector2 inputVector = 1.2f * new Vector2(xCirc, yCirc);
		if (inputVector.magnitude > 1f)
			inputVector.Normalize();

		GetComponent<Rigidbody2D>().velocity = inputVector * MaxSpeed;

		LaserUpdate();
	}


	/// <summary>
	/// Update used for shooting lazers.
	/// Checks to make sure lazer is on beat.
	/// </summary>
	void Update () {
	
		
	}

	private void LaserUpdate()
	{
		if (Freeze || Paused)
		{
			return;
		}

		bool blue = Input.GetButton("FireBlue");
		bool green = Input.GetButton("FireGreen");
		bool red = Input.GetButton("FireRed");

		bool bluePressed = blue && !InputBlue;
		bool greenPressed = green && !InputGreen;
		bool redPressed = red && !InputRed;
		bool laserPressed = bluePressed || greenPressed || redPressed;

		InputBlue = blue;
		InputGreen = green;
		InputRed = red;

		//Check for firing on beat
		//ColorEnum? col = GetColorShot();

		if (Fired == false && laserPressed)
		{
			//Consume shot, even if it wasn't on beat.
			Fired = true;
			ColorEnum col = ColorEnum.BLUE;

			if (bluePressed)
				col = ColorEnum.BLUE;
			else if (greenPressed)
				col = ColorEnum.GREEN;
			else
				col = ColorEnum.RED;


			//Fire laser if player was inside the window.
			if (BeatGenerator.GetSingleton().IsInWindow())
			{
				//Fired on time!
				ASLaserSound.volume = 1f;

				//Do the raycast
				RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position, new Vector2(0, 1), 50f);
				Vector2 startLazer = (Vector2)transform.position + new Vector2(0, .5f);
				float lazDist = hit.distance == 0 ? 50 : hit.distance - .5f;
				Vector2 endLazer = (Vector2)transform.position + new Vector2(0, lazDist);
				ShootLazer((ColorEnum)col, startLazer, endLazer);

				if (hit.transform != null)
				{
					Damageable other = hit.transform.gameObject.GetComponent<Damageable>();
					if (other != null)
					{
						other.Damage(1, (ColorEnum)col);
					}
				}
			}
			else
			{
				//Missed the fire window...
				SoundMaker2D.Singleton.PlayClipAtPoint(soundMissBeat, Camera.main.transform, 1f);
				Instantiate(OffBeatPref, transform.position + new Vector3(0, 2, 0), Quaternion.identity);
			}
		}
	}


	/// <summary>
	/// Returns the color that the player shot this frame.
	/// Return null if the player didn't shoot.
	/// </summary>
	/// <returns></returns>
	/*
	private ColorEnum? GetColorShot()
	{
		bool blue = Input.GetKeyDown(KeyCode.LeftArrow);
		bool green = Input.GetKeyDown(KeyCode.DownArrow);
		bool red = Input.GetKeyDown(KeyCode.RightArrow);

		if (blue)
			return ColorEnum.BLUE;
		else if (green)
			return ColorEnum.GREEN;
		else if (red)
			return ColorEnum.RED;
		else
			return null;
	}
	*/


	/// <summary>
	/// Creates the lazer visual thingey.
	/// </summary>
	/// <param name="col"> Color </param>
	/// <param name="start"> Start position </param>
	/// <param name="end"> End Position </param>
	private void ShootLazer(ColorEnum col, Vector2 start, Vector2 end)
	{
		//Shake Camera
		CameraShake.ShakeCamera(.1f);

		//Insatntiate lazer
		Color c = new Color();
		if (col == ColorEnum.RED)
			c = Color.red;
		else if (col == ColorEnum.GREEN)
			c = Color.green;
		else if (col == ColorEnum.BLUE)
			c = Color.blue;
		else
			c = Color.white;

		GameObject obj = Instantiate(LazerShootPref, start, transform.rotation);
		obj.GetComponent<SpriteRenderer>().color = c;

		obj = Instantiate(LazerHitPref, end, transform.rotation);
		obj.GetComponent<SpriteRenderer>().color = c;

		obj = Instantiate(LazerBeamPref, Vector2.Lerp(start, end, 0.5f), transform.rotation);
		//obj.GetComponent<Transform>().localScale = new Vector3(1, Vector2.Distance(start, end) + .75f  , 1);
		obj.GetComponent<SpriteRenderer>().color = c;
		Vector2 newSize = obj.GetComponent<SpriteRenderer>().size;
		newSize.y = Vector2.Distance(start, end);
		obj.GetComponent<SpriteRenderer>().size = newSize; 
	}

	public void OnBeat()
	{

	}

	public void OnUpbeat()
	{
		Fired = false;

		//LASER SOUND EFFECT
		//Schedule the laser sound on the beat, but set its volume to 0.
		//Now, when they actually fire, just turn the folume up immediately.
		BeatGenerator bg = BeatGenerator.GetSingleton();

		float disp = bg.GetBeatAbsoluteTime(bg.GetNextBeatIndex()) - bg.GetSongCurrentTime();
		ASLaserSound.volume = 0f;
		ASLaserSound.PlayScheduled(AudioSettings.dspTime + disp);
	}


	/// <summary>
	/// Checks for collisions with bullets.
	/// When you collide with a bullet, hurt the player.
	/// </summary>
	/// <param name="collision"></param>
	public void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.tag == "Bullet")
		{
			Destroy(collision.gameObject);

			//Hurt self
			Damage(1,GetColor());
		}
	}

	/// <summary>
	/// Damages the player.
	/// </summary>
	/// <param name="amt"></param>
	/// <param name="damColor"></param>
	/// <returns></returns>
	public override int Damage(int amt, ColorEnum damColor)
	{
		//play sound?

		SoundMaker2D.Singleton.PlayClipAtPoint(soundDamage, transform.position, 1f);

		CameraShake.ShakeCamera(.2f);

		if (Invincible)
			return Health;
		else
			return base.Damage(amt, damColor);
	}


	/// <summary>
	/// Kills the player.
	/// </summary>
	public override void Kill()
	{
		//play sound?

		SoundMaker2D.Singleton.PlayClipAtPoint(soundKill, transform.position, 1f);

		generator.RemoveListener(this);
		Singleton = null;

		UIState.Singleton.SetState(UIState.State.LOSE);

		base.Kill();
	}

	#region PauseHandler

	void OnPauseGame()
	{
		Paused = true;

		ASRocketSound.Pause();
	}

	void OnResumeGame()
	{
		Paused = false;

		ASRocketSound.UnPause();
	}

	#endregion
}
