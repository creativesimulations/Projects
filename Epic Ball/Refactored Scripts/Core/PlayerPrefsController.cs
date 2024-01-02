using static UnityEngine.PlayerPrefs;
using static EpicBall.GlobalConstants;

namespace EpicBall
{
    public static class PlayerPrefsController
    {

        #region Scoring

        /// <summary>
        /// Increases the saved number of completed levels by 1.
        /// </summary>
        public static void IncreaseLvlsCompleted()
        {
            SetInt(LEVELS_COMPLETED, GetLvlsCompleted() + 1);
            Save();
        }

        /// <summary>
        /// Returns the saved number of completed levels.
        /// </summary>
        public static int GetLvlsCompleted()
        {
            return GetInt(LEVELS_COMPLETED);
        }

        /// <summary>
        /// Increases the saved number of active levels by 1.
        /// </summary>
        public static void IncreaseActiveLvls()
        {
            SetInt(LEVELS_ACTIVE, GetActiveLvls() + 1);
            Save();
        }

        /// <summary>
        /// Returns the saved number of active levels.
        /// </summary>
        public static int GetActiveLvls()
        {
            return GetInt(LEVELS_ACTIVE);
        }

        /// <summary>
        /// Saves the value of the high score.
        /// </summary>
        /// <param name="currentHighScore"></param> The high score value.
        public static void SetHighScore(int currentHighScore)
        {
            SetInt(HIGH_SCORE, currentHighScore);
            Save();
        }

        /// <summary>
        /// Returns the saved value of the high score.
        /// </summary>
        public static int GetHighScore()
        {
            return GetInt(HIGH_SCORE);
        }

        /// <summary>
        /// Returns whether a specified button has an active star or not.
        /// </summary>
        public static bool CheckForStar(string keyName)
        {
            bool hasStar;
            if (!HasKey(keyName))
            {
                hasStar = false;
            }
            else
            {
                hasStar = true;
            }
            return hasStar;
        }

        /// <summary>
        /// Sets a value of '10' for a specified button to denote that a star is active.
        /// </summary>
        /// <param name="keyName"></param> The button name for which to set an active star.
        public static void SetStar(string keyName)
        {
            // The number 10 is just to set the key to something other than 0.
            SetInt(keyName, 10);
            Save();
        }

        /// <summary>
        /// Removes a key for a specified button to denote that it doesn't have an active star.
        /// </summary>
        /// <param name="keyName"></param> The button name for which to remove a star.
        public static void RemoveStar(string keyName)
        {
            if (HasKey(keyName))
            {
                DeleteKey(keyName);
            }
            Save();
        }

        #endregion

        #region Skins

        /// <summary>
        /// Saves which ball skin is currently chosen.
        /// </summary>
        /// <param name="skinNum"></param> the number of skin found in the skin list.
        public static void SetChosenSkin(int skinNum)
        {
            SetInt(CHOSEN_PLAYER_KEY, skinNum);
            Save();
        }

        /// <summary>
        /// Returns the saved value of skin currently chosen.
        /// </summary>
        /// <returns></returns>
        public static int GetChosenSkin()
        {
            return GetInt(CHOSEN_PLAYER_KEY, 0);
        }

        /// <summary>
        /// Increases the saved number of available skins by 1.
        /// </summary>
        public static void IncreaseAvailableSkins()
        {
            SetInt(SKINS_AVAILABLE, GetSkinAmountAvailable() + 1);
            Save();
        }

        /// <summary>
        /// Returns the saved value of skins currently available.
        /// </summary>
        /// <returns></returns>
        public static int GetSkinAmountAvailable()
        {
            return GetInt(SKINS_AVAILABLE);
        }

        /// <summary>
        /// Returns whether a specified number is less than the amount of skins currently available.
        /// </summary>
        /// <param name="skinNum"></param> The number to check against the amount of skins currently available.
        /// <returns></returns>
        public static bool CheckIfSkinLocked(int skinNum)
        {
            if (GetSkinAmountAvailable() > skinNum)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion


        #region Controls

        /// <summary>
        /// Returns what value is saved for the player controls direction.
        /// </summary>
        /// <returns></returns>
        public static int GetControlsDirection()
        {
            return GetInt(CONTROL_DIRECTION, 0);
        }

        /// <summary>
        /// Sets and saves the player control direction.
        /// </summary>
        /// <param name="direction"></param> The number to define which direction the player controls should be. '0' is for left handed and '1' is for right handed.
        public static void SetControlsDirection(int direction)
        {
            SetInt(CONTROL_DIRECTION, direction);
            Save();
        }

        #endregion


        #region Audio

        /// <summary>
        /// Sets and saves the music and sound volumes.
        /// </summary>
        /// <param name="musicVolume"></param> Music volume value.
        /// <param name="soundVolume"></param> Sound volume value.
        public static void SetAudioVolume(float musicVolume, float soundVolume)
        {
            SetFloat(MUSIC_VOLUME_KEY, musicVolume);
            SetFloat(SOUND_VOLUME_KEY, soundVolume);
            Save();
        }

        /// <summary>
        /// Returns the music volume value.
        /// </summary>
        /// <returns></returns>
        public static float GetMusicVolume()
        {
            return GetFloat(MUSIC_VOLUME_KEY, .3f);
        }

        /// <summary>
        /// Returns the sound volume value.
        /// </summary>
        /// <returns></returns>
        public static float GetSoundVolume()
        {
            return GetFloat(SOUND_VOLUME_KEY, .5f);
        }

        #endregion
        public static void ResetPrefs()
        {
            DeleteAll();
            GameManager.AudioVolume(GetMusicVolume(), GetSoundVolume());
            Save();
        }

    }
}