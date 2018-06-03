using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBeat : MonoBehaviour, IBeatListener {

    public float BeatDip = 30f;
    private float DipStart = 0;
    private float DipEnd = 0;

    Vector2 Center;

    RectTransform rect;

	// Use this for initialization
	void Start () {
        BeatGenerator.GetSingleton().AddListener(this);
        rect = GetComponent<RectTransform>();
        Center = rect.anchoredPosition;
	}

    // Update is called once per frame
    void Update()
    {
        float dip = 0;
        if (Time.time < DipEnd)
        {
            dip = 1 - ((Time.time - DipStart) / (DipEnd - DipStart));
        }

        rect.anchoredPosition = Center + new Vector2(0, dip * -BeatDip);
    }

    public void OnBeat()
    {
        DipStart = Time.time;
        DipEnd = DipStart + .2f;
    }

    public void OnUpbeat()
    {
        //do nothing?
    }
}
