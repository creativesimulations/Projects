using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chaser : MonoBehaviour
{

    GameObject player;
    bool isSphere;
    [SerializeField] public bool isAlwaysActive = false;
    SphereMovement sphereMovement;
    CylinderMovement cylinderMovement;
    Ball ball;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag ("Player");
        ball = player.GetComponent<Ball> ();
        if (GetComponent<SphereMovement> () != null)
        {
            isSphere = true;
            sphereMovement = GetComponent<SphereMovement> ();
            if (isAlwaysActive)
            {
                StartCoroutine (sphereMovement.ChasePlayer (ball));
            }
        }
        else
        {
            isSphere = false;
            cylinderMovement = GetComponent<CylinderMovement> ();
            if (isAlwaysActive)
            {
                StartCoroutine (cylinderMovement.LocateSide (ball));
            }
        }
    }

    public void ActivateChaser()
    {
        if (isSphere)
        {
            if (!sphereMovement.activate)
            {
                sphereMovement.activate = true;
            StartCoroutine (sphereMovement.ChasePlayer (ball));
            }
        }
        else if (!cylinderMovement.activate)
        {
            cylinderMovement.activate = true;
            StartCoroutine (cylinderMovement.LocateSide (ball));
        }
    }

    public void DeactivateChaser()
    {
        if (isSphere)
        {
            sphereMovement.activate = false;
        }
        else
        {
            cylinderMovement.activate = false;
        }
    }
    
    

}
