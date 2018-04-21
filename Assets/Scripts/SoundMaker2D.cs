using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMaker2D : MonoBehaviour {

    public static SoundMaker2D Singleton;

    [SerializeField]
    private GameObject pref;

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
	}

    public void PlayClipAtPoint(AudioClip clip, Vector2 pos, float vol = 1f)
    {
        GameObject obj = Instantiate(pref, pos, Quaternion.identity);

        obj.GetComponent<AudioSource>().clip = clip;
        obj.GetComponent<AudioSource>().volume = vol;
        obj.GetComponent<AudioSource>().Play();
    }
}
