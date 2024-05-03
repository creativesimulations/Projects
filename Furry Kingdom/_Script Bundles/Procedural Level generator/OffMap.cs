using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Furry
{

    [RequireComponent(typeof(BoxCollider))]
public class OffMap : MonoBehaviour
{
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out MovementRigidBody player))
            {
                Vector3 teleportLocation = Utilities.TestNewLocation(other.transform.position, 100);
                StartCoroutine(TeleportToMap(player, teleportLocation));
            }
        }
        private IEnumerator TeleportToMap(MovementRigidBody player, Vector3 location)
        {
            Debug.Log("Teleport called");
            yield return new WaitForSeconds(3);
            player.ResetLocation(location);
        }
    }
}