using UnityEngine;
using static EpicBall.GameManager;
using static EpicBall.PlayerPrefsController;
using static EpicBall.GlobalConstants;
using System;
using System.IO;

namespace EpicBall
{
    public class Scoring : SaveScores
    {

        public static event Action<bool, bool, bool, bool, bool, bool> DisplayScore;
        [Header("Setup for the scoring during gameplay. How many bonus points will be given for each of these achievements.")]
        [Tooltip("How many points will be received when collecting a gold gem.")]
        [SerializeField] private int _goldGemPrize = 1000;
        [Tooltip("How many points will be received when collecting a purple gem.")]
        [SerializeField] private int _purpleGemPrize = 10;
        [Tooltip("How many points will be received when the player is awarded a gold medal.")]
        [SerializeField] private int _goldMedalPrize = 1000;
        [Tooltip("How many points will be received when the player is awarded a silver medal.")]
        [SerializeField] private int _silverMedalPrize = 200;
        [Tooltip("How many points will be received when the player is awarded a bronze medal.")]
        [SerializeField] private int _bronzeMedalPrize = 50;
        [Tooltip("How many points will be received when the player breaks a glass block or a TNT block.")]
        [SerializeField] private int _destroyBlockPrize = 2;
        [Tooltip("How many points will be received when the player completes the level faster than a previous time.")]
        [SerializeField] private int _bestTime = 100;

        [Tooltip("How many bonus points will be given per second that the level is finished quicker than the time required to get a gold medal.")]
        [SerializeField] private int _timeBonusPrize = 11;


        private int newScore;
        private int newTime;
        private string newMedal;
        private int newbonusPoints;

        private bool isCollectedGoldGem;
        private bool isNewBestScore;
        private bool isNewBestTime;
        private bool isNewMedal;
        private bool isNewHighScore;
        private bool isNewSkin;

        private void Awake()
        {
            SAVE_FOLDER = Application.persistentDataPath + "/Saved Game/";
            if (!Directory.Exists(SAVE_FOLDER))
            {
                Directory.CreateDirectory(SAVE_FOLDER);
            }
        }
        void Start()
        {
            TntBlock.OnExplode += AddExplodePrize;
            GlassBlock.OnBreak += AddExplodePrize;
            Gem.gemCollected += GemCollected;
            CompleteLvl += ScoringTotals;

            // When the game is first played the active levels are 0, so this increases it to 1, since the first level is active to be played.
            if (GetActiveLvls() < 1)
            {
                IncreaseActiveLvls();
            }
        }

        /// <summary>
        /// Runs all of the scoring calculations when the level is completed.
        /// </summary>
        public void ScoringTotals()
        {
            ProcessCompletedLvl();
            ProcessGoldGem();
            AddPrizes(TIME);
            CalculateMedal();
            SetCurrentLevelScore();
            SaveInfo();
            SendDisplayNotification();
            ResetScriptScores();
        }

        /// <summary>
        /// Resets the fields that declare whether a specific score is new or better than a previous score.
        /// </summary>
        private void ResetScriptScores()
        {
            newScore = 0;
            newTime = 0;
            newMedal = "";
            newbonusPoints = 0;
            isNewMedal = false;
            isNewHighScore = false;
            isNewBestScore = false;
            isNewBestTime = false;
            isNewSkin = false;
            isCollectedGoldGem = false;
        }

        /// <summary>
        /// Adds the score amount of a given achievement to the bonus score, which will be added to the score when the level is completed.
        /// </summary>
        /// <param name="prizeType"></param> The type of prize that was achieved.
        #region Prizes
        private void AddPrizes(string prizeType)
        {
            switch (prizeType)
            {
                case DESTROY_BLOCK_PRIZE:
                    newbonusPoints += _destroyBlockPrize;
                    break;

                case PURPLE_GEM_PRIZE:
                    newScore += _purpleGemPrize;
                    break;

                case GOLD_GEM_PRIZE:
                    newbonusPoints += _goldGemPrize;
                    break;

                case GOLD_MEDAL:
                    newScore += _goldMedalPrize;
                    break;

                case SILVER_MEDAL:
                    newScore += _silverMedalPrize;
                    break;

                case BRONZE_MEDAL:
                    newScore += _bronzeMedalPrize;
                    break;
                case TIME:
                    newbonusPoints += CalculateTimebonus();
                    break;

                default:
                    Debug.LogError("No prize was given for " + prizeType + ". Please allocate a prize for " + prizeType);
                    break;

            }
        }

