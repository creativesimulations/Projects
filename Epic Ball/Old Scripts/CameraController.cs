using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour
{

    public GameObject cameraTarget;
    private Vector3 offset;
    private float speed = .1f;
    private bool activeGame;
    public AudioSource audioSource;
    public Material[] skyboxes;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource> ();
        offset = new Vector3 (0, 15, 0);
    }
    
    void Update()
    {
        if (cameraTarget == null)
        {
            activeGame = false;
        }
        if (activeGame == true)
        {
            transform.position = cameraTarget.transform.position + offset;
        }
    }

    public void SetUpCamera(GameObject player, bool isactive)
    {
        cameraTarget = player;
        activeGame = isactive;
        SetMusicAndSkybox ();
        if (player != null)
        {
            transform.rotation = Quaternion.Euler (80, 0, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler (22, 58, 0);
            transform.position = new Vector3 (12, -12, 6);
        }
    }

    private void SetMusicAndSkybox()
    {
        string levelName = SceneManager.GetActiveScene ().name;

        switch (levelName)
        {
            case GlobalConstants.SPLASH_SCREEN:
            audioSource.clip = Resources.Load<AudioClip> (GlobalConstants.MUSIC_1);
            RenderSettings.skybox = skyboxes[2];
            break;
            case GlobalConstants.LEVEL_1:
            audioSource.clip = Resources.Load<AudioClip> (GlobalConstants.MUSIC_7);
            RenderSettings.skybox = skyboxes[7];
            break;
            case GlobalConstants.LEVEL_2:  //  Simple
                audioSource.clip = Resources.Load<AudioClip>(GlobalConstants.MUSIC_3);
                RenderSettings.skybox = skyboxes[5];
                break;
            case GlobalConstants.LEVEL_3:  //  Stairs
                audioSource.clip = Resources.Load<AudioClip>(GlobalConstants.MUSIC_19);
                RenderSettings.skybox = skyboxes[4];//?
                break;
            case GlobalConstants.LEVEL_4:  //  Scitter Scatter
                audioSource.clip = Resources.Load<AudioClip>(GlobalConstants.MUSIC_7);
                RenderSettings.skybox = skyboxes[7];
                break;
            case GlobalConstants.LEVEL_5:  //  Shapes
                audioSource.clip = Resources.Load<AudioClip> (GlobalConstants.MUSIC_23);
            RenderSettings.skybox = skyboxes[5];
            break;
            case GlobalConstants.LEVEL_6:  //  Cave In
                audioSource.clip = Resources.Load<AudioClip>(GlobalConstants.MUSIC_17);
                RenderSettings.skybox = skyboxes[5];//?
                break;
            case GlobalConstants.LEVEL_7:  //  Rollers From Hell
                audioSource.clip = Resources.Load<AudioClip>(GlobalConstants.MUSIC_20);
                RenderSettings.skybox = skyboxes[1];
                break;
            case GlobalConstants.LEVEL_8:  //  Temple
                audioSource.clip = Resources.Load<AudioClip>(GlobalConstants.MUSIC_29);
                RenderSettings.skybox = skyboxes[7];//?
                break;
            case GlobalConstants.LEVEL_9:  //  Consume
                audioSource.clip = Resources.Load<AudioClip> (GlobalConstants.MUSIC_19);
            RenderSettings.skybox = skyboxes[1];
            break;
            case GlobalConstants.LEVEL_10:  //  Back to the Beginning
                audioSource.clip = Resources.Load<AudioClip>(GlobalConstants.MUSIC_24);
                RenderSettings.skybox = skyboxes[6];//?
                break;
            case GlobalConstants.LEVEL_11:  //  Tele Collect
                audioSource.clip = Resources.Load<AudioClip>(GlobalConstants.MUSIC_15);
                RenderSettings.skybox = skyboxes[0];//?
                break;
            case GlobalConstants.LEVEL_12:  //  Shrapnel
                audioSource.clip = Resources.Load<AudioClip> (GlobalConstants.MUSIC_26);
            RenderSettings.skybox = skyboxes[3];
            break;
            case GlobalConstants.LEVEL_13:  //  Divided
                audioSource.clip = Resources.Load<AudioClip>(GlobalConstants.MUSIC_5);
                RenderSettings.skybox = skyboxes[2];//?
                break;
            case GlobalConstants.LEVEL_14:  //  Explode
                audioSource.clip = Resources.Load<AudioClip> (GlobalConstants.MUSIC_24);
            RenderSettings.skybox = skyboxes[0];
            break;
            case GlobalConstants.LEVEL_15:  //  Constructs
                audioSource.clip = Resources.Load<AudioClip> (GlobalConstants.MUSIC_8);
            RenderSettings.skybox = skyboxes[1];
            break;
            case GlobalConstants.LEVEL_16:  //  Down We Go
                audioSource.clip = Resources.Load<AudioClip> (GlobalConstants.MUSIC_3);
            RenderSettings.skybox = skyboxes[6];
            break;
            case GlobalConstants.LEVEL_17:  //  Slider
            audioSource.clip = Resources.Load<AudioClip> (GlobalConstants.MUSIC_18);
            RenderSettings.skybox = skyboxes[7];
            break;
            case GlobalConstants.LEVEL_18:  //  Lever
                audioSource.clip = Resources.Load<AudioClip> (GlobalConstants.MUSIC_19);
            RenderSettings.skybox = skyboxes[3];
            break;
            case GlobalConstants.LEVEL_19:  //  Dodger Field
                audioSource.clip = Resources.Load<AudioClip>(GlobalConstants.MUSIC_22);
                RenderSettings.skybox = skyboxes[2];
                break;
            case GlobalConstants.LEVEL_20:  //  Coniption
                audioSource.clip = Resources.Load<AudioClip>(GlobalConstants.MUSIC_22);
                RenderSettings.skybox = skyboxes[2];//?
                break;
            case GlobalConstants.LEVEL_21:  //  It's Raining
                audioSource.clip = Resources.Load<AudioClip>(GlobalConstants.MUSIC_14);
                RenderSettings.skybox = skyboxes[4];//?
                break;
            case GlobalConstants.LEVEL_22:  //  Bridges Falling Down
                audioSource.clip = Resources.Load<AudioClip>(GlobalConstants.MUSIC_11);
                RenderSettings.skybox = skyboxes[7];
                break;
            case GlobalConstants.LEVEL_23:  //  Tele Maze
                audioSource.clip = Resources.Load<AudioClip> (GlobalConstants.MUSIC_23);
            RenderSettings.skybox = skyboxes[0];
            break;
            case GlobalConstants.LEVEL_24:  //  Balance Beams
                audioSource.clip = Resources.Load<AudioClip>(GlobalConstants.MUSIC_29);
                RenderSettings.skybox = skyboxes[5];
                break;
            case GlobalConstants.LEVEL_25:  //  Consumer City
                audioSource.clip = Resources.Load<AudioClip>(GlobalConstants.MUSIC_17);
                RenderSettings.skybox = skyboxes[6];
                break;
            case GlobalConstants.LEVEL_26:  //  Demolition Gauntlet
                audioSource.clip = Resources.Load<AudioClip> (GlobalConstants.MUSIC_12);
            RenderSettings.skybox = skyboxes[4];
            break;
            case GlobalConstants.LEVEL_27:  //  Suspended
                audioSource.clip = Resources.Load<AudioClip> (GlobalConstants.MUSIC_15);
            RenderSettings.skybox = skyboxes[0];
            break;
            case GlobalConstants.LEVEL_28:  //  Warehouse
                audioSource.clip = Resources.Load<AudioClip>(GlobalConstants.MUSIC_17);
                RenderSettings.skybox = skyboxes[6];
                break;
            case GlobalConstants.LEVEL_29:  //  Jumpy
                audioSource.clip = Resources.Load<AudioClip> (GlobalConstants.MUSIC_5);
            RenderSettings.skybox = skyboxes[7];
            break;
            case GlobalConstants.LEVEL_30:  //  Omg
                audioSource.clip = Resources.Load<AudioClip>(GlobalConstants.MUSIC_4);
                RenderSettings.skybox = skyboxes[4];//?
                break;
            case GlobalConstants.LEVEL_31:  //  Holy Cow
                audioSource.clip = Resources.Load<AudioClip> (GlobalConstants.MUSIC_21);
            RenderSettings.skybox = skyboxes[0];
            break;
            case GlobalConstants.LEVEL_32:  //  Mines
                audioSource.clip = Resources.Load<AudioClip> (GlobalConstants.MUSIC_17);
            RenderSettings.skybox = skyboxes[5];
            break;
            case GlobalConstants.LEVEL_33:  //  Traps
                audioSource.clip = Resources.Load<AudioClip> (GlobalConstants.MUSIC_18);
            RenderSettings.skybox = skyboxes[1];
            break;
            case GlobalConstants.LEVEL_34:  //  =)
                audioSource.clip = Resources.Load<AudioClip>(GlobalConstants.MUSIC_18);
                RenderSettings.skybox = skyboxes[1];//?
                break;
            case GlobalConstants.LEVEL_35:  //  Race
                audioSource.clip = Resources.Load<AudioClip> (GlobalConstants.MUSIC_10);
            RenderSettings.skybox = skyboxes[6];
            break;
            case GlobalConstants.LEVEL_36:  //  Push Blocks
                audioSource.clip = Resources.Load<AudioClip> (GlobalConstants.MUSIC_27);
            RenderSettings.skybox = skyboxes[5];
            break;
            case GlobalConstants.LEVEL_37:  //  Towers
                audioSource.clip = Resources.Load<AudioClip> (GlobalConstants.MUSIC_31);
            RenderSettings.skybox = skyboxes[4];
            break;
            case GlobalConstants.LEVEL_38:  //  Labyrinth
                audioSource.clip = Resources.Load<AudioClip>(GlobalConstants.MUSIC_20);
            RenderSettings.skybox = skyboxes[6];
            break;
            case GlobalConstants.LEVEL_39:  //  The Chase
                audioSource.clip = Resources.Load<AudioClip>(GlobalConstants.MUSIC_10);
                RenderSettings.skybox = skyboxes[1];
                break;
            case GlobalConstants.LEVEL_40:  //  Ballerina
                audioSource.clip = Resources.Load<AudioClip>(GlobalConstants.MUSIC_21);
                RenderSettings.skybox = skyboxes[4];
                break;
            case GlobalConstants.LEVEL_41:  //  Ball Pool
                audioSource.clip = Resources.Load<AudioClip> (GlobalConstants.MUSIC_14);
            RenderSettings.skybox = skyboxes[2];
            break;
            case GlobalConstants.LEVEL_42:  //  Tippy
                audioSource.clip = Resources.Load<AudioClip>(GlobalConstants.MUSIC_15);
                RenderSettings.skybox = skyboxes[1];
                break;
            default:
            break;
        }
        DynamicGI.UpdateEnvironment ();
        audioSource.Play ();
    }



    public void ZoomCamera()
    {
        Ball playerBall = cameraTarget.transform.parent.GetComponent<Ball> ();
        activeGame = false;
        if (playerBall.win)
        {
            StartCoroutine (ZoomInWin ());
        }
        else
        {
            StartCoroutine (ZoomInKilled (playerBall));
        }
    }

    IEnumerator ZoomInKilled(Ball playerBall)
    {
        Time.timeScale = 0;
        float cameraDistance = Vector3.Distance (transform.position, cameraTarget.transform.position);

        while (cameraDistance > 8f)
        {
            cameraDistance = Vector3.Distance (transform.position, cameraTarget.transform.position);
            transform.localPosition = Vector3.MoveTowards (transform.position, cameraTarget.transform.position, speed);

            yield return new WaitForEndOfFrame ();
        }

        Time.timeScale = 1;
        var sound = GetComponent<AudioSource> ();
        AudioSourceExtensions.FadeOut (sound, 4f);
        cameraTarget.transform.parent = null;
        StartCoroutine (FindObjectOfType<Goal> ().HardRestartGame (1f));
        playerBall.KillPlayer ();
    }

    IEnumerator ZoomInWin()
    {
        float cameraDistance = Vector3.Distance (transform.position, cameraTarget.transform.position);
        while (cameraDistance < 24f)
        {
            cameraDistance = Vector3.Distance (transform.position, cameraTarget.transform.position);
            transform.localPosition = Vector3.MoveTowards (transform.position, cameraTarget.transform.position, - speed / 3);
            transform.RotateAround (cameraTarget.transform.position, new Vector3 (1, 0, 0), -10 * Time.deltaTime);

            yield return new WaitForEndOfFrame ();
        }

        var sound = GetComponent<AudioSource> ();
        AudioSourceExtensions.FadeOut (sound, 4f);
    }



}