using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;

public class ProjectileExploder : MonoBehaviour
{
    public float explosionRadius = 2f;
    public float explosionUpwardsForce = 1f;
    public int explosionDamage = 100;
    public float explosionForce = 70f;
    public float explosionDamageFalloff = 1f;
    private float torqueX;
    private float torqueY;
    private float torqueZ;
    public bool landed;
    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        PlayExplosionParticle();
        var rigidbodies = new List<Rigidbody>();
        Collider[] objectsInRange = Physics.OverlapSphere(transform.position, explosionRadius);

        // for every object within the explosion radius with a rididbody component, add them to the rigidbody list
        foreach (Collider colliderToAffect in objectsInRange)
        {
            if (colliderToAffect.attachedRigidbody != null && !rigidbodies.Contains(colliderToAffect.attachedRigidbody))
            {
                if (colliderToAffect.gameObject.tag == "Explodable" || colliderToAffect.gameObject.tag == "Vehicle" || colliderToAffect.gameObject.tag == "Block")
                {
                    rigidbodies.Add(colliderToAffect.attachedRigidbody);
                    if (colliderToAffect.gameObject.tag == "Vehicle")
                    {
                        colliderToAffect.gameObject.GetComponent<VehicleWheelsTorque>().DettachSpline();
                    }
                }
            }

            //// if any object has a target script attached, run the take damage function, with a fall-off effect, i.e. the further away the target is from the explosion origin, the less damage they take
            //Target target = collision.GetComponent<Target>();
            //if (target != null)
            //{
            //    float proximity = (transform.position - target.transform.position).magnitude;
            //    float effect = explosionDamageFalloff - (proximity / explosionRadius);
            //    // if the target is very close to the explosion origin, take full damage
            //    if (proximity <= 0.7f)
            //    {
            //        target.takeDamage(explosionDamage);
            //    }
            //    // else, take splash damage
            //    else
            //    {
            //        target.takeDamage(explosionDamage * effect);
            //    }
            //}
        }

        // add explosion force to each rigidbody
        foreach (var rb in rigidbodies)
        {
            rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, explosionUpwardsForce, ForceMode.Impulse);
            rb.angularVelocity = Random.insideUnitSphere * 3;
            torqueX = Random.Range(-15.0f, 15.0f);
            torqueY = Random.Range(-15.0f, 15.0f);
            torqueZ = Random.Range(-15.0f, 15.0f);
            Vector3 newTorque = new Vector3(torqueX, torqueY, torqueZ);
            rb.AddTorque(newTorque);
        }
    }

    private void PlayExplosionParticle()
    {
        if(!landed)
        {
            GameObject particle = EventManager.SpawnObject(EventManager.objectPooler.explosionParticle, false, EventManager.objectPooler.explosionParticleParent);
        particle.transform.position = transform.position;
        StartCoroutine(EventManager.DelayDespawnObject(particle, null, 1.5f));
            landed = true;
            rb.collisionDetectionMode = CollisionDetectionMode.Discrete;
        }
    }
}
