using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{

    [SerializeField] private ParticleSystem fireworks;
    [SerializeField] private ParticleSystem winParticle;
    [SerializeField] private ParticleSystem currentParticle;

    GemCounter gemCounter;
    private AudioSource audioSource;
    public List<GameObject> gemsInGame = new List<GameObject>();
    Rigidbody rb;
    private LevelChanger levelChanger;
    public bool betweenLevel = false;
    Ball ball;
    public bool isTutorialLevel = false;

    private void Awake() {
        audioSource = GetComponentInChildren<AudioSource>();
    }

private void Start()
    {
        currentParticle = Instantiate(currentParticle, transform.position, Quaternion.identity) as ParticleSystem;
        gemsInGame.AddRange(GameObject.FindGameObjectsWithTag("Gem"));
        levelChanger = FindObjectOfType<LevelChanger>();
        ball = FindObjectOfType<Ball> ();
        Invoke("SetCounter", 0.1f);
    }

    private void OnTriggerEnter(Collider other) {

        if(other.gameObject.CompareTag("Player"))
        {
            GameObject player = other.gameObject;
            if (ball.GemCount >= gemsInGame.Count && !ball.win)
            {
                ball.WinGame();
                fireworks = Instantiate(fireworks, transform.position, Quaternion.identity) as ParticleSystem;
                fireworks.transform.Rotate(new Vector3(-90, 0, 0));
                audioSource.PlayOneShot(audioSource.clip);
                var nextScene = SceneManager.GetActiveScene().buildIndex;
                if (!isTutorialLevel)
                {
                    if (PlayerPrefs.GetInt(GlobalConstants.NUMBER_OF_LEVELS) >= nextScene)
                    {
                        StartCoroutine (StartNextLevel(8f, nextScene));
                    }
                    else
                    {
                        StartCoroutine(StartOver(8f));
                    }
                }
                else
                {
                    if (PlayerPrefs.GetInt(GlobalConstants.NUMBER_OF_TUT_LEVELS) >= PlayerPrefs.GetInt(GlobalConstants.TUT_LEVELS_COMPLETE_KEY))
                    {
                        int reachedTutLvl = PlayerPrefs.GetInt(GlobalConstants.TUT_LEVELS_COMPLETE_KEY);
                        PlayerPrefs.SetInt(GlobalConstants.TUT_LEVELS_COMPLETE_KEY, reachedTutLvl + 1);
                        StartCoroutine(StartNextLevel(8f, nextScene));
                    }
                    else
                    {
                        PlayerPrefs.SetInt(GlobalConstants.TUT_LEVELS_COMPLETE_KEY, 0);
                        StartCoroutine(StartOver(8f));
                    }
                }
                
            }
        }
    }

    public void SetCounter()
    {
        if (SceneManager.GetActiveScene().name != GlobalConstants.SPLASH_SCREEN)
        {
        gemCounter = FindObjectOfType<GemCounter>();
            gemCounter.SetGemCounter(gemsInGame.Count);

        }
    }

    public void SetWinParticle(float gemsCollected)
    {
        if (gemsCollected >= gemsInGame.Count)
        {
            currentParticle.Stop();
            winParticle = Instantiate(winParticle, transform.position, Quaternion.identity) as ParticleSystem;
        }
    }

    
    public IEnumerator HardRestartGame(float timeToWait)
    {
        if (!betweenLevel)
        {
        betweenLevel = true;
            yield return new WaitForSecondsRealtime (timeToWait);
            levelChanger.FadeToLevel (SceneManager.GetActiveScene ().name);
            betweenLevel = false;
        }
        yield return new WaitForEndOfFrame();
    }

    public IEnumerator StartNextLevel(float timeToWait, int nextScene)
    {
        if (!betweenLevel)
        {
            betweenLevel = true;

            PlayerPrefsController playerPrefsController = FindObjectOfType<PlayerPrefsController> ();
            yield return new WaitForSecondsRealtime(timeToWait);
        if (!playerPrefsController.randomize)
        {
            levelChanger.FadeToLevel (nextScene);
        }
        else
        {
            levelChanger.RandomLevel (nextScene -1);
        }
            betweenLevel = false;
        }
        yield return new WaitForEndOfFrame ();
        ball.win = false;
    }

    public IEnumerator StartOver(float timeToWait)
    {
        if (!betweenLevel)
        {
            betweenLevel = true;
            yield return new WaitForSecondsRealtime(timeToWait);
        levelChanger.FadeToLevel(GlobalConstants.SPLASH_SCREEN);
        betweenLevel = false;
    }
    yield return new WaitForEndOfFrame();
}

}
