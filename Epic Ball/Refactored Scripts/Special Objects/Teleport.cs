using Core;
using UnityEngine;
using UnityEngine.Serialization;

namespace EpicBall
{
    public class Teleport : MonoBehaviour
    {
        [Tooltip("The 'Teleport Exit' prefab where the objects will be teleported to.")]
        [FormerlySerializedAs("teleTo")]
        [SerializeField] private GameObject _teleTo = null;

        private AudioSource _audioSource;
        private ObjectPooler _objectPooler;
        private GameObject _particle;

        private void Awake()
        {
            _audioSource = _teleTo.GetComponent<AudioSource>();
        }

        private void Start()
        {
            _objectPooler = Singleton.instance.GetComponent<ObjectPooler>();
        }

        void OnTriggerEnter(Collider collider)
        {
            if (!collider.isTrigger)
            {
                CheckTeleportation(collider);
            }
        }

        /// <summary>
        /// Checks if the collider object is teleportable. If so, the teleport audio clip and particle is played and the object is teleported to the exit destination. 
        /// </summary>
        /// <param name="collider"></param> The object to be teleported.
        private void CheckTeleportation(Collider collider)
        {
            var objectToTeleport = collider.gameObject.GetComponent<ITeleportable>();
            if (objectToTeleport == null)
            {
                return;
            }
            _audioSource.PlayOneShot(_audioSource.clip);
            PlayParticle(_objectPooler.teleportParticle, _objectPooler.teleportParticleParent, transform.position);

            objectToTeleport.Teleport(_teleTo.transform.position);

            _teleTo.GetComponent<AudioSource>().PlayOneShot(_audioSource.clip);
            PlayParticle(_objectPooler.teleportParticle, _objectPooler.teleportParticleParent, _teleTo.transform.position);

        }

        /// <summary>
        /// Sets the teleport particle to be played and activates it.
        /// </summary>
        /// <param name="teleparticle"></param> The teleport particle to play.
        /// <param name="particleParent"></param> The teleport particle parent, where the particle is kept.
        /// <param name="position"></param> the position at which to play the particle.
        public void PlayParticle(GameObject teleparticle, Transform particleParent, Vector3 position)
        {
            _particle = _objectPooler.PopFromPool(teleparticle, false, true, particleParent, false);
            _particle.transform.position = position;
            _particle.SetActive(true);
        }
    }
}



