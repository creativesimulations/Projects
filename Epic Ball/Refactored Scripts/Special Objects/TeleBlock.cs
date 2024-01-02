using UnityEngine;
using Core;

namespace EpicBall
{
    public class TeleBlock : Block, IConsumeable
    {
        [Header("Setup parameters for this block.")]
        [Tooltip("If the rigid body on this block should be awake at start.")]
        [SerializeField] private bool _awakeOnStart;
        [Tooltip("Array of short audio clips for when the block bumps into a block that it can't consume.")]
        [SerializeField] private AudioClip[] _bumpClips;
        [Tooltip("A short audio clip for when the block consumes another block.")]
        [SerializeField] private AudioClip _specialActionClip;

        private GameObject _particle;

        private void Awake()
        {
            _startAwake = _awakeOnStart;
            Rb = GetComponent<Rigidbody>();
            audioSource = GetComponentInChildren<AudioSource>();
            objectPooler = Singleton.instance.GetComponent<ObjectPooler>();
        }
        private void OnCollisionEnter(Collision other)
        {
            CheckTeleport(other);
        }

        /// <summary>
        /// Checks if the other game object is the player and if so, teleports them.
        /// If not, it will play a bump audio clip if the collision velocity is above the threshold and the other object hasn't already played one upon this collision.
        /// </summary>
        /// <param name="other"></param> The collision of the other game object.
        private void CheckTeleport(Collision other)
        {
            if (other.gameObject.CompareTag(GlobalConstants.PLAYER))
            {
                var tele = other.gameObject.GetComponent<ITeleportable>();
                tele.Teleport();
                PlayParticle();
            }
            else
            {
                if (GetClipPlayed())
                {
                    SetClipPlayed(false);
                }
                if (other.relativeVelocity.magnitude > GetThresholdToPlayBump())
                {
                    var bump = other.gameObject.GetComponent<IBump>();
                    if (bump == null)
                    {
                        audioSource.volume = other.relativeVelocity.magnitude / 10;
                        PlayBumpClip(_bumpClips);
                    }
                    else if (!bump.GetClipPlayed())
                    {
                        audioSource.volume = other.relativeVelocity.magnitude / 10;
                        PlayBumpClip(_bumpClips);
                        SetClipPlayed(true);
                    }
                }
            }
        }

        /// <summary>
        /// Plays the relevant particle effect.
        /// </summary>
        public void PlayParticle()
        {
            _particle = objectPooler.PopFromPool(objectPooler.teleportParticle, false, true, objectPooler.teleportParticleParent, false);
            _particle.transform.position = transform.position;
            _particle.SetActive(true);
        }

    }
}
