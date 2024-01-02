using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AudioSourceExtensions
{
    public static void FadeOut(this AudioSource a, float duration)
    {
        a.GetComponent<MonoBehaviour> ().StopAllCoroutines ();
        a.GetComponent<MonoBehaviour>().StartCoroutine(FadeOutCore(a, duration));
    }
    
    public static void FadeIn(this AudioSource a, float duration)
    {
        a.GetComponent<MonoBehaviour> ().StopAllCoroutines ();
        a.GetComponent<MonoBehaviour> ().StartCoroutine (FadeInCore (a, duration));
    }


    private static IEnumerator FadeOutCore(AudioSource a, float duration)
    {
       // float startVolume = a.volume; //PlayerPrefs.GetFloat (GlobalConstants.MUSIC_VOLUME_KEY);
       
        while (a.volume > 0.001)
        {
            a.volume -= 1 * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        
    }

    private static IEnumerator FadeInCore(AudioSource a, float duration)
    {
       // float startVolume = a.volume; //PlayerPrefs.GetFloat (GlobalConstants.MUSIC_VOLUME_KEY);
       
        while (a.volume < 1)
        {
            a.volume += 1 * Time.deltaTime;
            yield return new WaitForEndOfFrame ();
        }
        
    }

    public static void PlayAfterDestroy(AudioSource audioSource)
{
        audioSource.PlayOneShot(audioSource.clip);
        audioSource.transform.parent = null;
        Object.Destroy(audioSource.gameObject, audioSource.clip.length);
}




}