using System.Threading.Tasks;
using UnityEngine;
using System.Collections;

namespace Furry
{
public class PlayerDetector : MonoBehaviour
{
        [SerializeField] public Player PlayerDetected; // {  get; private set; }
        public bool PlayerInAttackRange;

        public void SetTriggerRaidus(float radius)
        {
            GetComponent<CapsuleCollider>().radius = radius;
        }
        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("other entered trigger");
            other.TryGetComponent<Player>(out Player player);
                if (PlayerDetected == null)
            {
                PlayerDetected = player;
            }
                else if (player == PlayerDetected)
            {
                Debug.Log("Stopping all coroutines");
                StopAllCoroutines();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            Debug.Log("other left trigger");
            other.TryGetComponent<Player>(out Player player);
            if (PlayerDetected == player)
            {
                StartCoroutine(ClearDetectedPlayerDelay());
            }
        }

        private IEnumerator ClearDetectedPlayerDelay()
        {
            Debug.Log("ClearDetectedPlayerDelay");
            yield return new WaitForSeconds(3);
            Debug.Log("Cleared");
            PlayerDetected = null;
        }

    }
}
