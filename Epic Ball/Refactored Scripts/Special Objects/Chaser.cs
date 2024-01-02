using UnityEngine;

namespace EpicBall
{

    public class Chaser : MonoBehaviour, IMove
    {
        protected bool active;

        void Awake()
        {
            GameManager.Die += StopChasing;
            GameManager.Win += StopChasing;
        }

        /// <summary>
        /// Begin chasing.
        /// </summary>
        /// <param name="player"></param>
        public virtual void Chase(GameObject player)
        {
        }

        /// <summary>
        /// Deactivate this block to prevent it from chasing.
        /// </summary>
        public virtual void StopChasing()
        {
            active = false;
            StopAllCoroutines();
        }

        private void OnDisable()
        {
            GameManager.Die -= StopChasing;
            GameManager.Win -= StopChasing;
        }
    }
}