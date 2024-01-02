using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{

    [SerializeField] [Range (0, 6)] private float duration = 1f;
    public GameObject titleText;
    public Animator animator;
    [HideInInspector] public string levelName;
    private List<string> congrats = new List<string> ();
    string currentCongrats;
    PlayerPrefsController playerPrefsController;

    private void Start()
    {
        animator.updateMode = AnimatorUpdateMode.UnscaledTime;
        playerPrefsController = FindObjectOfType<PlayerPrefsController> ();
        MakeCongratsList ();
    }

    private void MakeCongratsList()
    {
        congrats.Add ("Congrats!!");
        congrats.Add ("Woohoo!");
        congrats.Add ("Woot!!");
        congrats.Add ("YES!");
        congrats.Add ("Awesome!");
        congrats.Add ("COOL!");
        congrats.Add ("Way to go!");
        congrats.Add ("Finally!");
        congrats.Add ("That didn't take too long...");
        congrats.Add ("Did you get the gold Gem?");
        congrats.Add ("What took so long?");
        congrats.Add ("Now wasn't that easy?");
        congrats.Add ("GREAT!");
        congrats.Add ("Wonderful!!");
        congrats.Add ("Fantastic!");
        congrats.Add ("INCREDIBLE!");
        congrats.Add ("Superb!");
        congrats.Add ("Terrific!!");
        congrats.Add ("Outstanding!");
        congrats.Add ("Magnificient!!");
        congrats.Add ("Phenomenal!");
        congrats.Add ("Fabulous!");
        congrats.Add ("Excellent!!");
        congrats.Add ("BRILLIANT!");
        congrats.Add ("Amazing!");
        congrats.Add ("Impressive!");
        congrats.Add ("Woot!");
    }
    
    public void FadeToLevel(string levelToLoad)
    {
        levelName = levelToLoad;
        Fade (levelName);
    }

    public void FadeToLevel(int levelToLoad)
    {
        levelName = NameFromIndex(levelToLoad);
        Fade (levelName);
    }

    public string addCongrats()
    {
            currentCongrats = congrats[Random.Range (0, congrats.Count)];
        return currentCongrats;
    }

    public void Fade(string levelTitle)
    {
        playerPrefsController.fading = true;
        FindObjectOfType<Goal> ().betweenLevel = true;
        playerPrefsController.pauseLock = true;
        FadeMusic ();
        animator.SetTrigger("FadeOut");
        if (levelTitle != GlobalConstants.SPLASH_SCREEN)
        {
            Ball ball = FindObjectOfType<Ball> ();
            if (ball != null && ball.win)
            {
                titleText.GetComponent<TMPro.TextMeshProUGUI> ().text = addCongrats () + "\n Loading Level: \n" + levelTitle + "\n Score: " + PlayerPrefs.GetInt((SceneManager.GetActiveScene().buildIndex +1) + "_SCORE");
            }
            else
            {
                titleText.GetComponent<TMPro.TextMeshProUGUI> ().text = "\n Loading Level: \n" + levelTitle;
            }
        }
        else
        {
            titleText.GetComponent<TMPro.TextMeshProUGUI> ().text = "Loading...";
        }
    }
    
    public IEnumerator OnfadeComplete()
    {
        FindObjectOfType<Goal> ().betweenLevel = false;
        playerPrefsController.UnPause ();
        AsyncOperation ao = SceneManager.LoadSceneAsync (levelName);
        yield return ao;
        playerPrefsController.pauseLock = false;
        FadeInMusic ();
        animator.SetTrigger ("FadeIn");
        playerPrefsController.fading = false;
    }

    public string NameFromIndex(int BuildIndex)
    {
        string path = SceneUtility.GetScenePathByBuildIndex(BuildIndex);
        int slash = path.LastIndexOf('/');
        string name = path.Substring(slash + 1);
        int dot = name.LastIndexOf('.');
        return name.Substring(0, dot);
    }

    private void FadeMusic()
    {
        AudioSourceExtensions.FadeOut (Camera.main.GetComponent<AudioSource> (), 2.5f);
    }

    private void FadeInMusic()
    {
        AudioSourceExtensions.FadeIn (Camera.main.GetComponent<AudioSource> (), 2.5f);
    }

    public void RandomLevel(int lastLevelPlayed)
    {
        if (!playerPrefsController.fading)
        {
            int levelReached = PlayerPrefs.GetInt(GlobalConstants.LEVELS_COMPLETE_KEY, 1);
            int randomLevelNumber = Random.Range(1, levelReached);
            if (randomLevelNumber == lastLevelPlayed)
            {
                randomLevelNumber++;
            }
            FadeToLevel(randomLevelNumber);
            playerPrefsController.randomize = true;
        }
    }
}
