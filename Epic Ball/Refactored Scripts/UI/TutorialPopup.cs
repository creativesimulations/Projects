using System;
using UnityEngine;

namespace EpicBall
{
    public class TutorialPopup : MonoBehaviour
    {
        public static event Action<string> OnTutEnter;

        [Header("This script sends the relevant text to the tutorial controller to be displayed to the player.")]
        [Tooltip("The tutorial scriptable object with the desired text for the tutorial.")]
        [SerializeField] private TutorialPopupScriptable _tutScriptable;

        void Start()
        {
            CheckRelevance();
        }

        /// <summary>
        /// Will deactivate the tutorial if it has previously been displayed.
        /// </summary>
        private void CheckRelevance()
        {
            if (_tutScriptable == null)
            {
                Singleton.instance.GetComponent<ExceptionManager>().SendMissingObjectMessage("_tutScriptable", this.GetType().ToString(), name);
            }
            if (_tutScriptable.displayed)
            {
                gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Notifies the tutorial controller to activate the tutorial scene and display the text on the scriptable object referenced here. The layers are set so only the player can active this method.
        /// </summary>
        private void OnTriggerEnter()
        {
            if (_tutScriptable != null)
            {
                OnTutEnter?.Invoke(_tutScriptable._popupText);
                _tutScriptable.displayed = true;
            }
            Destroy(gameObject);
        }


    }
}