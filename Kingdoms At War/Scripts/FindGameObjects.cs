using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindGameObjects : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
            AudioSource[] audios= FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
    foreach(var a in audios)
    {
        Debug.Log(a.gameObject.name + " has an audio source on it");
    }

    }

}
