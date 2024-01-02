using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BumpSounds : MonoBehaviour
{

    [SerializeField] public bool isGiant;
    [HideInInspector] public AudioSource audioSource;
    [HideInInspector] public AudioSource childAudioSource;
    [SerializeField] private AudioClip[] bumpClips;
    [SerializeField] public AudioClip specialClip;
    [SerializeField] private AudioClip moveClip;

    private string blockName;

private void Awake()
{
        blockName = gameObject.tag;
        audioSource = GetComponent<AudioSource>();
}

    void Start()
    {
        if (transform.childCount > 0)
        {
            childAudioSource = transform.GetChild(0).GetComponent<AudioSource>();
        }
    }

    public void PlayMoveSound()
    {
        childAudioSource.Play();
    }

private void OnCollisionEnter(Collision other)
{
    string otherTag = other.gameObject.tag;
    switch (blockName)
    {
        case "Player":
            if (otherTag != "Enemy" && otherTag != "Destroyemy" && otherTag != "Explode" && otherTag != "Glass")
            {
                //if (otherTag == "Plane" && GetComponent<BallUserControl>().CanJump == true)
                //{
                //    break;
                //}
                PlayBumpClip();
            }
        break;
        case "Neutral":
            if (otherTag == "Plane") //(otherTag == "Neutral" || otherTag == "Enemy")
                {
                PlayBumpClip();
            }
        break;
        case "Glass":
            if (otherTag == "Plane") //(otherTag == "Neutral" || otherTag == "Enemy" || otherTag == "Glass" || otherTag == "Destroy" || otherTag == "Destroyemy")
                {
                PlayBumpClip();
            }
        break;
        case "Enemy":
            if (otherTag == "Plane") //(otherTag == "Neutral" || otherTag == "Enemy")
                {
                PlayBumpClip();
            }
        break;
        case "Destroyer":
            if (otherTag == "Plane") //(otherTag == "Glass" || otherTag == "Destroy" || otherTag == "Destroyemy")
                {
                PlayBumpClip();
            }
            if (otherTag == "Neutral" || otherTag == "Enemy" || otherTag == "Explode")
            {
                audioSource.PlayOneShot(specialClip);
            }
        break;
        case "Destroyemy":
            if (otherTag == "Plane") //(otherTag == "Glass" || otherTag == "Destroy" || otherTag == "Destroyemy")
            {
                PlayBumpClip();
            }
            if (otherTag == "Neutral" || otherTag == "Enemy" || otherTag == "Explode")
            {
                audioSource.PlayOneShot(specialClip);
            }
        break;
        default:
        break;
    }
}
    public void PlayBumpClip()
    {
        int index = Random.Range(0, bumpClips.Length);
        if (audioSource.enabled && index != 0)
        {
        AudioClip bumpClip = bumpClips[index];
        audioSource.PlayOneShot(bumpClip);
        }
    }

}
