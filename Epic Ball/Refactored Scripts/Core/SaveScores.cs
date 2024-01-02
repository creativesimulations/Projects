using UnityEngine;
using System.IO;

namespace EpicBall
{

    public class SaveScores : MonoBehaviour
    {

        public static string SAVE_FOLDER;

        [Tooltip("Put the scriptable object with a list of the easy levels here.")]
        [SerializeField] protected LevelsScriptable _easyLevels;
        [Tooltip("Put the scriptable object with a list of the hard levels here.")]
        [SerializeField] protected LevelsScriptable _hardLevels;

        private LevelSettingsScriptable _currentLvlSettings;

        /// <summary>
        /// Gets the current level settings and updates it according to the supplied information.
        /// </summary>
        /// <param name="currentLvl"></param> The sciptable level object to update.
        /// <param name="completedTime"></param> The time it took to complete the level.
        /// <param name="score"></param> The total score when completing the level.
        /// <param name="goldGemCollected"></param> Whether or not a gold gem ws collected in this level.
        /// <param name="medalWon"></param> The medal that was won when the level was completed.
        protected void UpdateLevelScore(int completedTime, int score, bool goldGemCollected, string newMedalWon)
        {
            _currentLvlSettings = LevelManager.CurrentLvlSettings;
            if (_currentLvlSettings == null)
            {
                ExceptionManager.instance.SendMissingObjectMessage("_currentLvlSettings", GetType().ToString(), name);
            }
            else
            {
                CompleteLvl();
                UpdateTime(completedTime);
                UpdateScore(score);
                UpdateGoldGem(goldGemCollected);
                UpdateMedal(newMedalWon);
                SaveToJson();
            }
        }

        /// <summary>
        /// Sets the scriptable object for the current level as having been completed.
        /// </summary>
        private void CompleteLvl()
        {
            if (!_currentLvlSettings._isCompleted)
            {
                _currentLvlSettings._isCompleted = true;
            }
        }

        /// <summary>
        /// Makes a new level available to be played.
        /// </summary>
        /// <param name="lvlNumToActivate"></param> The number of the level that should be activated.
        protected void ActivateLevel(LevelSettingsScriptable lvlToActivate)
        {
            if (lvlToActivate != null)
            {
                lvlToActivate._isActive = true;
            }
        }

        /// <summary>
        /// Checks a new score amount against the previous scored amount and returns 'true' it is, or 'false' if it isn't.
        /// </summary>
        /// <param name="newAmount"></param> The new score amount.
        /// <param name="oldAmount"></param> The previous score amount.
        /// <returns></returns>
        private bool NewAmountIsMore(int newAmount, int oldAmount)
        {
            bool result;
            if (newAmount > oldAmount)
            {
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }

        /// <summary>
        /// Sets the best score on the current level scriptable object as the highest attained score for this level, whether it is the new score or the previous score.
        /// </summary>
        /// <param name="score"></param> The new score.
        private void UpdateScore(int score)
        {
            if (NewAmountIsMore(score, _currentLvlSettings._bestScore))
            {
                _currentLvlSettings._bestScore = score;
            }
        }

        /// <summary>
        /// Sets the best completed time on the level scriptable object as the lowest completed time for this level, whether it is the new time or the previous time.
        /// </summary>
        /// <param name="completedTime"></param> The new time.
        private void UpdateTime(int completedTime)
        {
            if (_currentLvlSettings._bestTime == 0 || !NewAmountIsMore(completedTime, _currentLvlSettings._bestTime))
            {
                _currentLvlSettings._bestTime = completedTime;
            }
        }

        /// <summary>
        /// Sets the collected gold gem on the current level scriptable object as true or false depending on if it was collected.
        /// </summary>
        /// <param name="goldgemCollected"></param> If the gold gem was collected.
        private void UpdateGoldGem(bool goldgemCollected)
        {
            if (goldgemCollected && !_currentLvlSettings._goldGemCollected)
            {
                _currentLvlSettings._goldGemCollected = true;
            }
        }

        /// <summary>
        /// Sets the medal awarded on the current level scriptable object.
        /// </summary>
        /// <param name="newMedalWon"></param> The medal name that was awarded.
        private void UpdateMedal(string newMedalWon)
        {
            switch (newMedalWon)
            {
                case GlobalConstants.GOLD_MEDAL:
                    if (_currentLvlSettings._medal != newMedalWon)
                    {
                        _currentLvlSettings._medal = newMedalWon;
                    }
                    break;
                case GlobalConstants.SILVER_MEDAL:
                    if (_currentLvlSettings._medal != newMedalWon && _currentLvlSettings._medal != GlobalConstants.GOLD_MEDAL)
                    {
                        _currentLvlSettings._medal = newMedalWon;
                    }
                    break;
                case GlobalConstants.BRONZE_MEDAL:
                    if (_currentLvlSettings._medal != newMedalWon && _currentLvlSettings._medal != null)
                    {
                        _currentLvlSettings._medal = newMedalWon;
                    }
                    break;
                default:
                    break;
            }
        }

        private void SaveToJson()
        {
            for (int i = 0; i < _easyLevels._levelSettings.Count; i++)
            {
                string levelData = JsonUtility.ToJson(_easyLevels._levelSettings[i]);
                File.WriteAllText(SAVE_FOLDER + "/savedlevel" + _easyLevels._levelSettings[i]._levelName + ".json", levelData);
            }
            for (int i = 0; i < _hardLevels._levelSettings.Count; i++)
            {
                string levelData = JsonUtility.ToJson(_hardLevels._levelSettings[i]);
                File.WriteAllText(SAVE_FOLDER + "/savedlevel" + _hardLevels._levelSettings[i]._levelName + ".json", levelData);
            }
        }
    }
}
