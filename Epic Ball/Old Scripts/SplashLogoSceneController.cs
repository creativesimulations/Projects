using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashLogoSceneController : MonoBehaviour
{

    [SerializeField] [Range(0, 6)] private float duration = 1f;

private void Start() {
    StartCoroutine(LoadSplashScene());
}

    private IEnumerator LoadSplashScene()
    {
        yield return new WaitForSeconds(duration);
        SceneManager.LoadSceneAsync(GlobalConstants.SPLASH_SCREEN);
    }
}
