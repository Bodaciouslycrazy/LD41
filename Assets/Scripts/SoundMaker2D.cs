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
		if (clip == null || vol <= 0f)
			return;

        GameObject obj = Instantiate(pref, pos, Quaternion.identity);

        obj.GetComponent<AudioSource>().clip = clip;
        obj.GetComponent<AudioSource>().volume = vol;
        obj.GetComponent<AudioSource>().Play();
    }

	public void PlayClipAtPoint(AudioClip clip, Transform parent, float vol = 1f)
	{
		if (clip == null || vol <= 0f)
			return;

		GameObject obj = Instantiate(pref, Vector2.zero, Quaternion.identity);

		obj.transform.SetParent(parent);

		obj.GetComponent<AudioSource>().clip = clip;
		obj.GetComponent<AudioSource>().volume = vol;
		obj.GetComponent<AudioSource>().Play();
	}
}
