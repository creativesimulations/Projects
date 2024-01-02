using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EpicBall
{
    public class SplashSceneController : MonoBehaviour
    {
        [Tooltip("Set the duration to display the background before loading the main menu.")]
        [SerializeField][Range(0, 10)] private float duration = 2f;

        private void Start()
        {
            StartCoroutine(LoadMainMenu());
        }

        /// <summary>
        /// Loads the main menu after showing the background for a brief period.
        /// </summary>
        /// <returns></returns>
        private IEnumerator LoadMainMenu()
        {
            yield return new WaitForSeconds(duration);
            SceneManager.LoadScene(GlobalConstants.MAIN_MENU, LoadSceneMode.Additive);
        }


    }
}