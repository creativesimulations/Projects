using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuSceneController : MonoBehaviour
{

    public GameObject highScoreButton;
    public List<string> tutScenes = new List<string>();


    private void Start()
    {
        AddTutorialScenes();
    }

    public void PlayOnClick()
    {
        SceneManager.LoadSceneAsync(GlobalConstants.LEVEL_MENU, LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync(GlobalConstants.MAIN_MENU);
    }

    public void OptionsOnClick()
    {
        SceneManager.LoadSceneAsync(GlobalConstants.OPTIONS_MENU, LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync(GlobalConstants.MAIN_MENU);
    }

    public void ExitOnClick()
    {
        if (Application.isEditor == true)
        {
        PlayerPrefs.DeleteAll();
        }
        
        Application.Quit();
    }

    public void TipsClick()
    {
        SceneManager.LoadSceneAsync (GlobalConstants.TIPS_MENU, LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync(GlobalConstants.MAIN_MENU);
    }

    public void HighscoresClick()
    {
        SceneManager.LoadSceneAsync(GlobalConstants.HIGHSCORES_MENU, LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync(GlobalConstants.MAIN_MENU);
    }

    public void TutorialClick()
    {
        LevelChanger levelChanger = FindObjectOfType<LevelChanger>();
        levelChanger.FadeToLevel(tutScenes[PlayerPrefs.GetInt(GlobalConstants.TUT_LEVELS_COMPLETE_KEY)]);
    }

    private void AddTutorialScenes()
    {
        tutScenes.Add(GlobalConstants.TUTLVL_0);
        tutScenes.Add(GlobalConstants.TUTLVL_1);
        tutScenes.Add(GlobalConstants.TUTLVL_2);
        tutScenes.Add(GlobalConstants.TUTLVL_3);
        tutScenes.Add(GlobalConstants.TUTLVL_4);
        PlayerPrefs.SetInt(GlobalConstants.NUMBER_OF_TUT_LEVELS, tutScenes.Count);
        Debug.Log("tutscene.count = " + PlayerPrefs.GetInt(GlobalConstants.NUMBER_OF_TUT_LEVELS));
    }
}
