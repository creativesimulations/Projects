using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallChaserTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.GetComponent<Chaser>())
        {
            Chaser chaser = collider.gameObject.GetComponent<Chaser>();
            if (!chaser.isAlwaysActive)
            {
                chaser.ActivateChaser();
            }
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.GetComponent<Chaser>())
        {
            Chaser chaser = collider.gameObject.GetComponent<Chaser>();
            if (!chaser.isAlwaysActive)
            {
                chaser.DeactivateChaser();
            }
        }
    }
}
