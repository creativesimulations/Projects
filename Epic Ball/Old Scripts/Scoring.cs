using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Scoring : MonoBehaviour
{

    private int goldGemPrize = 500;
    private int purpleGemPrize = 5;
    private int goldMedalPrize = 1000;
    private int silverMedalPrize = 200;
    private int bronzeMedalPrize = 50;
    private int currentLvl;
    public int currentScore = 0;
    public int currentTime = 0;
    public string currentMedal;
    public string previousMedal;
    public int previousTime = 0;
    public int previousScore = 0;
    private int highScore;
    Timer timer;
    LevelChanger levelChanger;

    // Start is called before the first frame update
    void Start()
    {
        timer = FindObjectOfType<Timer>();
        levelChanger = FindObjectOfType<LevelChanger>();
    }

    public void ResetScores()
    {
        currentLvl = SceneManager.GetActiveScene().buildIndex;
        currentScore = 0;
        highScore = 0;
        previousScore = 0;
        previousTime = 0;
        GetOldScores();
    }

    private void GetOldScores()
    {
        previousTime = timer.GetTime(currentLvl);
        previousScore = PlayerPrefs.GetInt(currentLvl + "_SCORE");
        if (PlayerPrefs.HasKey(SceneManager.GetActiveScene().name + "_MEDAL"))
        {
        previousMedal = PlayerPrefs.GetString(SceneManager.GetActiveScene().name + "_MEDAL");
        }
    }

    public void HandleScoring()
    {
        CalculateMedal();
        SetScore();
        //AddScores();
        ShowScore();
    }

    private void SetScore()
    {
        if (previousScore < currentScore)
        {
            PlayerPrefs.SetInt(currentLvl + "_SCORE", currentScore);
        }
    }

    private void CalculateMedal()
    {
        currentTime = timer.GetTime(currentLvl);
        if (currentTime <= timer.goldTimes[currentLvl])
            {
                AddPrize(GlobalConstants.GOLD_MEDAL);
                currentMedal = GlobalConstants.GOLD_MEDAL;
        }
            else if (currentTime > timer.goldTimes[currentLvl] && currentTime < timer.silverTimes[currentLvl])
            {
                AddPrize(GlobalConstants.SILVER_MEDAL);
                currentMedal = GlobalConstants.SILVER_MEDAL;
        }
            else
            {
                AddPrize(GlobalConstants.BRONZE_MEDAL);
                currentMedal = GlobalConstants.BRONZE_MEDAL;
            }
    }

    private void ShowScore()
    {
        // new high score!
    }

    public void AddPrize(string prize)
    {
            switch (prize)
            {
                case GlobalConstants.GOLD_GEM_PRIZE:
                currentScore += goldGemPrize;
                break;
                case GlobalConstants.PURPLE_GEM_PRIZE:
                currentScore += purpleGemPrize;
                break;
                case GlobalConstants.GOLD_MEDAL:
                    PlayerPrefs.SetString(SceneManager.GetActiveScene().name + "_MEDAL", GlobalConstants.GOLD_MEDAL);
                    currentScore += goldMedalPrize;
                break;
                case GlobalConstants.SILVER_MEDAL:
                if (previousMedal == GlobalConstants.BRONZE_MEDAL)
                    PlayerPrefs.SetString(SceneManager.GetActiveScene().name + "_MEDAL", GlobalConstants.SILVER_MEDAL);
                currentScore += silverMedalPrize;
                break;
                case GlobalConstants.BRONZE_MEDAL:
                    PlayerPrefs.SetString(SceneManager.GetActiveScene().name + "_MEDAL", GlobalConstants.BRONZE_MEDAL);
                currentScore += bronzeMedalPrize;
                break;
                default:
                    break;
            }
    }

    public int AddScores()
    {
        for (int i = 1; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            if (PlayerPrefs.HasKey(i + "_SCORE"))
            {
                highScore += PlayerPrefs.GetInt(i + "_SCORE");
                PlayerPrefs.SetInt(GlobalConstants.HIGH_SCORE, highScore);
            }
        }
        return PlayerPrefs.GetInt(GlobalConstants.HIGH_SCORE);
    }
}
