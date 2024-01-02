using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SplashSceneController : MonoBehaviour
{
    private PlayerPrefsController playerPrefsController;
    private CameraController cameraController;
    [SerializeField] [Range(0, 6)] private float duration = 2f;

    private void Start()
    {
        Application.targetFrameRate = 60;
        cameraController = Camera.main.GetComponent<CameraController>();
        playerPrefsController = FindObjectOfType<PlayerPrefsController>();
        playerPrefsController.OnGameStart();
        cameraController.SetUpCamera(null, true);
        StartCoroutine(LoadMainMenu());
    }

    private IEnumerator LoadMainMenu()
    {
        yield return new WaitForSeconds(duration);
        SceneManager.LoadScene(GlobalConstants.MAIN_MENU, LoadSceneMode.Additive);
    }

}
