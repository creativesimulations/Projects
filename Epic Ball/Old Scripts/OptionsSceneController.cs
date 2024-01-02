using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OptionsSceneController : MonoBehaviour
{
    PlayerPrefsController playerPrefsController;
    public Slider musicSlider;
    public Slider soundSlider;

    private void Awake()
    {
        playerPrefsController = FindObjectOfType<PlayerPrefsController>();
        SetSliders();
    }
    
    public void BackOnClick()
    {
        PlayerPrefs.Save();
        SceneManager.LoadSceneAsync(GlobalConstants.MAIN_MENU, LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync(GlobalConstants.OPTIONS_MENU);
    }

    public void ChangeMusicVolume(float volume)
    {
        playerPrefsController.SetMusicVolume(volume);
    }

    public void ChangeSoundVolume(float volume)
    {
        playerPrefsController.SetSoundVolume(volume);
    }

    private void SetSliders()
    {
        musicSlider.value = PlayerPrefs.GetFloat(GlobalConstants.MUSIC_VOLUME_KEY, .3f);
        soundSlider.value = PlayerPrefs.GetFloat(GlobalConstants.SOUND_VOLUME_KEY, .5f);
    }
    
}
