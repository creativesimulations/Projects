using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace EpicBall
{
    public class GemCounterUI : MonoBehaviour
    {

        public static event Action CollectedAllGems;

        [Tooltip("The text field where the 'gems collected' message will be displayed.")]
        [SerializeField] private TextMeshProUGUI _text;

        private int _gemsCollected;
        private int _gemsInLevel;
        private Animator _animator;
        private List<GameObject> _listOfGemsInLevel = new List<GameObject>();
        private CanvasGroup _canvasGroup;


        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _text = GetComponent<TextMeshProUGUI>();
            _animator = GetComponent<Animator>();
            if (_text == null)
            {
                Singleton.instance.GetComponent<ExceptionManager>().SendMissingObjectMessage("_text", this.GetType().ToString(), name);
                return;
            }
            if (_animator == null)
            {
                Singleton.instance.GetComponent<ExceptionManager>().SendMissingComponentMessage("_animator", this.GetType().ToString(), name);
                return;
            }
        }

        private void Start()
        {
            Gem.gemCollected += CollectGem;
            GameManager.CompleteLvl += HideCanvas;
            GameManager.CompleteLvl += ClearList;
            GameManager.PlayGame += InitializeGems;
            GameManager.MainMenu += HideCanvas;
            GameManager.Die += ClearList;
            PauseSceneManager.OnRestartLvl += ClearList;
            GameManager.Pause += DecreaseAlpha;
        }

        /// <summary>
        /// Find all of the purple gems in the level and put them in a list. Also sets the gems collected amount to 0.
        /// </summary>
        private void InitializeGems()
        {
            if (_listOfGemsInLevel.Count == 0)
            {
                _listOfGemsInLevel.AddRange(GameObject.FindGameObjectsWithTag("Gem"));
                _gemsInLevel = _listOfGemsInLevel.Count;
                if (_listOfGemsInLevel.Count == 0)
                {
                    Singleton.instance.GetComponent<ExceptionManager>().SendEmptyContainerMessage("_listOfGemsInLevel", this.GetType().ToString(), name);
                    return;
                }
            }
                DisplayGemsCollected();
                IncreaseAlpha();
        }

        /// <summary>
        /// Clears the current list of gems.
        /// </summary>
        private void ClearList()
        {
            _listOfGemsInLevel.Clear();
            _gemsCollected = 0;
        }

        /// <summary>
        /// Displays how many gems have been collected and activates the text animation.
        /// </summary>
        private void DisplayGemsCollected()
        {
            _animator.SetTrigger("collect");
            _text.text = _gemsCollected + " / " + _gemsInLevel + " Gems";
        }

        /// <summary>
        /// Increases the gems collected amount by 1 and runs the text display method. If all of the gems have been collected a notification is sent out. 
        /// </summary>
        /// <param name="isGoldGem"></param>
        private void CollectGem(bool isGoldGem)
        {
            if (!isGoldGem)
            {
                _gemsCollected++;
                DisplayGemsCollected();
                if (_gemsCollected >= _gemsInLevel)
                {
                    CollectedAllGems?.Invoke();
                }
            }
            else
            {
                return;
            }
        }

        /// <summary>
        /// Hides the Gem counter canvas.
        /// </summary>
        private void HideCanvas()
        {
            ClearList();
            DecreaseAlpha();
        }

        /// <summary>
        /// Hides the canvas that displays how many gems have been collected.
        /// </summary>
        private void DecreaseAlpha()
        {
            if (_canvasGroup == null)
            {
                Singleton.instance.GetComponent<ExceptionManager>().SendMissingComponentMessage("_canvasGroup", this.GetType().ToString(), name);
                return;
            }
            _canvasGroup.alpha = 0;
        }
        private void IncreaseAlpha()
        {
            if (_canvasGroup == null)
            {
                Singleton.instance.GetComponent<ExceptionManager>().SendMissingComponentMessage("_canvasGroup", this.GetType().ToString(), name);
                return;
            }
            _canvasGroup.alpha = 1;
        }

        private void OnDisable()
        {
            Gem.gemCollected -= CollectGem;
            GameManager.CompleteLvl -= DecreaseAlpha;
            GameManager.PlayGame -= InitializeGems;
            GameManager.MainMenu -= HideCanvas;
            GameManager.CompleteLvl += ClearList;
            PauseSceneManager.OnRestartLvl -= ClearList;
            GameManager.Pause -= DecreaseAlpha;
        }
    }
}