        /// <summary>
        /// Adds a prize when breaking a glass block or a TNT block.
        /// </summary>
        private void AddExplodePrize()
        {
            AddPrizes(DESTROY_BLOCK_PRIZE);
        }

        /// <summary>
        /// Adds a prize when collecting a purple or gold gem.
        /// </summary>
        /// <param name="isGoldGem"></param> If the gem collected is a gold gem.
        private void GemCollected(bool isGoldGem)
        {
            if (!isGoldGem)
            {
                AddPrizes(PURPLE_GEM_PRIZE);
            }
            else
            {
                isCollectedGoldGem = true;
                AddPrizes(GOLD_GEM_PRIZE);
            }
        }

        /// <summary>
        /// Adds a prize when awarded a medal.
        /// </summary>
        /// <param name="newMedalType"></param> The type of medal that the player is awarded.
        private void SetNewMedal(string newMedalType)
        {
            AddPrizes(newMedalType);
            newMedal = newMedalType;
        }

        /// <summary>
        /// If a gold gem was collected and had not been collected previously, then a check will be made if there is another level available to be activated.
        /// </summary>
        private void ProcessGoldGem()
        {
            if (isCollectedGoldGem && !LevelManager.CurrentLvlSettings._goldGemCollected)
            {
                ActivateLevel(CheckBeforeActivatingNewLevel());
            }
        }

        /// <summary>
        /// If the level is completed and had not been completed previously, then a check will be made if there is another level available to be activated.
        /// </summary>
        private void ProcessCompletedLvl()
        {
            if (!LevelManager.CurrentLvlSettings._isCompleted)
            {
                ActivateLevel(CheckBeforeActivatingNewLevel());
                IncreaseLvlsCompleted();
                CalculateSkin();
            }
        }
        #endregion

        #region Calculations

        /// <summary>
        /// Checks how long it took to complete the level and awards a gold, silver or bronze medal accordingly.
        /// </summary>
        private void CalculateMedal()
        {
            if (newTime <= LevelManager.CurrentLvlSettings._goldTime)
            {
                SetNewMedal(GOLD_MEDAL);
                if (LevelManager.CurrentLvlSettings._medal != GOLD_MEDAL)
                {
                    isNewMedal = true;
                }
            }
            else if (newTime > LevelManager.CurrentLvlSettings._goldTime && newTime < LevelManager.CurrentLvlSettings._silverTime)
            {
                SetNewMedal(SILVER_MEDAL);
                if (LevelManager.CurrentLvlSettings._medal != SILVER_MEDAL && LevelManager.CurrentLvlSettings._medal != GOLD_MEDAL)
                {
                    isNewMedal = true;
                }
            }
            else
            {
                SetNewMedal(BRONZE_MEDAL);
                if (LevelManager.CurrentLvlSettings._medal != BRONZE_MEDAL)
                {
                    isNewMedal = true;
                }
            }
        }

        /// <summary>
        /// Checks if three new levels have been completed and if so, increases the number of skins available by 1 and sets the appropriate star to be active
        /// for the buttons on the game menus.
        /// </summary>
        private void CalculateSkin()
        {
            if (GetLvlsCompleted() % 3 == 0 && GetSkinAmountAvailable() != SKINS_AMOUNT)
            {
                isNewSkin = true;
                IncreaseAvailableSkins();
                if (!CheckForStar(SKINS_STAR))
                {
                    SetStar(SKINS_STAR);
                }
            }
        }

