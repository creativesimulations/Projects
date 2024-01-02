using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Blocks {

    public class Hole : MonoBehaviour {

        [SerializeField] private float changePositionEvery = 4f;
        [SerializeField] private float holeSize = 6f;
        [SerializeField] private float movementRadius = 5f;
        [SerializeField] private float movementSpeed = 5f;

        private Vector3 destination;
        private Vector3 parentPosition;
        private Vector3 rendererBoundsSize;
        private GameObject player;
        private SphereCollider playerSphere;
        private AudioClip constantClip;
        private AudioClip fallClip;
        private float solidifyAlt;
        AudioSource audioSource;


        private void Awake()
        {
            SetSizeOfHole();
            AudioSource[] audioSources = GetComponents<AudioSource> ();
            audioSource = audioSources[0];
            constantClip = audioSources[0].clip;
            fallClip = audioSources[1].clip;
        }

        void Start ()
        {
            player = GameObject.FindWithTag("Player");
            playerSphere = player.GetComponent<SphereCollider>();
            InvokeRepeating ("Move", .1f, changePositionEvery);
        }

        private void FixedUpdate ()
        {
            float speed = movementSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards (transform.position, destination, speed);
        }

        void OnTriggerEnter (Collider other)
        {
            if (other.transform.localScale.x <= holeSize && other.transform.localScale.y <= holeSize && other.transform.localScale.z <= holeSize) {
                bool fits = true;
                if (other != playerSphere && fits && other.attachedRigidbody && !other.gameObject.CompareTag ("Ignore") && !other.gameObject.CompareTag ("Untagged") && !other.gameObject.CompareTag ("CameraTarget")) {
                    
                    audioSource.PlayOneShot (fallClip);
                    StartCoroutine(FallThroughHole(other));
                }
            }
        }

         private IEnumerator FallThroughHole(Collider other)
         {
            if (other.gameObject == player)
            {
                playerSphere.enabled = false;
                rendererBoundsSize = player.GetComponent<Renderer>().bounds.size;
                solidifyAlt = other.transform.position.y - rendererBoundsSize.y;
            }
            else
            {
                other.enabled = false;
                rendererBoundsSize = other.gameObject.GetComponent<Renderer>().bounds.size;
                solidifyAlt = other.transform.position.y - rendererBoundsSize.y;
            }
             while (other.gameObject.transform.position.y > solidifyAlt)
             {
             yield return new WaitForEndOfFrame();
            }
            if (other.gameObject == player)
            {
                playerSphere.enabled = true;
            }
            else
            {
            other.enabled = true;
            }
         }

        private void SetSizeOfHole()
        {
            parentPosition = transform.parent.position;
            Transform childTransform = gameObject.transform.GetChild(0);
            childTransform.gameObject.transform.localScale = new Vector3(holeSize, 0.1f, holeSize);
            gameObject.transform.localScale = new Vector3(holeSize, 0.1f, holeSize);
        }

        private void Move () {
            destination = new Vector3(Random.Range(parentPosition.x + movementRadius / 2, parentPosition.x - movementRadius / 2), parentPosition.y, Random.Range(parentPosition.z + movementRadius / 2, parentPosition.z - movementRadius / 2));
        }

        void OnDrawGizmosSelected () {
            float outsideOfHole = movementRadius + holeSize / 2;
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere (transform.position, movementRadius);
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, holeSize);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, outsideOfHole);
        }

    }
}