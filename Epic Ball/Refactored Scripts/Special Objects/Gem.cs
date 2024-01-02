using Core;
using System;
using UnityEngine;

namespace EpicBall
{
    public class Gem : MonoBehaviour
    {

        public static event Action<bool> gemCollected;

        [Tooltip("The scriptable object with the parameters for this gem.")]
        [SerializeField] private GemScriptable _gemScriptable;
        private AudioSource _audioSource;
        private ObjectPooler _objectPooler;
        private GameObject _particle;


        private void Start()
        {
            _audioSource = GetComponentInChildren<AudioSource>();
            _objectPooler = Singleton.instance.GetComponent<ObjectPooler>();
        }

        /// <summary>
        /// Plays the audio clip and particles for collecting a gem, send a notification on the type of gem collected and deletes the gem object.
        /// </summary>
        public void Collect()
        {
            PlaySound();

            if (!_gemScriptable._isGoldGem)
            {
                PlayParticle();
                gemCollected?.Invoke(false);
            }
            else
            {
                Instantiate(_gemScriptable._particleSystem, transform.position, transform.rotation);
                gemCollected?.Invoke(true);
            }
            Destroy(gameObject);
        }

        /// <summary>
        /// sets the gem particle position and activates it.
        /// </summary>
        public void PlayParticle()
        {
            _particle = _objectPooler.PopFromPool(_gemScriptable._particleSystem, false, true, _objectPooler.gemParticleParent, false);
            _particle.transform.position = transform.position;
            _particle.SetActive(true);
        }

        /// <summary>
        /// Plays the audio clip after the gem is destroyed.
        /// </summary>
        public void PlaySound()
        {
            AudioSourceExtensions.PlayAfterDestroy(_audioSource, _gemScriptable._audioClip);
        }

        /// <summary>
        /// Collects the gem if the player enters the trigger collider on the gem.
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag(GlobalConstants.PLAYER))
            {
                Collect();
            }
        }
    }
}
