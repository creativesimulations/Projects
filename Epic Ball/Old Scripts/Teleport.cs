using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{

    [SerializeField] GameObject teleTo = null;
    AudioSource audioSource;
  //  [SerializeField] private ParticleSystem teleParticle;
    Ball ball;
    ParticleHolder particleHolder;

    private void Awake() {
    audioSource = teleTo.GetComponent<AudioSource>();
}

    private void Start()
    {
        ball = FindObjectOfType<Ball> ();
        particleHolder = FindObjectOfType<ParticleHolder>();
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag != "Hole" && collider.tag != "CameraTarget" && collider.tag != "Untagged" && !collider.GetComponent<BumpSounds>().isGiant)
        {
            collider.gameObject.transform.position = teleTo.transform.position;
            audioSource.PlayOneShot(audioSource.clip);
            // ParticleSystem newTeleParticle = Instantiate(teleParticle, collider.gameObject.transform.position, Quaternion.identity) as ParticleSystem;
            particleHolder.playParticle(2, collider.gameObject.transform.position, transform.lossyScale);
        }
        if (collider.gameObject.tag == "Player")
        {
                StartCoroutine(HeightAfterTele());
        }
    }
IEnumerator HeightAfterTele()
    {
            yield return new WaitForSeconds(0.5f);
        ball.DetectPlanes ();
    }
}


