using Core;
using UnityEngine;

namespace EpicBall
{

    public class DestroyerBlock : Block, ITeleportable
    {

        [Header("Setup parameters for this block.")]
        [Tooltip("If the rigid body on this block should be awake at start.")]
        [SerializeField] private bool _awakeOnStart;
        [Tooltip("Array of short audio clips for when the block bumps into a block that it can't consume.")]
        [SerializeField] private AudioClip[] _bumpClips;
        [Tooltip("A short audio clip for when the block consumes another block.")]
        [SerializeField] private AudioClip _specialActionClip;
        [Tooltip("The particle effect to be played when the block consumes another.")]
        [SerializeField] private GameObject _destroyParticle;

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
            CheckDestroy(other);
        }

        /// <summary>
        /// Checks if the other object that collided with this object is consumable. If so, it will consume it and play the related particle effect and audio clip.
        /// If the other object is the player, it will destroy the player.
        /// If not, it will play a bump audio clip if the collision velocity is above the threshold and the other object hasn't already played one upon this collision.
        /// </summary>
        /// <param name="other"></param> The collision of the other game object.
        private void CheckDestroy(Collision other)
        {
            if (other.gameObject.CompareTag(GlobalConstants.PLAYER))
            {
                if (GameManager._gameStates != GameManager.GameStates.CompleteLvl)
                {
                    GameManager.SetGameState(GameManager.GameStates.Dead);
                }
            }
            if (GetClipPlayed())
            {
                SetClipPlayed(false);
            }
            var consumable = other.gameObject.GetComponent<IConsumeable>();
            if (consumable == null)
            {
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
            else
            {
                audioSource.volume = .5f;
                PlaySpecialActionClip(_specialActionClip);
                PlayParticle(other.contacts[0].point, other.gameObject.transform.localScale);
                consumable.Consume();
            }
        }

        /// <summary>
        /// Sets the parameters of the particle effect to be played when this block consumes another and then activates it.
        /// </summary>
        /// <param name="pointToSpawn"></param> The point of contact where the particle effect should appear.
        /// <param name="size"></param> The size of the block that will determine the size of the particle effect.
        private void PlayParticle(Vector3 pointToSpawn, Vector3 size)
        {
            _particle = objectPooler.PopFromPool(objectPooler.destroyerParticle, false, true, objectPooler.destroyerParticleParent, false);
            _particle.transform.position = pointToSpawn;
            _particle.transform.localScale = size;
            _particle.SetActive(true);
        }
    }
}