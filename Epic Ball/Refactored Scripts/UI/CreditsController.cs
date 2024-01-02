using System.Collections;
using UnityEngine;

namespace EpicBall
{
    public class CreditsController : MonoBehaviour
    {

        /// <summary>
        /// restarts the game once the credits are finished. This method is run at the end of the credits animation which scrools the text.
        /// </summary>
        /// <returns></returns>
        public IEnumerator StartOver()
        {
            yield return new WaitForSeconds(.5f);
            GameManager.SetGameState(GameManager.GameStates.MainMenu);
            yield return new WaitForEndOfFrame();
        }
    }
}