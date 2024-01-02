using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EpicBall
{
    public class AwardsController : MonoBehaviour
    {

        [Header("Set up the various components of the award scene.")]
        [Tooltip("The main awards canvas that will be activated when a level is completed.")]
        [SerializeField] private GameObject _canvas;
        [Tooltip("The object where the new medal image will be shown.")]
        [SerializeField] private GameObject _medalImg;
        [Tooltip("The object where the gold gem image will be shown if it is collected.")]
        [SerializeField] private GameObject _goldGemImg;
        [Tooltip("The object where the new skin image will be shown when it becomes available.")]
        [SerializeField] private GameObject _newBallImg;

        [Tooltip("The gold star to be shown if it is achieved.")]
        [SerializeField] private Sprite _goldStar;
        [Tooltip("The silver star to be shown if it is achieved.")]
        [SerializeField] private Sprite _silverStar;
        [Tooltip("The bronze star to be shown if it is achieved.")]
        [SerializeField] private Sprite _bronzeStar;

        [Tooltip("The text object where the time will be shown.")]
        [SerializeField] private TextMeshProUGUI _timeText;
        [Tooltip("The text object where the score will be shown.")]
        [SerializeField] private TextMeshProUGUI _scoreText;
        [Tooltip("The text object where the new skin text will be shown.")]
        [SerializeField] private TextMeshProUGUI _skinText;
        [Tooltip("The text object where the gold gem message will be shown if it is collected.")]
        [SerializeField] private TextMeshProUGUI _goldGemText;
        [Tooltip("The text object where the medal message will be shown.")]
        [SerializeField] private TextMeshProUGUI _medalText;
        [Tooltip("The text object where the high score will be shown.")]
        [SerializeField] private TextMeshProUGUI _highScoreText;

        /// <summary>
        /// A list of congratulatory messages to be added to the final award text.
        /// </summary>
        private List<string> _congrats = new List<string> { "Congrats!!", "Woohoo!", "Woot!!", "YES!", "Awesome!", "COOL!", "Way to go!", "Finally!", "GREAT!", "Wonderful!!", "Fantastic!", "INCREDIBLE!", "Superb!", "Terrific!!", "Outstanding!", "Magnificient!!", "Phenomenal!", "Fabulous!", "Excellent!!", "BRILLIANT!", "Amazing!", "Impressive!" };

        /// <summary>
        /// A list of the skin names that will be shown when a new skin becomes available.
        /// </summary>
        private List<string> _skinNames = new List<string> { "Matrix", "Fireball Icon", "Die Icon", "Billiard Icon", "Eye Icon", "Beach Icon", "Spark Icon", "Bucky Icon", "Wood Icon", "Wheel Icon", "Bubble Icon", "Spike Icon", "Penguin Icon", "Yoyo Icon", "Banana Icon", "Soccer Icon", "Spirit Icon", "Chicken Icon" };

        private LevelSettingsScriptable _currentLvlSettings;

        void Start()
        {
            Scoring.DisplayScore += UpdateLevelAccomplishments;
            GameManager.ChangeLvl += DisableCanvas;
            DisableCanvas();
        }

        /// <summary>
        /// This is the main method that activates the awards scene and populates it with the relevant information. This method is run when from the scoring notification.
        /// </summary>
        /// <param name="currentLvl"></param> The sciptable level object to update.
        /// <param name="completedTime"></param> The time it took to complete the level.
        /// <param name="score"></param> The total score when completing the level.
        /// <param name="goldGemCollected"></param> Whether or not a gold gem ws collected in this level.
        /// <param name="medalWon"></param> The medal that was won when the level was completed.
        private void UpdateLevelAccomplishments(bool collectedGoldGem, bool bestScore, bool bestTime, bool bestMedal, bool highestScore, bool newSkin)
        {
            if (_canvas == null)
            {
                Singleton.instance.GetComponent<ExceptionManager>().SendMissingObjectMessage("_canvas", this.GetType().ToString(), name);
                return;
            }
            _canvas.SetActive(true);

            _currentLvlSettings = LevelManager.CurrentLvlSettings;

            UpdateScore(bestScore);
            UpdateHighScore(highestScore);
            UpdateTime(bestTime);
            UpdateGoldGem(collectedGoldGem);
            UpdateMedal(bestMedal);
            UpdateSkin(newSkin);
        }

        #region Updates
        /// <summary>
        /// Displays the score that has been saved to the current level scriptable object.
        /// </summary>
        /// <param name="newScore"></param> Declares if the score is higher than the previous one.
        public void UpdateScore(bool newScore)
        {
            if (_scoreText == null)
            {
                Singleton.instance.GetComponent<ExceptionManager>().SendMissingObjectMessage("_scoreText", this.GetType().ToString(), name);
                return;
            }
            int bestScore = _currentLvlSettings._bestScore;
            StartCoroutine(IncreaseNumber(_scoreText, bestScore, bestScore));
            Pop(_scoreText, newScore);
        }

        /// <summary>
        /// Displays the high score that has been saved in PlayerPrefs.
        /// </summary>
        /// <param name="newHighScore"></param> Declares if the high score is higher than the previous one.
        private void UpdateHighScore(bool newHighScore)
        {
            if (_highScoreText == null)
            {
                Singleton.instance.GetComponent<ExceptionManager>().SendMissingObjectMessage("_highScoreText", this.GetType().ToString(), name);
                return;
            }
            int highScore = PlayerPrefsController.GetHighScore();
            StartCoroutine(IncreaseNumber(_highScoreText, highScore, highScore));
            Pop(_highScoreText, newHighScore);
        }

        /// <summary>
        /// Displays the time it took to complete the level and was saved to the current level scriptable object.
        /// </summary>
        /// <param name="newTime"></param> Declares if the time is lower than the previous one.
        private void UpdateTime(bool newTime)
        {
            if (_timeText == null)
            {
                Singleton.instance.GetComponent<ExceptionManager>().SendMissingObjectMessage("_timeText", this.GetType().ToString(), name);
                return;
            }
            _timeText.GetComponent<TMPro.TextMeshProUGUI>().text = Timer.StringTime(_currentLvlSettings._bestTime);
            Pop(_timeText, newTime);
        }

        /// <summary>
        /// Displays the gold gem message and enables the relevant image.
        /// </summary>
        /// <param name="goldGemCollected"></param> Declares if the gold gem was collected.
        private void UpdateGoldGem(bool goldGemCollected)
        {
            if (_goldGemImg == null)
            {
                Singleton.instance.GetComponent<ExceptionManager>().SendMissingObjectMessage("_goldGemImg", this.GetType().ToString(), name);
                return;
            }
            if (_goldGemText == null)
            {
                Singleton.instance.GetComponent<ExceptionManager>().SendMissingObjectMessage("_goldGemText", this.GetType().ToString(), name);
                return;
            }
            if (goldGemCollected)
            {
                _goldGemImg.GetComponent<RawImage>().enabled = true;
                _goldGemText.text = (AddCongrats() + " You collected the gold gem!");
                Pop(_goldGemText, goldGemCollected);
            }
            else
            {
                _goldGemImg.GetComponent<RawImage>().enabled = false;
                AddCongrats();
            }
        }

        /// <summary>
        /// Displays the medal that has been saved to the current level scriptable object.
        /// </summary>
        /// <param name="newmedalWon"></param> Declares if the medal is better than the previous one.
        private void UpdateMedal(bool newmedalWon)
        {
            if (_medalText == null)
            {
                Singleton.instance.GetComponent<ExceptionManager>().SendMissingObjectMessage("_medalText", this.GetType().ToString(), name);
                return;
            }
            _medalText.text = (AddCongrats() + " You won a " + _currentLvlSettings._medal + "!");
            DisplayMedalImg(_currentLvlSettings._medal);
            Pop(_medalText, newmedalWon);
        }
        #endregion

        /// <summary>
        /// Displays the correct medal image depending on which medal name was saved in the level scriptable object for the current level.
        /// </summary>
        /// <param name="medal"></param> The medal name that was awarded.
        private void DisplayMedalImg(string medal)
        {
            switch (medal)
            {
                case GlobalConstants.GOLD_MEDAL:
                    if (_goldStar == null)
                    {
                        Singleton.instance.GetComponent<ExceptionManager>().SendMissingObjectMessage("_goldStar", this.GetType().ToString(), name);
                        return;
                    }
                    _medalImg.GetComponent<Image>().sprite = _goldStar;
                    break;
                case GlobalConstants.SILVER_MEDAL:
                    if (_silverStar == null)
                    {
                        Singleton.instance.GetComponent<ExceptionManager>().SendMissingObjectMessage("_silverStar", this.GetType().ToString(), name);
                        return;
                    }
                    _medalImg.GetComponent<Image>().sprite = _silverStar;
                    break;
                case GlobalConstants.BRONZE_MEDAL:
                    if (_bronzeStar == null)
                    {
                        Singleton.instance.GetComponent<ExceptionManager>().SendMissingObjectMessage("_bronzeStar", this.GetType().ToString(), name);
                        return;
                    }
                    _medalImg.GetComponent<Image>().sprite = _bronzeStar;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Displays the correct skin image depending on which new skin is to be available.
        /// </summary>
        /// <param name="newSkin"></param> Declares if a new skin is available.
        private void UpdateSkin(bool newSkin)
        {
            if (_newBallImg == null)
            {
                Singleton.instance.GetComponent<ExceptionManager>().SendMissingObjectMessage("_newBallImg", this.GetType().ToString(), name);
                return;
            }
            if (_skinText == null)
            {
                Singleton.instance.GetComponent<ExceptionManager>().SendMissingObjectMessage("_skinText", this.GetType().ToString(), name);
                return;
            }
            if (newSkin)
            {
                _newBallImg.SetActive(true);
                _newBallImg.GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>(_skinNames[PlayerPrefsController.GetSkinAmountAvailable()]);
                _skinText.text = "New Ball!";
                Pop(_skinText, newSkin);
            }
            else
            {
                _skinText.text = null;
                _newBallImg.SetActive(false);
            }
        }

        /// <summary>
        /// Returns a congratulations message based on a random entry from the "_congrats" list.
        /// </summary>
        private string AddCongrats()
        {
            string _currentCongrats = _congrats[Random.Range(0, _congrats.Count)];
            return _currentCongrats;
        }


        /// <summary>
        /// Displays a number text in a given gameobject and quickly increases it from 0 to the desired number in a spooling effect.
        /// </summary>
        /// <param name="textToRun"></param> The gameobject where the text is to be displayed.
        /// <param name="numberGoal"></param> The number to be displayed
        /// <param name="divideBy"></param> This number speeds up or slows down the spooling effect.
        /// <returns></returns>
        IEnumerator IncreaseNumber(TextMeshProUGUI textToRun, int numberGoal, int divideBy)
        {
            int currentNumber = 0;
            float addedNumber = 0;
            while (currentNumber < numberGoal)
            {
                addedNumber += (divideBy * Time.deltaTime);
                currentNumber = (int)addedNumber;
                textToRun.text = currentNumber.ToString();

                yield return new WaitForEndOfFrame();
            }
            textToRun.text = numberGoal.ToString();
        }

        /// <summary>
        /// Plays an animation which highlights the given text and makes a "pop" effect. This is run when an achievement is new, or better than a previous achievement.
        /// </summary>
        /// <param name="textToRun"></param> The text to be animated.
        /// <param name="pop"></param> Declares if it should be animated.
        private void Pop(TextMeshProUGUI textToRun, bool pop)
        {
            if (pop)
            {
                Animator animator = textToRun.GetComponent<Animator>();
                if (animator == null)
                {
                    Singleton.instance.GetComponent<ExceptionManager>().SendMissingComponentMessage("animator", this.GetType().ToString(), name);
                    return;
                }
                animator.SetBool("Pop", true);
            }
        }

        /// <summary>
        /// Disables the award canvas so it is invisible during game play.
        /// </summary>
        private void DisableCanvas()
        {
            _canvas.SetActive(false);
        }
    }
}