using System.Threading.Tasks;
using UnityEngine;
using System.Collections;

namespace Furry
{
public class PlayerDetector : MonoBehaviour
{
        public Player PlayerDetected { get; private set; }
        public bool IsPlayerLeaving;
        public bool PlayerInAttackRange;

        public void SetTriggerRaidus(float radius)
        {
            GetComponent<CapsuleCollider>().radius = radius;
        }
        private void OnTriggerEnter(Collider other)
        {
        IsPlayerLeaving = false;
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
                StartCoroutine(PlayerLeaving());
            }
        }

        public void CheckAttackRadius(float attackRange)
        {
            if (attackRange >= Vector3.Distance(this.transform.position, PlayerDetected.transform.position))
            {
                PlayerInAttackRange = true;
            }
        }
        private IEnumerator PlayerLeaving()
        {
            IsPlayerLeaving = true;
            yield return new WaitForSeconds(3);
            if (IsPlayerLeaving)
            {
                PlayerDetected = null;
            }
        }
        private void ChangeTargets()
        {
            // Use a stack? To check if another player is in the area after the first one left. If so, change to that target before trying to clear
        }

    }
}
