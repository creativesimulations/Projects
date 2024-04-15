using System.Threading.Tasks;
using UnityEngine;
using System.Collections;

namespace Furry
{
public class PlayerDetector : MonoBehaviour
{
        public Player PlayerDetected { get; private set; }
        public bool PlayerLeaving;
        public bool PlayerInAttackRange;

        public void SetTriggerRaidus(float radius)
        {
            GetComponent<CapsuleCollider>().radius = radius;
        }
        private void OnTriggerEnter(Collider other)
        {
        PlayerLeaving = false;
        // add to stack? if another player comes into range
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
            other.TryGetComponent<Player>(out Player player);
            if (PlayerDetected == player)
            {
                // check that no other players are in the area first.
                PlayerLeaving = true;
             //   StartCoroutine(ClearDetectedPlayerDelay());
            }
        }

        public void CheckAttackRadius(float attackRange)
        {
            if (attackRange >= Vector3.Distance(this.transform.position, PlayerDetected.transform.position))
            {
                PlayerInAttackRange = true;
            }
        }

        private void ChangeTargets()
        {
            // Use a stack? To check if another player is in the area after the first one left. If so, change to that target before trying to clear
        }

        public void PlayerLeft()
        {
            Debug.Log("Player Left");
            PlayerDetected = null;
            PlayerLeaving = false;
        }
    }
}
