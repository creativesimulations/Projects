using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using TMPro;
using System.IO;
using static EpicBall.GameManager;
using static EpicBall.LevelSettingsScriptable;

namespace EpicBall
{

    [RequireComponent(typeof(Animator))]

    public class LevelManager : MonoBehaviour
    {
        [Header("Settings for changing levels.")]
        [Tooltip("The delay between the splash screen and the game background scene.")]
        [SerializeField][Range(0, 6)] private float _preSplashDuration = 1f;
        [Tooltip("The delay between finishing a level and fading to change to the next level.")]
        [SerializeField] private float _delayBeforeFading = 8.0f;
        [Tooltip("The delay between the ball death and fading to restart the level.")]
        [SerializeField] private float _delayAfterDeath = 4.0f;

        [Header("These scriptable objects are used by the Level Manager to find the correct levels to change to.")]
        [Tooltip("The scriptable object with a list of the UI scenes.")]
        [SerializeField] private LevelsScriptable UILevels;
        [Tooltip("The scriptable object with a list of the easy level scenes.")]
        [SerializeField] private LevelsScriptable EasyLevels;
        [Tooltip("The scriptable object with a list of the hard level scenes.")]
        [SerializeField] private LevelsScriptable HardLevels;

        private Dictionary<int, LevelSettingsScriptable> _allSceneSettingsIntDict = new Dictionary<int, LevelSettingsScriptable>();
        private Dictionary<string, LevelSettingsScriptable> _allSceneSettingsStringDict = new Dictionary<string, LevelSettingsScriptable>();

        public static LevelSettingsScriptable CurrentLvlSettings { get; private set; }

        private Animator _animator;
        private Canvas _canvas;
        private TextMeshProUGUI _lvlTitleText;

        #region OnStartUp
        private void Start()
        {
            _animator = GetComponent<Animator>();
            _animator.updateMode = AnimatorUpdateMode.UnscaledTime;
            _canvas = GetComponentInChildren<Canvas>();
            _lvlTitleText = GetComponentInChildren<TextMeshProUGUI>();

            CompleteLvl += ProceedToNextLevel;
            MainMenu += RestartGame;
            Pause += LoadPauseScene;
            PauseSceneManager.OnRestartLvl += RestartLevel;
            Ball.OnDestroyed += PlayerDied;
            Ball.OnFall += PlayerDied;
            LevelSceneController.OnChooseLvl += ChangeLvl;

            if (UILevels == null || EasyLevels == null || HardLevels == null)
            {
                ExceptionManager.instance.SendMissingObjectMessage("LevelsScriptable", GetType().ToString(), name);
            }
            else
            {
                LoadLevelSettings(UILevels);
                LoadLevelSettings(EasyLevels);
                LoadLevelSettings(HardLevels);
                LoadFromJson();
                SetFirstLevel();
                ChangeLvl(GlobalConstants.SPLASH_SCREEN, _preSplashDuration);
            }
        }


        /// <summary>
        /// Populates the dictionaries with the level settings of all of the levels so that they can be found and accessed quickly.
        /// </summary>
        /// <param name="levelList"></param> The scriptable level settings with a list of level settings to be loaded.
        private void LoadLevelSettings(LevelsScriptable levelList)
        {
            foreach (var level in levelList._levelSettings)
            {
                _allSceneSettingsIntDict.Add(level._levelNum, level);
                _allSceneSettingsStringDict.Add(level._levelName, level);
            }
        }

        /// <summary>
        /// Sets the first level to active if it isn't already set. This is only for when the game is first played.
        /// </summary>
        private void SetFirstLevel()
        {
            if (!_allSceneSettingsStringDict["In the Beginning"]._isActive)
            {
                _allSceneSettingsStringDict["In the Beginning"]._isActive = true;
            }
        }
        #endregion

        #region Level changing methods

        /// <summary>
        /// Loads a scene additively.
        /// </summary>
        /// <param name="sceneName"></param> The name of the scene to be loaded.
        public void LoadAdditiveScene(string sceneName)
        {
            SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        }

        /// <summary>
        /// Changes the Game Manager to 'change level' and begins the coroutine to fade.
        /// </summary>
        /// <param name="chosenLvl"></param> The name of the level to change to.
        public void ChangeLvl(string chosenLvl)
        {
            CurrentLvlSettings = _allSceneSettingsStringDict[chosenLvl];
            SetGameState(GameStates.ChangeLvl);
            StartCoroutine(ChangeLevelWithDelay(chosenLvl, 0));
        }

        /// <summary>
        /// Changes the Game Manager to 'change level' and begins the coroutine to fade after a specified duration.
        /// </summary>
        /// <param name="chosenLvl"></param> The name of the level to change to.
        /// <param name="delayAmount"></param> The amount of time to delay before changing the level.
        public void ChangeLvl(string chosenLvl, float delayAmount)
        {
            CurrentLvlSettings = _allSceneSettingsStringDict[chosenLvl];
            SetGameState(GameStates.ChangeLvl);
            StartCoroutine(ChangeLevelWithDelay(chosenLvl, delayAmount));
        }

        /// <summary>
        /// Restarts the game by loading the background scene with the main menu.
        /// </summary>
        private void RestartGame()
        {
            CurrentLvlSettings = _allSceneSettingsStringDict[GlobalConstants.SPLASH_SCREEN];
            // Change to the first scene of the game which initiates the main menu.
            ChangeLvl(GlobalConstants.SPLASH_SCREEN);
        }

        /// <summary>
        /// Restarts the current level.
        /// </summary>
        private void RestartLevel()
        {
            ChangeLvl(CurrentLvlSettings._levelName);
        }

