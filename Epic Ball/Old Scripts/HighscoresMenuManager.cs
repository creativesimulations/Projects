using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HighscoresMenuManager : MonoBehaviour
{
    private void Start()
    {
        //highScoreButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Your Highscore: " + FindObjectOfType<Scoring>().AddScores();
    }

    public void BackOnClick()
    {
        SceneManager.LoadSceneAsync(GlobalConstants.MAIN_MENU, LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync(GlobalConstants.HIGHSCORES_MENU);
    }

}
