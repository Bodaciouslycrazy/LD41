using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimingPanel : MonoBehaviour {

    public static TimingPanel Singleton;

    [SerializeField]
    private GameObject TickPref;
	[SerializeField]
	private RectTransform LeftWindow;
	[SerializeField]
	private RectTransform RightWindow;
    [SerializeField]
    private int MaxTicks = 3;


    private List<GameObject> Ticks;

	// Use this for initialization
	void Start () {
        Singleton = this;

        Ticks = new List<GameObject>();

		//Initialize window ticks to match the current beat window.
		float winDist = BeatGenerator.GetSingleton().GetWindow() * 100;

		LeftWindow.anchoredPosition = new Vector2(-winDist, 0);
		RightWindow.anchoredPosition = new Vector2(winDist, 0);
	}
	
	public void AddTick(float perc)
    {
        if (Ticks.Count == MaxTicks)
        {
            Destroy(Ticks[0]);
            Ticks.RemoveAt(0);
        }

        float x = perc * 200;

        GameObject tick = Instantiate(TickPref, transform, false);
        Ticks.Add(tick);


        tick.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, 0);
    }
}
