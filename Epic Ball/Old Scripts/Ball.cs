using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class Ball : MonoBehaviour
{
    [SerializeField] private float jumpPower = 400f; // The force added to the ball when it jumps.
    [SerializeField] private float desiredSpeed = 10f;
    [SerializeField] private float forceConstant = 5f;
    [SerializeField] private ParticleSystem deathParticle;
    [SerializeField] private float xVelocityDrag = .9f;
    [SerializeField] private float zVelocityDrag = .9f;

    GemCounter gemCounter;
    public bool isAlive;
    public bool win = false;
    public Rigidbody rigidbody;
    private int gemCount = 0;
    public int goldGemCount = 0;
    private CameraController cameraController;
    public Goal goal;
    public PlaneDetector[] planes;
    private SphereCollider camTarSphereCol;
    ParticleHolder particleHolder;
    AudioSource audioSource;
    BumpSounds bumpSounds;
    Vector3 startPos;
    PlayerPrefsController playerPrefsController;
    Timer timer;
    Scoring scoring;
    public bool goldGemCollected;


    private void Awake()
    {
        bumpSounds = GetComponent<BumpSounds>();
        audioSource = GetComponentInChildren<AudioSource>();
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        camTarSphereCol = GetComponentInChildren<SphereCollider>();
        planes = FindObjectsOfType<PlaneDetector>();
        playerPrefsController = FindObjectOfType<PlayerPrefsController>();
        playerPrefsController.EnableControls();
        goal = FindObjectOfType<Goal>();
        timer = FindObjectOfType<Timer>();
        particleHolder = FindObjectOfType<ParticleHolder>();
        startPos = transform.position;
        cameraController = Camera.main.GetComponent<CameraController>();
        cameraController.SetUpCamera(transform.GetChild(0).gameObject, true);
        scoring = FindObjectOfType<Scoring>();
        gemCounter = FindObjectOfType<GemCounter>();
        particleHolder.CreateParticles();
        isAlive = true;
        timer.StartTimer();
        scoring.ResetScores();
    }

    public void Move(Vector3 moveDirection)
    {
        float forceMultiplier = Mathf.Clamp01((desiredSpeed - rigidbody.velocity.magnitude) / desiredSpeed);
        rigidbody.AddForce(moveDirection * (forceMultiplier * Time.deltaTime * forceConstant), ForceMode.Impulse);
    }

    public void SlowBallx()
    {
        Vector3 vel = rigidbody.velocity;
        vel.x *= xVelocityDrag;
        rigidbody.velocity = vel;
    }

    public void SlowBallz()
    {
        Vector3 vel = rigidbody.velocity;
        vel.z *= zVelocityDrag;
        rigidbody.velocity = vel;
    }

    public void BallJump()
    {
        rigidbody.velocity = rigidbody.velocity + Vector3.up * jumpPower;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy") && !win)
        {
            transform.position = startPos;
            audioSource.PlayOneShot(audioSource.clip);
            particleHolder.playParticle(2, transform.position, transform.lossyScale);
            DetectPlanes();
        }

        else if (other.gameObject.CompareTag("Destroyemy") && !win)
        {
            isAlive = false;
            StartDeath();
        }
    }


    public void DetectPlanes()
    {
        foreach (PlaneDetector planeDetector in planes)
        {
            planeDetector.CheckHeight(gameObject);
            planeDetector.SetRender(planeDetector.IsAbove);
        }
    }

    public void CollectGem()
    {
        if (!goal.isTutorialLevel)
        {
            scoring.AddPrize(GlobalConstants.PURPLE_GEM_PRIZE);
        }
        gemCount = gemCount + 1;
        gemCounter.ChangeCounter(gemCount);
        goal.SetWinParticle(gemCount);
    }

    public void CollectGoldGem()
    {
        string levelNameKey = SceneManager.GetActiveScene().name + "GoldKey";
        scoring.AddPrize(GlobalConstants.GOLD_GEM_PRIZE);
        if (!PlayerPrefs.HasKey(levelNameKey))
        {
            goldGemCount = goldGemCount + 1;
            PlayerPrefs.SetInt(levelNameKey, 1);
        }
    }

    public void StartDeath()
    {
        playerPrefsController.pauseLock = true;
        cameraController.ZoomCamera();
    }

    public void KillPlayer()
    {
        AudioSourceExtensions.PlayAfterDestroy(bumpSounds.childAudioSource);
        ParticleSystem newDeathParticle = Instantiate(deathParticle, transform.position, Quaternion.identity) as ParticleSystem;
        gameObject.SetActive(false);
    }

    public void WinGame()
    {
        if (!win)
        {
            GetComponent<BallUserControl>().allowJump = false;
            playerPrefsController.pauseLock = true;
            rigidbody.velocity = Vector3.zero;
            if (!goal.isTutorialLevel)
            {
                if (goldGemCollected)
                {
                    CollectGoldGem();
                }
                playerPrefsController.LevelComplete(SceneManager.GetActiveScene().buildIndex);
                timer.SaveTime(SceneManager.GetActiveScene().buildIndex);
                scoring.HandleScoring();
            }
            win = true;
            cameraController.ZoomCamera();
        }
    }

    public float GemCount
    {
        get { return gemCount; }
    }

}

