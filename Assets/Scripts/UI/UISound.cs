using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UISound : MonoBehaviour, ISelectHandler, ICancelHandler, ISubmitHandler {

	public AudioClip SoundSelect;
	public AudioClip SoundSubmit;
	public AudioClip SoundCancel;

	public void OnSelect(BaseEventData eventData)
	{
		SoundMaker2D.Singleton.PlayClipAtPoint(SoundSelect, Camera.main.transform);
	}

	public void OnCancel(BaseEventData eventData)
	{
		SoundMaker2D.Singleton.PlayClipAtPoint(SoundCancel, Camera.main.transform);
	}

	public void OnSubmit(BaseEventData eventData)
	{
		SoundMaker2D.Singleton.PlayClipAtPoint(SoundSubmit, Camera.main.transform);
	}
}
