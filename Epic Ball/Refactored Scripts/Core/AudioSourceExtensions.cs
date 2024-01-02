using System.Collections;
using UnityEngine;

namespace EpicBall
{
    public static class AudioSourceExtensions
    {
        /// <summary>
        /// Stops all coroutines that might be running and begins the coroutine to fade out an audio clip.
        /// </summary>
        /// <param name="a"></param> The audio source to use.
        /// <param name="duration"></param> The length of time to fade out.
        public static void FadeOut(this AudioSource a, float duration)
        {
            a.GetComponent<MonoBehaviour>().StopAllCoroutines();
            a.GetComponent<MonoBehaviour>().StartCoroutine(FadeOutCore(a, duration));
        }

        /// <summary>
        /// Stops all coroutines that might be running and begins the coroutine to fade in an audio clip.
        /// </summary>
        /// <param name="a"></param> The audio source to use.
        /// <param name="duration"></param> The length of time to fade in.
        public static void FadeIn(this AudioSource a, float duration)
        {
            a.GetComponent<MonoBehaviour>().StopAllCoroutines();
            a.GetComponent<MonoBehaviour>().StartCoroutine(FadeInCore(a, duration));
        }

        /// <summary>
        /// Fades out an audio clip over a specified amount of time.
        /// </summary>
        /// <param name="a"></param> The audio source to use.
        /// <param name="duration"></param> The length of time to fade out.
        /// <returns></returns>
        private static IEnumerator FadeOutCore(AudioSource a, float duration)
        {
            while (a.volume > 0.001)
            {
                a.volume -= 1 * Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

        }

        /// <summary>
        /// Fades in an audio clip over a specified amount of time.
        /// </summary>
        /// <param name="a"></param> The audio source to use.
        /// <param name="duration"></param> The length of time to fade out.
        /// <returns></returns>
        private static IEnumerator FadeInCore(AudioSource a, float duration)
        {
            while (a.volume < 1)
            {
                a.volume += 1 * Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

        }

        /// <summary>
        /// Plays an audio clip and deparents the game object with the audiosource, then destroys the audiosource game object after the clip is finished playing.
        /// </summary>
        /// <param name="audioSource"></param> The audio source to use.
        /// <param name="clipToPlay"></param> The audio clip to play.
        public static void PlayAfterDestroy(AudioSource audioSource, AudioClip clipToPlay)
        {
            audioSource.PlayOneShot(clipToPlay);
            audioSource.transform.parent = null;
            Object.Destroy(audioSource.gameObject, clipToPlay.length);
        }
    }
}