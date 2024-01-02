using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class LevelSceneController : MonoBehaviour
{

    public int levelReached;
    public Button[] levelButtons;
    [HideInInspector] string levelName;
    private LevelChanger levelChanger;
    public Sprite bronzeStar;
    public Sprite silverStar;
    public Sprite goldStar;
    Timer timer;


    // Start is called before the first frame update
    void Start()
    {
        levelChanger = FindObjectOfType<LevelChanger>();
        levelReached = PlayerPrefs.GetInt(GlobalConstants.LEVELS_COMPLETE_KEY, 0);
        int accessLevel = levelReached + FindObjectOfType<Ball>().goldGemCount;
        timer = FindObjectOfType<Timer> ();
        PlayerPrefs.SetInt(GlobalConstants.NUMBER_OF_LEVELS, levelButtons.Length);

        for (int i = 0; i < levelButtons.Length; i++)
        {
            if (i > accessLevel)
            {
                levelButtons[i].interactable = false;
            }
            else
            {
                SetLevelTime (levelButtons[i], i+1);
                int numberToGet = i + 1;
                int bestTime = timer.GetTime (numberToGet);
                if (bestTime != 0)
                {
                    GameObject background = levelButtons[i].transform.GetChild(1).gameObject;
                    GameObject star = background.transform.GetChild(0).gameObject;
                    if (bestTime <= timer.goldTimes[numberToGet])
                    {
                        star.GetComponent<Image> ().sprite = goldStar;
                    }
                    else if (bestTime <= timer.silverTimes[numberToGet])
                    {
                        star.GetComponent<Image> ().sprite = silverStar;
                    }
                    else
                    {
                        star.GetComponent<Image> ().sprite = bronzeStar;
                    }
                }
            }
            if (!PlayerPrefs.HasKey(levelButtons[i].name + "GoldKey"))
            {
                levelButtons[i].transform.Find("Background").GetComponentInParent<RawImage> ().enabled = false;
            }
        }
    }

    private void SetLevelTime(Button currentButton, int buttonNumber)
    {
     //   GameObject parentGO = currentButton.transform.parent.parent.gameObject;
     
        GameObject gO = currentButton.transform.Find (currentButton.name + " Time").gameObject; //parentGO.
        gO.GetComponent<TMPro.TextMeshProUGUI> ().text = timer.StringTime (PlayerPrefs.GetInt ("LEVEL_" + buttonNumber + "_TIME_KEY", 0));
    }

    public void StartLevelOnClick()
    {
        PlayerPrefsController playerPrefsController = FindObjectOfType<PlayerPrefsController> ();
        if (!FindObjectOfType<Goal> ().betweenLevel)
        {
        //playerPrefsController.UnPause();
        levelChanger.FadeToLevel(EventSystem.current.currentSelectedGameObject.name);
        playerPrefsController.randomize = false;
        }
    }

    public void BackOnClick()
    {
        SceneManager.LoadSceneAsync(GlobalConstants.MAIN_MENU, LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync (GlobalConstants.LEVEL_MENU);
    }

    public void PlayRandomLevel()
    {
        levelChanger.RandomLevel (0);
    }

}
