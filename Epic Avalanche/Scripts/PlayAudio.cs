using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudio : MonoBehaviour
{
    [SerializeField] AudioClip[] clips;
    [SerializeField] private bool playAudioOnStart;
    [SerializeField] private bool playAudioOnTrigger;
    [SerializeField] private bool loopRandomly;
    [SerializeField] private bool playAudioAfterDelay;
    [SerializeField] private float delayClipTime;
    [SerializeField] private bool deactivateAfterAudioPlayed;
    [SerializeField] private bool destroyAfterAudioPlayed;

    private bool oneTimeAudioPlayed;
    AudioSource audioSource;
    AudioClip clipToPlay;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (playAudioAfterDelay)
        {
            Invoke("PlayRandomAudio", delayClipTime);
        }
        else if (loopRandomly)
        {
            StartCoroutine(PlayRandomAudioAfterTime());
        }
        else if (playAudioOnStart)
        {
            PlayRandomAudio();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (playAudioOnTrigger)
        {
            if (playAudioAfterDelay)
            {
            Invoke("PlayRandomAudio", delayClipTime);
            }
        }
    }

    public void PlayRandomAudio()
    {
        if (clips != null)
        {
            int arrayIndex = Random.Range(0, clips.Length);
            clipToPlay = clips[arrayIndex];
            audioSource.PlayOneShot(clipToPlay);
            if(deactivateAfterAudioPlayed)
            {
                DeactivateAudio();
            }
            else if (destroyAfterAudioPlayed)
            {
                DestroyAudio();
            }
        }
        else
        {
            Debug.Log("AudioArray on " + gameObject.name + " is empty.");
        }
    }

    private void DeactivateAudio()
    {
        this.gameObject.SetActive(false);
    }

    private void DestroyAudio()
    {
        Destroy(this.gameObject);
    }

    private IEnumerator PlayRandomAudioAfterTime(float delay = 1)
    {
        yield return new WaitForSeconds(delay);

        if (clips != null)
        {
            int arrayIndex = Random.Range(0, clips.Length);
            clipToPlay = clips[arrayIndex];
            audioSource.PlayOneShot(clipToPlay);
        }
        else
        {
            Debug.Log("AudioArray on " + gameObject.name + " is empty.");
        }
        if(loopRandomly)
        {
            StartCoroutine(PlayRandomAudioAfterTime(Random.Range(.5f, 10f)));
        }
    }

}
