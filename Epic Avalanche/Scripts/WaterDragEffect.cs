using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;

public class WaterDragEffect : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        GameObject particle = EventManager.SpawnObject(EventManager.objectPooler.splashParticle, false, EventManager.objectPooler.splashParticleParent);
        particle.transform.position = other.gameObject.transform.position;
        StartCoroutine(EventManager.DelayDespawnObject(particle, null, 1.5f));
    }
    
}
