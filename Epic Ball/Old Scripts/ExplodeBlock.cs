using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeBlock : MonoBehaviour
{
    [SerializeField] private int health = 4;
    [SerializeField] private float radius = 5.0F;
    [SerializeField] private float power = 400.0F;
    [SerializeField] private float pushUp = 3.0F;
  //  [SerializeField] private ParticleSystem explodeParticle;
    private bool exploded;

    ParticleHolder particleHolder;
    BumpSounds bumpSounds;

private void Start()
    {
        particleHolder = FindObjectOfType<ParticleHolder>();
        bumpSounds = GetComponent<BumpSounds>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.relativeVelocity.magnitude > health)
        {
            RunExplosion(power);
        }
    }

    public void RunExplosion(float explodeAmount)
    {
        if (gameObject.CompareTag("Glass") || gameObject.CompareTag("Explode"))
        {
            AudioSourceExtensions.PlayAfterDestroy(bumpSounds.childAudioSource);
            if (gameObject.CompareTag("Glass"))
            {
                //  ParticleSystem newExplodeParticle = Instantiate(explodeParticle, transform.position, Quaternion.identity) as ParticleSystem;
                particleHolder.playParticle(4, transform.position, transform.localScale);
              //  newExplodeParticle.transform.localScale = transform.localScale;
            }

            if (gameObject.CompareTag("Explode"))
            {
                // ParticleSystem newExplodeParticle = Instantiate(explodeParticle, transform.position, Quaternion.identity) as ParticleSystem;
                particleHolder.playParticle(1, transform.position, transform.localScale);
                ExplodeEffect(explodeAmount);
            }
        }
        Destroy(gameObject);
    }

    public void ExplodeEffect(float explodeForce)
    {
        Exploded = true;
        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
        foreach (Collider hit in colliders)
        {   
            GameObject hitO = hit.gameObject;
            if (!hitO.CompareTag("Untagged") && !hitO.CompareTag("Hole") && !hitO.CompareTag("Teleport") && !hitO.CompareTag("Goal"))
            {
                Rigidbody rb = hit.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddExplosionForce(explodeForce, explosionPos, radius);
                    if (hitO.GetComponent<ExplodeBlock>())
                    {
                        ExplodeBlock eB = hitO.GetComponent<ExplodeBlock>();
                        if (!eB.Exploded)
                        eB.RunExplosion(explodeForce / 6);;
                    }
                }
            }
        }
    }

    public bool Exploded
    {
        get { return exploded;}
        set {exploded = value;}
    }

}