        /// <summary>
        /// Restarts the current level and a short delay.
        /// </summary>
        private void PlayerDied()
        {
            ChangeLvl(CurrentLvlSettings._levelName, _delayAfterDeath);
        }
        #endregion

        #region Internal Methods

        /// <summary>
        /// Loads the option menu.
        /// </summary>
        private void LoadPauseScene()
        {
            LoadAdditiveScene(GlobalConstants.PAUSE_MENU);
        }

        /// <summary>
        /// Starts the coroutine to load the next level.
        /// </summary>
        private void ProceedToNextLevel()
        {
            string nextLvlName = _allSceneSettingsIntDict[CurrentLvlSettings._levelNum + 1]._levelName;
            StartCoroutine(ChangeLevelWithDelay(nextLvlName, _delayBeforeFading));
        }
        /// <summary>
        /// In order to advance to the next level we need to check if the current level number plus 1 is less than the total number of levels.
        /// </summary>
        private void UpdateLevelSettings(string newLvlName)
        {
            if (_allSceneSettingsStringDict[newLvlName]._levelNum <= (EasyLevels._levelSettings.Count + HardLevels._levelSettings.Count))
            {
                // Set the current level settings for the next level.
                CurrentLvlSettings = _allSceneSettingsStringDict[newLvlName];
            }
            else
            {
                // Otherwise set the curent level settings to the credits scene.
                CurrentLvlSettings = _allSceneSettingsStringDict[GlobalConstants.FINALE];
            }

        }

        /// <summary>
        /// Sets the fade out animation on the canvas that covers game play and displays a short message to the player ontop of that canvas.
        /// </summary>
        private void FadeOut()
        {
            _animator.SetTrigger("FadeOut");
            if (CurrentLvlSettings._levelName != GlobalConstants.SPLASH_SCREEN)
            {
                _lvlTitleText.text = "\n Loading Level: \n" + CurrentLvlSettings._levelName;
            }
            else
            {
                _lvlTitleText.text = "Loading...";
            }
        }

        /// <summary>
        /// Sets the z coordinates of the black canvas that covers game play when fading to be behind all other elements.
        /// </summary>
        private void ActivateFadeCanvas()
        {
            if (_canvas != null)
            {
                _canvas.sortingOrder = 200;
            }
            else
            {
                ExceptionManager.instance.SendMissingComponentMessage("_canvas", GetType().ToString(), name);
            }
        }

        /// <summary>
        /// Sets the z coordinates of the black canvas that covers game play when fading to be in front of all other elements. This method is run from a key frame at the end of the 'fade in' animation.
        /// </summary>
        private void DeactivateFadeCanvas()
        {
            _canvas.sortingOrder = -2;
        }

        /// <summary>
        /// After a specified duration this method changes the Game Manager state to 'change level', sets the current level settings to the new level that is being loaded and begins fading out the 
        /// black canvas that covers the game play.
        /// </summary>
        /// <param name="newLvlName"></param> The name of the level to be loaded.
        /// <param name="delayBeforeFading"></param> The amount of time to delay before loading the level.
        /// <returns></returns>
        private IEnumerator ChangeLevelWithDelay(string newLvlName, float delayBeforeFading)
        {
            yield return new WaitForSeconds(delayBeforeFading);
            SetGameState(GameStates.ChangeLvl);
            UpdateLevelSettings(newLvlName);
            FadeOut();
        }

        /// <summary>
        /// Begins the 'fade in' animation for the black canvas that covers the game play once the new level is loaded. This method is run from a key frame at the end of the 'fade out' animation.
        /// </summary>
        /// <returns></returns>
        private IEnumerator OnLvlLoadComplete()
        {
            
            AsyncOperation ao = LoadLvl();

            yield return ao;

            FadeInScene();
        }

        /// <summary>
        /// Sets the Game Manager state to 'Play', activates the black canvas that cover the game play and beings the 'fade in' animation.
        /// </summary>
        private void FadeInScene()
        {
            if (CurrentLvlSettings._sceneType != SceneType.UI)
            {
                SetGameState(GameStates.Play);
            }
            ActivateFadeCanvas();
            _animator.SetTrigger("FadeIn");
        }

        /// <summary>
        /// Loads the relevant level as per the current level settings that have been updated on this script. This method is called from the animated fade controller once the new level is loaded.
        /// </summary>
        /// <returns></returns>
        private AsyncOperation LoadLvl()
        {
            return SceneManager.LoadSceneAsync(CurrentLvlSettings._levelName);
        }

        /// <summary>
        /// Returns the relevant scriptable level settings level as per the specified level number.
        /// </summary>
        /// <param name="levelNum"></param> The level number of the desired scriptable level settings.
        /// <returns></returns>
        public LevelSettingsScriptable GetLevel(int levelNum)
        {
            return _allSceneSettingsIntDict[levelNum];
        }

        private void LoadFromJson()
        {
            for (int i = 0; i < EasyLevels._levelSettings.Count; i++)
            {
                string filePath = SaveScores.SAVE_FOLDER + "/savedlevel" + EasyLevels._levelSettings[i]._levelName + ".json";
                if (File.Exists(filePath))
                {
                    string settingsData = File.ReadAllText(filePath);
                    JsonUtility.FromJsonOverwrite(settingsData, EasyLevels._levelSettings[i]);
                }
            }
            for (int i = 0; i < HardLevels._levelSettings.Count; i++)
            {
                string filePath = SaveScores.SAVE_FOLDER + "/savedlevel" + HardLevels._levelSettings[i]._levelName + ".json";
                if (File.Exists(filePath))
                {
                    string settingsData = File.ReadAllText(filePath);
                    JsonUtility.FromJsonOverwrite(settingsData, HardLevels._levelSettings[i]);
                }
            }
        }

        #endregion
    }
}