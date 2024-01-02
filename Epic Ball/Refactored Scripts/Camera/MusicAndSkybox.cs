using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;

namespace EpicBall
{
    [RequireComponent(typeof(AudioSource))]

    public class MusicAndSkybox : MonoBehaviour
    {
        [Tooltip("The audio mixer to be used.")]
        [SerializeField] private AudioMixer _audioMixer;
        [Tooltip("The speed at which to fade the music during fade animations.")]
        [SerializeField] private float _fadeMusicSpeed = 2.5f;
        [Tooltip("The skyboxes to be used in the levels.")]
        [SerializeField] private Material[] _skyboxes;
        [Tooltip("The music to be used in the levels.")]
        [SerializeField] private AudioClip[] _audioClips;

        private AudioSource _audioSource;

        void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            GameManager.ChangeLvl += FadeMusic;
            GameManager.AudioChanges += SetAudioVolume;
        }

        private void Start()
        {
            LevelSetUp();
            SetAudioVolume();
        }

        /// <summary>
        /// Sets the music and sound effect volumes to the saved values in PlayerPrefs, or to the default levels or no values are saved yet.
        /// </summary>
        private void SetAudioVolume()
        {
            if (_audioMixer != null)
            {
                _audioMixer.SetFloat(GlobalConstants.MUSIC_VOLUME, Mathf.Log10(PlayerPrefsController.GetMusicVolume()) * 20);
                _audioMixer.SetFloat(GlobalConstants.SOUND_VOLUME, Mathf.Log10(PlayerPrefsController.GetSoundVolume()) * 20);
            }
            else
            {
                ExceptionManager.instance.SendMissingObjectMessage("_audioMixer", GetType().ToString(), name);
            }
        }

        /// <summary>
        /// Sets the music and sound effect volumes to the values specified.
        /// </summary>
        /// <param name="musicVolume"></param> The music volume value.
        /// <param name="soundVolume"></param> The sound volume value.
        private void SetAudioVolume(float musicVolume, float soundVolume)
        {
            _audioMixer.SetFloat(GlobalConstants.MUSIC_VOLUME, Mathf.Log10(musicVolume) * 20);
            _audioMixer.SetFloat(GlobalConstants.SOUND_VOLUME, Mathf.Log10(soundVolume) * 20);
            PlayerPrefsController.SetAudioVolume(musicVolume, soundVolume);

        }

        /// <summary>
        /// Sets the skybox and music to those specified in the current scriptable level settings.
        /// </summary>
        private void LevelSetUp()
        {
            LevelSettingsScriptable levelSettings = LevelManager.CurrentLvlSettings;

            _audioSource.clip = _audioClips[levelSettings._levelNum];
            _audioSource.Play();
            RenderSettings.skybox = _skyboxes[levelSettings._levelNum];
            DynamicGI.UpdateEnvironment();

            FadeInMusic();
        }

        /// <summary>
        /// Fades the music out.
        /// </summary>
        private void FadeMusic()
        {
            AudioSourceExtensions.FadeOut(_audioSource, _fadeMusicSpeed);
        }

        /// <summary>
        /// Fades the music in.
        /// </summary>
        private void FadeInMusic()
        {
            AudioSourceExtensions.FadeIn(_audioSource, _fadeMusicSpeed);
        }

        private void OnDisable()
        {
            GameManager.ChangeLvl -= FadeMusic;
            GameManager.AudioChanges -= SetAudioVolume;
        }
    }
}