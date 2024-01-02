using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RayFire;
using Core;
using Scriptables;

public class BallExplode : MonoBehaviour
{

    [Header("Put a Projectile Scriptable Object here")]
    private float torqueX;
    private float torqueY;
    private float torqueZ;
    private bool exploded;

    [Tooltip("Put parent's Weapon Scriptable Object here.")]
    public WeaponScriptableObject WSO;
    Rigidbody rb;
    public int team;
    private RayfireActivator Rfa;
    [SerializeField] private AudioClip[] explosionclips;
    [SerializeField] private AudioClip[] rockClips;
    private AudioSource audioSource;

    private List<Rigidbody> rigidbodies = new List<Rigidbody>();


    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        Rfa = GetComponentInParent<RayfireActivator>();

        Invoke("SetActiveRadius", 1);
    }

    private void SetActiveRadius()
    {
        Rfa.sphereRadius = WSO.explosionRadius;
        Rfa.ResizeCollider();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!exploded)
        {
        Debug.Log(gameObject.name + " Collided with = " + collision.gameObject.name);
            exploded = true;
            PlayExplosionParticle();
            PlayAudioClip();

            Collider[] objectsInRange = Physics.OverlapSphere(transform.position, WSO.explosionRadius);
            Rfa.enabled = false;

            // for every object within the explosion radius with a rididbody component, add them to the rigidbody list
            foreach (Collider colliderToAffect in objectsInRange)
            {
                if (colliderToAffect.gameObject.tag == "UpgradeBlock")
                {
                    colliderToAffect.gameObject.GetComponent<UpgradeBlockWithScriptable>().OnHit(team);
                }
                else if (colliderToAffect.attachedRigidbody != null && !rigidbodies.Contains(colliderToAffect.attachedRigidbody))
                {
                    if (colliderToAffect.gameObject.layer == 11)
                    {
                        rigidbodies.Add(colliderToAffect.attachedRigidbody);
                    }
                }
                if (colliderToAffect.gameObject.tag == "Weapon")
                {
                    colliderToAffect.gameObject.GetComponent<Cannon>().OnHit(team);
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
                // }

                // add explosion force to each rigidbody
                VelocityToRigidBodies();
            }
        }
    }

    private void VelocityToRigidBodies()
    {
        if (rigidbodies != null)
        {

            foreach (var rb in rigidbodies)
            {
                if (rb.gameObject.tag == "MiscObjects")
                {
                    rb.isKinematic = false;
                }
                rb.AddExplosionForce(WSO.explosionForce, transform.position, WSO.explosionRadius, WSO.explosionUpwardsForce, ForceMode.Impulse);
                rb.angularVelocity = Random.insideUnitSphere * 3;
                torqueX = Random.Range(-15.0f, 15.0f);
                torqueY = Random.Range(-15.0f, 15.0f);
                torqueZ = Random.Range(-15.0f, 15.0f);
                Vector3 newTorque = new Vector3(torqueX, torqueY, torqueZ);
                rb.AddTorque(newTorque);
            }
        }
    }

    private void PlayExplosionParticle()
    {
        GameObject particle = EventManager.objectPooler.PopFromPool(EventManager.objectPooler.cannonBallExplosionParticle.name, false, false, EventManager.objectPooler.cannonBallExplosionParticleParent);
        // GameObject particle = EventManager.SpawnObject(EventManager.objectPooler.cannonBallExplosionParticle, false, EventManager.objectPooler.cannonBallExplosionParticleParent);
        particle.transform.position = transform.position;
        StartCoroutine(EventManager.DelayDespawnObject(particle, null, 2.2f));
        rb.collisionDetectionMode = CollisionDetectionMode.Discrete;
    }

    private void PlayAudioClip()
    {
        if (explosionclips != null)
        {
            AudioClip clipToPlay1;
            int arrayIndex = Random.Range(0, explosionclips.Length);
            clipToPlay1 = explosionclips[arrayIndex];
            audioSource.PlayOneShot(clipToPlay1);
            Debug.Log("Playing " + clipToPlay1);
        }
        if (rockClips != null)
        {
            AudioClip clipToPlay2;
            int arrayIndex = Random.Range(0, rockClips.Length);
            clipToPlay2 = rockClips[arrayIndex];
            audioSource.PlayOneShot(clipToPlay2);
            Debug.Log("Playing " + clipToPlay2);
        }
    }

}