        /// <summary>
        /// Checks if the level was completed quicker than a previous saved time for this level and then returns the amount plus the bonus point if earned.
        /// </summary>
        /// <returns></returns>
        private int CalculateTimebonus()
        {
            int bonusTimePoints = 0;
            newTime = Timer.GetIntTime();
            int previousBestTime = LevelManager.CurrentLvlSettings._bestTime;
            // If the current time it took to finish this level is less (better) than the previously saved best time, add the desired time bonus
            if (previousBestTime == 0 || newTime < previousBestTime)
            {
                bonusTimePoints += _bestTime;
                isNewBestTime = true;
            }

            if (LevelManager.CurrentLvlSettings._goldTime >= newTime)
            {
                // get the amount of seconds that the level was finished quicker than the required time to get the gold medal multiplied by the desired time bonus prize.
                bonusTimePoints += (LevelManager.CurrentLvlSettings._goldTime - newTime) * _timeBonusPrize;
            }
            return bonusTimePoints;
        }
        #endregion

        #region Set Scores

        /// <summary>
        /// Sets the total score for the level.
        /// </summary>
        private void SetCurrentLevelScore()
        {
            // Add the bonus points to the regular points.
            newScore += newbonusPoints;

            // If the current best score for this level is less than the current score total, then save the current score as the new best score for this level.
            if (LevelManager.CurrentLvlSettings._bestScore < newScore)
            {
                isNewBestScore = true;
                // Since this is a new high score for this level, the new all time high score needs to be saved as well.
                SetNewHighScore();
            }
        }

        /// <summary>
        /// Sets the new high score amount if the current total score is better than the previous high score for this level.
        /// </summary>
        private void SetNewHighScore()
        {
            isNewHighScore = true;
            // Subtract the previous best score for this level from the high score and add the new current best score.
            SetHighScore(GetHighScore() - LevelManager.CurrentLvlSettings._bestScore + newScore);
            // Set a star on the leaderboard to notify of an update to the high score.
            SetStar(LEADERBOARD_STAR);
        }

        /// <summary>
        /// Checks if there are any easy or hard levels to be made available and returns the scriptable level settings that should be made active.
        /// </summary>
        /// <returns></returns>
        private LevelSettingsScriptable CheckBeforeActivatingNewLevel()
        {
            if (_easyLevels == null)
            {
                ExceptionManager.instance.SendMissingObjectMessage("_easyLevels", GetType().ToString(), name);
                return null;
            }
            else if (_hardLevels == null)
            {
                ExceptionManager.instance.SendMissingObjectMessage("_hardLevels", GetType().ToString(), name);
                return null;
            }
            else
            {
                LevelSettingsScriptable activateLvl;
                int activeLvls = GetActiveLvls();
                if (activeLvls >= (_easyLevels._levelSettings.Count + _hardLevels._levelSettings.Count))
                {
                    return null;
                }
                if (activeLvls < _easyLevels._levelSettings.Count)
                {
                    // Put a star on the main menu easy lvls button if there isn't a star there yet.
                    if (!CheckForStar(EASY_LEVELS_STAR))
                    {
                        SetStar(EASY_LEVELS_STAR);
                    }
                    activateLvl = _easyLevels._levelSettings[activeLvls];
                }
                else
                {
                    // Put a star on the main menu hard lvls button if there isn't a star there yet.
                    if (!CheckForStar(HARD_LEVELS_STAR))
                    {
                        SetStar(HARD_LEVELS_STAR);
                    }
                    // must subtract the number of easy levels in order to get the poper hard level numbers.
                    activateLvl = _hardLevels._levelSettings[activeLvls - _easyLevels._levelSettings.Count];
                }
                IncreaseActiveLvls();
            return activateLvl;
            }
        }


        #endregion

        /// <summary>
        /// Informs the base method of the information that is to be updated on the current scriptable level settings.
        /// </summary>
        public void SaveInfo()
        {
            UpdateLevelScore(newTime, newScore, isCollectedGoldGem, newMedal);
        }

        /// <summary>
        /// Sends a notification of which scores are new and should be animated.
        /// </summary>
        public void SendDisplayNotification()
        {
            DisplayScore?.Invoke(isCollectedGoldGem, isNewBestScore, isNewBestTime, isNewMedal, isNewHighScore, isNewSkin);
        }

        /// <summary>
        /// Disables the delegates that this script is subscribed to.
        /// </summary>
        private void OnDisable()
        {
            Gem.gemCollected -= GemCollected;
            CompleteLvl -= ScoringTotals;
            TntBlock.OnExplode -= AddExplodePrize;
            GlassBlock.OnBreak -= AddExplodePrize;
        }
    }
}