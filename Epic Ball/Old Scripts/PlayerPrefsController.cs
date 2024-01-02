using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class PlayerPrefsController : MonoBehaviour
{

    public AudioMixer audioMixer;
    public GameObject controls;
    public bool isPaused;
    public bool pauseLock;
    public bool randomize;
    public bool fading;


    public void OnGameStart()
    {
        DisableControls ();
        audioMixer.SetFloat(GlobalConstants.MUSIC_VOLUME, Mathf.Log10(PlayerPrefs.GetFloat(GlobalConstants.MUSIC_VOLUME_KEY, .3f)) * 20);
        audioMixer.SetFloat(GlobalConstants.SOUND_VOLUME, Mathf.Log10(PlayerPrefs.GetFloat(GlobalConstants.SOUND_VOLUME_KEY, .5f)) * 20);
        LevelComplete(PlayerPrefs.GetInt(GlobalConstants.LEVELS_COMPLETE_KEY, 1));
        PlayerPrefs.SetInt(GlobalConstants.HIGH_SCORE ,0);
        LevelComplete (50);
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat(GlobalConstants.MUSIC_VOLUME, Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat(GlobalConstants.MUSIC_VOLUME_KEY, volume);
    }

    public void SetSoundVolume(float volume)
    {
        audioMixer.SetFloat(GlobalConstants.SOUND_VOLUME, Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat(GlobalConstants.SOUND_VOLUME_KEY, volume);
    }

    public void LevelComplete(int levelCompleted)
    {
        if (PlayerPrefs.GetInt(GlobalConstants.LEVELS_COMPLETE_KEY) < levelCompleted)
        {
            PlayerPrefs.SetInt(GlobalConstants.LEVELS_COMPLETE_KEY, levelCompleted);
        }
    }
    

    public void DisableControls()
    {
            controls.SetActive(false);
    }

    public void EnableControls()
    {
        //if (!Application.isEditor)
        //{
        controls.SetActive(true);
        //}
    }

    public void PauseToggle()
    {

        if (!pauseLock)
        {
            Pause ();
        }
        else
        {
            UnPause();
        }
            }

    public void Pause()
    {
        if (!pauseLock)
            {
            pauseLock = true;
            Time.timeScale = 0f;
            isPaused = true;
            DisableControls();
            SceneManager.LoadSceneAsync(GlobalConstants.PAUSE_MENU, LoadSceneMode.Additive);
            }
    }

    public void UnPause()
    {
        if (isPaused)
        {
            pauseLock = false;
            Time.timeScale = 1f;
            isPaused = false;
            EnableControls ();
            SceneManager.UnloadSceneAsync (GlobalConstants.PAUSE_MENU);
        }
    }

}
