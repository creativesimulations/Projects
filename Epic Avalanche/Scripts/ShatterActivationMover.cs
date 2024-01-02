using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShatterActivationMover : MonoBehaviour
{
    [SerializeField] private Transform origin;
    [SerializeField] private Transform destination;
    [SerializeField] private float speed = 1;

    private float t;
    private Vector3 currentPos;

    void Update()
    { 
        t += Time.deltaTime * speed;
         transform.position = Vector3.Lerp(origin.position, destination.position, t);
         currentPos = transform.position;
         if (currentPos == destination.position)
         {
            this.gameObject.SetActive(false);
         }
    }

}
