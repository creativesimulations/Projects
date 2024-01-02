using UnityEngine;
using UnityEngine.SceneManagement;

namespace EpicBall
{
    public class BonusLevesScreenController : MonoBehaviour
    {

        /// <summary>
        /// If no hard levels are avaialble an additive scene is loaded with a message indicating how to access the levels. This method unloads the additive scene on clicking the 'back' button.
        /// </summary>
        public void BackOnClick()
        {
            SceneManager.UnloadSceneAsync(GlobalConstants.HARD_LEVEL_NOTICE);
        }
    }
}