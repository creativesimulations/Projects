using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Blocks
{
    

public class Destroyer : MonoBehaviour
{
        
        ParticleHolder particleHolder;

        private void Start()
        {
            particleHolder = FindObjectOfType<ParticleHolder>();
        }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Neutral") || other.gameObject.CompareTag("Explode"))
            {
                //  ParticleSystem newdestroyParticle = Instantiate(destroyParticle, other.contacts[0].point, Quaternion.identity) as ParticleSystem;
                //   newdestroyParticle.transform.localScale = other.gameObject.transform.localScale;
                if (gameObject.CompareTag("Destroyer"))
                {
                particleHolder.playParticle(0, other.contacts[0].point, other.gameObject.transform.lossyScale);
                }
                else
                {
                    particleHolder.playParticle(5, other.contacts[0].point, other.gameObject.transform.lossyScale);
                }
                Destroy (other.gameObject);
        }
    }
}
}