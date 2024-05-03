using UnityEngine;
using System.Collections;

namespace Furry
{
    public class PlayerDetector : MonoBehaviour
    {
        [Header("Detector fields")]
        [Tooltip("Radius of the detector trigger collider.")]
        [SerializeField] private float _detectorRadius;
        [Tooltip("Delay after player leaves before nullifying it as a target.")]
        [SerializeField] private float _delayAfterPlayerLeft;

        public Player PlayerDetected { get; private set; }
        public bool IsPlayerLeaving;
        public bool PlayerInAttackRange;

        private void Awake()
        {
            SetTriggerRaidus(_detectorRadius);
        }

        /// <summary>
        /// Sets the radius of the trigger collider to detect players.
        /// </summary>
        /// <param name="radius"></param> The radius amount to set.
        public void SetTriggerRaidus(float radius)
        {
            if (radius > 0)
            {
                GetComponent<CapsuleCollider>().radius = radius;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            DetectPlayer(other);
        }

        /// <summary>
        /// If player is not already detected, set PlayerDetected as true. If the same player was leaving, stops that coroutine as well.
        /// </summary>
        /// <param name="other"></param> Object to check.
        private void DetectPlayer(Collider other)
        {
            // MUST EVENTUALLY IMPLEMENT A MECHANIC TO CHECK FOR OTHER PLAYERS THAT MIGHT BE IN THE AREA AS WELL ***

            IsPlayerLeaving = false;

            other.TryGetComponent<Player>(out Player player);
            if (PlayerDetected == null)
            {
                PlayerDetected = player;
            }
            else if (player == PlayerDetected)
            {
                StopAllCoroutines();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            NotDetectingPlayer(other);
        }

        /// <summary>
        /// If the player has left, set IsPlayerLeaving as true and begin PlayerLeaving coroutine.
        /// </summary>
        /// <param name="other"></param>
        private void NotDetectingPlayer(Collider other)
        {
            other.TryGetComponent<Player>(out Player player);
            if (PlayerDetected == player)
            {
                // check that no other players are in the area first.
                StartCoroutine(PlayerLeaving());
            }
        }

        /// <summary>
        /// Check if player is within attack range.
        /// </summary>
        /// <param name="attackRange"></param> Maximum attack range.
        public void CheckAttackRadius(float attackRange)
        {
            if (attackRange >= Vector3.Distance(this.transform.position, PlayerDetected.transform.position))
            {
                PlayerInAttackRange = true;
            }
        }

        /// <summary>
        /// Waits a desired amount of time before nullifying PlayerDetected.
        /// </summary>
        /// <returns></returns>
        private IEnumerator PlayerLeaving()
        {
            IsPlayerLeaving = true;
            yield return new WaitForSeconds(3);
            if (IsPlayerLeaving)
            {
                PlayerDetected = null;
            }
        }

        /// <summary>
        /// Changes the player target if current target leaves.
        /// </summary>
        private void ChangeTargets()
        {
            //  check if another player is in the area after the first one left. If so, change to that target before trying to clear ***
        }

    }
}
