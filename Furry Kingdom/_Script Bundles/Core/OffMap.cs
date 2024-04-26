using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Furry
{


public class OffMap : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }
        private void OnTriggerEnter(Collider other)
        {
            Player player;
                other.gameObject.TryGetComponent<Player>(out player);
            if (player != null)
            {
                Vector3 teleportLocation = Utilities.TestNewLocation(other.transform.position, 100);
                StartCoroutine(TeleportToMap(other.gameObject, teleportLocation));
            }
        }
        private IEnumerator TeleportToMap(GameObject player, Vector3 location)
        {
            yield return new WaitForSeconds(3);
            player.transform.position = location;
        }
    }
}