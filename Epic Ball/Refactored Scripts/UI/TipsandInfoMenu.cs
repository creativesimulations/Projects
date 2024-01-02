using UnityEngine;
using UnityEngine.SceneManagement;

namespace EpicBall
{
    public class TipsandInfoMenu : MonoBehaviour
    {

        /// <summary>
        /// Unloads the 'Tips' scene on clicking 'back'.
        /// </summary>
        public void BackOnClick()
        {
            SceneManager.UnloadSceneAsync(GlobalConstants.TIPS_MENU);
        }

    }
}