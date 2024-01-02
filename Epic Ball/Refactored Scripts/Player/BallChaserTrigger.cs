using UnityEngine;

namespace EpicBall
{
    public class BallChaserTrigger : MonoBehaviour
    {
        /// <summary>
        /// Finds any objects entering the trigger collider with a movement interface and activates them.
        /// </summary>
        /// <param name="collider"></param> The collider entering the trigger collider.
        private void OnTriggerEnter(Collider collider)
        {
            if (GameManager._gameStates == GameManager.GameStates.Play)
            {
                IMove chaser = collider.gameObject.GetComponent<IMove>();
                if (chaser != null)
                {
                    chaser.Chase(gameObject.transform.parent.gameObject);
                }
            }
        }

        /// <summary>
        /// Finds any objects leaving the trigger collider with a movement interface and deactivates them.
        /// </summary>
        /// <param name="collider"></param> the collider leaving the trigger collider.
        private void OnTriggerExit(Collider collider)
        {
            IMove chaser = collider.gameObject.GetComponent<IMove>();
            if (chaser != null)
            {
                chaser.StopChasing();
            }
        }
    }
}