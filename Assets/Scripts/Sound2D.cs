using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound2D : MonoBehaviour {

    [SerializeField]
    private GameObject pref;

    private void Update()
    {
        if(!GetComponent<AudioSource>().isPlaying)
        {
            Destroy(gameObject);
        }
    }
}
