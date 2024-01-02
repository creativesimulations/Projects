using UnityEngine;

namespace EpicBall
{

    [CreateAssetMenu(fileName = "TutorialPopup", menuName = "EpicBallScriptables/TutorialPopup")]
    public class TutorialPopupScriptable : ScriptableObject
    {
        [Tooltip("The text to be displayed when this tutorial is activated.")]
        [TextArea(5, 10)]
        [SerializeField] public string _popupText;

        public bool displayed;
    }
}
