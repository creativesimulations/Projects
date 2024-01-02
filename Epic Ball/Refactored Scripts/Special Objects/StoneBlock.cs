using UnityEngine;

namespace EpicBall
{

    public class StoneBlock : Block, ITeleportable, IConsumeable
    {
        [Header("Setup parameters for this block.")]
        [Tooltip("If the rigid body on this block should be awake at start.")]
        [SerializeField] private bool _awakeOnStart;
        [Tooltip("Array of short audio clips for when the block bumps into a block that it can't consume.")]
        [SerializeField] private AudioClip[] _bumpClips;

        private void Awake()
        {
            _startAwake = _awakeOnStart;
            Rb = GetComponent<Rigidbody>();
            audioSource = GetComponentInChildren<AudioSource>();
        }
        private void OnCollisionEnter(Collision other)
        {
            CheckHit(other);
        }

        /// <summary>
        /// Plays and audio clip if the velocity of the collision is above the rigid body sleep threshhold and the colliding block hasn't played an audio clip for this collision.
        /// </summary>
        /// <param name="other"></param> The collision of the other block.
        private void CheckHit(Collision other)
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
}
