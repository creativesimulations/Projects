using Core;
using EpicBall;
using System;
using UnityEngine;

public class GlassBlock : Block, ITeleportable
{
    public static event Action OnBreak;

    [Header("Setup parameters for this block.")]
    [Tooltip("If the rigid body on this block should be awake at start.")]
    [SerializeField] private bool _awakeOnStart;
    [Tooltip("The strength of the block. If the velocity.magnitude of an object hitting it is higher than this, the block will be broken.")]
    [SerializeField] private int _health = 10;
    [Tooltip("Array of short audio clips for when the block bumps into a block that it can't consume.")]
    [SerializeField] private AudioClip[] _bumpClips;
    [Tooltip("A short audio clip for when the block consumes another block.")]
    [SerializeField] private AudioClip _specialActionClip;
    [Tooltip("The particle effect to be played when the block is broken.")]
    [SerializeField] private GameObject _breakGlassParticle;

    private Transform _particleParent;
    private GameObject _particle;


    private void Awake()
    {
        _startAwake = _awakeOnStart;
        Rb = GetComponent<Rigidbody>();
        audioSource = GetComponentInChildren<AudioSource>();
        objectPooler = Singleton.instance.GetComponent<ObjectPooler>();
        GetParticleParent();
    }

    /// <summary>
    /// Sets the particle parent where the particle can be found depending on the name (shape) of the block.
    /// </summary>
    private void GetParticleParent()
    {
        switch (_breakGlassParticle.name)
        {
            case "ShatterGlassBox":
                _particleParent = objectPooler.glassBoxParticleParent;
                break;
            case "ShatterGlassCylinder":
                _particleParent = objectPooler.glassCylinderParticleParent;
                break;
            case "ShatterGlassSphere":
                _particleParent = objectPooler.glassSphereParticleParent;
                break;
            default: break;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        BlockHit(other);
    }

    /// <summary>
    /// Breaks this block if the speed of the other object is more than the health of this block.
    /// Otherwise a bump audio clip is played if the collision velocity is above the threshold and neither object has already played an audio upon this collision.
    /// If one of the objects has already played an audio on this collision, another audio isn't played and both objects are set to not have played an audio.
    /// </summary>
    /// <param name="other"></param> The collision of the other game object.
    private void BlockHit(Collision other)
    {
        if (other.relativeVelocity.magnitude > _health)
        {
            OnBreak?.Invoke();
            BreakGlass();
        }
        else
        {
            if (other.relativeVelocity.magnitude > GetThresholdToPlayBump())
            {
                if (GetClipPlayed())
                {
                    SetClipPlayed(false);
                }
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
    /// Explodes this block if the power of the explosion force of the TNT block is greater than this block's health.
    /// </summary>
    /// <param name="power"></param> The power of the explosion from the TNT block.
    /// <param name="explosionPos"></param> The explosion position of the TNT block.
    /// <param name="radius"></param> The explosion radius of the TNT block
    /// <param name="pushUp"></param> The upward force of the TNT block.
    public override void Explode(float power, Vector3 explosionPos, float radius, float pushUp)
    {
        if (power > _health)
        {
            BreakGlass();
        }
        else
        {
            base.Explode(power, explosionPos, radius, pushUp);
        }
    }

    /// <summary>
    /// Sets the audio volume and plays the audio clip after the block is destroyed and destroys this block.
    /// </summary>
    private void BreakGlass()
    {
        audioSource.volume = .3f;
        PlayAfterDestroy(_specialActionClip);
        PlayParticle();
        Destroy(gameObject);
    }

    /// <summary>
    /// Sets the parameters of the particle effect to be played when this block is broken and activates it.
    /// </summary>
    public void PlayParticle()
    {
        _particle = objectPooler.PopFromPool(_breakGlassParticle, false, true, _particleParent, false);
        _particle.transform.SetLocalPositionAndRotation(transform.position, transform.rotation);
        _particle.transform.localScale = gameObject.transform.localScale;
        _particle.SetActive(true);
    }

}
