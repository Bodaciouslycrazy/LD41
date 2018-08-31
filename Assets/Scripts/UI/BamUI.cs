using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class BamUI : MonoBehaviour {


	private RectTransform BamTransform { get { return GetComponent<RectTransform>(); } }

	// Use this for initialization
	void Start () {
		//BamTransform = GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown("s"))
			Show();
		else if (Input.GetKeyDown("h"))
			Hide();
	}


	public void Show( bool instant = false)
	{
		SetInteractable(true);

		BamTransform.anchoredPosition = new Vector2(0, 0);
	}

	public void Hide( bool instant = false)
	{
		SetInteractable(false);

		float move = BamTransform.parent.GetComponent<RectTransform>().sizeDelta.x;

		BamTransform.anchoredPosition = new Vector2(move, 0);
	}

	private void SetInteractable( bool set )
	{
		SetInteractableRecursive(transform, set);
	}

	private void SetInteractableRecursive( Transform parent, bool set )
	{

		for(int i = 0; i < parent.childCount; i++)
		{
			Selectable sel = parent.GetChild(i).GetComponent<Selectable>();

			if (sel != null)
				sel.interactable = set;
		}

		for(int i = 0; i < parent.childCount; i++)
		{
			SetInteractableRecursive(parent.transform.GetChild(i), set);
		}

	}

}
