using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseSceneManager : MonoBehaviour
{

    private PlayerPrefsController playerPrefsController;
    private LevelChanger levelChanger;
    [SerializeField] GameObject timeDisplay;
    Timer timer;

    private void Start() {
        playerPrefsController = FindObjectOfType<PlayerPrefsController>();
        levelChanger = FindObjectOfType<LevelChanger>();
        timer = FindObjectOfType<Timer> ();
        ShowTime ();
    }

    private void ShowTime()
    {
        timeDisplay.GetComponent<TMPro.TextMeshProUGUI> ().text = "Time: " + timer.StringTime((int) timer.timeStart);
    }

    public void ResumeOnClick()
    {
        playerPrefsController.UnPause();
    }

    public void OptionsOnClick()
    {
        SceneManager.LoadSceneAsync(GlobalConstants.OPTIONS_MENU, LoadSceneMode.Additive);
    }

    public void RestartOnClick()
    {
        if (!playerPrefsController.fading)
        {
            levelChanger.FadeToLevel(SceneManager.GetActiveScene().name);
        }
    }

    public void MainMenuOnClick()
    {
        if (!playerPrefsController.fading)
        {
            levelChanger.FadeToLevel(GlobalConstants.SPLASH_SCREEN);
        }
    }

    public void ChooseLevelOnClick()
    {
        if (!playerPrefsController.fading)
        {
            SceneManager.LoadSceneAsync(GlobalConstants.LEVEL_MENU, LoadSceneMode.Additive);
        }
    }

    public void PlayOnClick()
    {
        if (!playerPrefsController.fading)
        {
            SceneManager.LoadSceneAsync(GlobalConstants.LEVEL_MENU, LoadSceneMode.Additive);
        }
    }
}
