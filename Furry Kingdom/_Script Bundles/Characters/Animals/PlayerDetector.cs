using System.Threading.Tasks;
using UnityEngine;
using System.Collections;

namespace Furry
{
public class PlayerDetector : MonoBehaviour
{
        public Player PlayerDetected { get; private set; }
        public bool PlayerInAttackRange;

        public void SetTriggerRaidus(float radius)
        {
            GetComponent<CapsuleCollider>().radius = radius;
        }
        private void OnTriggerEnter(Collider other)
        {
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
                StartCoroutine(ClearDetectedPlayerDelay());
            }
        }

        public void CheckAttackRadius(float attackRange)
        {
            if (attackRange >= Vector3.Distance(this.transform.position, PlayerDetected.transform.position))
            {
                PlayerInAttackRange = true;
            }
        }

        private IEnumerator ClearDetectedPlayerDelay()
        {
            yield return new WaitForSeconds(3);
            PlayerDetected = null;
        }

    }
}
