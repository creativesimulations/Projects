using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collect : MonoBehaviour
{

   // [SerializeField] private ParticleSystem collectParticle;
    AudioSource audioSource;
    ParticleHolder particleHolder;
    private bool collected;

    private void Awake()
    {
        audioSource = GetComponentInChildren<AudioSource>();
    }

    private void Start()
    {
        particleHolder = FindObjectOfType<ParticleHolder>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !collected)
        {
            collected = true;
            if (gameObject.tag != "GoldGem")
            {
                other.gameObject.GetComponent<Ball> ().CollectGem();
            }
            else
            {
                other.gameObject.GetComponent<Ball> ().goldGemCollected = true;
            }
            AudioSourceExtensions.PlayAfterDestroy(audioSource);
            // ParticleSystem newCollectParticle = Instantiate(collectParticle, transform.position, Quaternion.identity) as ParticleSystem;
            particleHolder.playParticle(3, transform.position, transform.lossyScale);
            Destroy(transform.gameObject);
        }
    }
}
