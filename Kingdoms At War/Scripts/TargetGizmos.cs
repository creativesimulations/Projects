using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetGizmos : MonoBehaviour
{
    [SerializeField] private bool DrawRadiusGizmo;
    [SerializeField] private float radius = 10f;

void OnDrawGizmos()
    {

#if UNITY_EDITOR
        Gizmos.color = Color.red;

        //Draw the suspension
        Gizmos.DrawLine(
            Vector3.zero,
            Vector3.up
        );

        if (DrawRadiusGizmo)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(transform.position, radius);
        }

        // Draw attack sphere
        // Gizmos.color = new Color(255f, 0f, .5f);
        // Gizmos.DrawWireSphere(transform.position, walkRadius);
        // Draw chase sphere
        // Gizmos.color = new Color(175f, 255f, .5f);
        // Gizmos.DrawWireSphere(transform.position, runRadius);
        // Draw attention sphere
        //Gizmos.color = new Color(150f, 100f, .5f);
        //Gizmos.DrawWireSphere(transform.position, triggerRadius);
        // Draw scared sphere
#endif
    }

    private void OnDrawGizmosSelected() {
        
    }
}
