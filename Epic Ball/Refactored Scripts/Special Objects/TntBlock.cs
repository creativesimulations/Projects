using Core;
using EpicBall;
using MilkShake.Demo;
using System;
using System.Drawing;
using UnityEngine;

public class TntBlock : Block, ITeleportable, IConsumeable
{
    public static event Action OnExplode;

    [Header("Setup parameters for this block.")]
    [Tooltip("If the rigid body on this block should be awake at start.")]
    [SerializeField] private bool _awakeOnStart;
    [Tooltip("The strength of the block. If the velocity.magnitude of an object hitting it is higher than this, the block will be broken.")]
    [SerializeField] private int _health = 10;
    [Tooltip("The radisu of the explosion.")]
    [SerializeField] private float _radius = 5.0F;
    [Tooltip("The power of the explosion.")]
    [SerializeField] private float _power = 400.0F;
    [Tooltip("The upward force of the explosion.")]
    [SerializeField] private float _pushUp = 4.0F;
    [Tooltip("Array of short audio clips for when the block bumps into a block that it can't consume.")]
    [SerializeField] private AudioClip[] _bumpClips;
    [Tooltip("A short audio clip for when the block explodes.")]
    [SerializeField] private AudioClip _specialActionClip;

    private BoxCollider _boxCollider;
    private bool _initialExplosion;
    private ShakeButton _shakeButton;
    private GameObject _particle;


    private void Awake()
    {
        _startAwake = _awakeOnStart;
        Rb = GetComponent<Rigidbody>();
        audioSource = GetComponentInChildren<AudioSource>();
        objectPooler = Singleton.instance.GetComponent<ObjectPooler>();
        _shakeButton = GetComponentInChildren<ShakeButton>();
    }
    private void Start()
    {
        _boxCollider = GetComponent<BoxCollider>();
    }

    private void OnCollisionEnter(Collision other)
    {
        CheckExplosion(other);
    }

    /// <summary>
    /// Explodes this block if the speed of the other object is more than the health of this block.
    /// Otherwise a bump audio clip is played if the collision velocity is above the threshold and the other object hasn't already played one upon this collision.
    /// </summary>
    /// <param name="other"></param> The collision of the other game object.
    private void CheckExplosion(Collision other)
    {
        if (other.relativeVelocity.magnitude > _health)
        {
            _initialExplosion = true;
            _shakeButton.UIShakeOnce();
            Explode(_power, transform.position, _radius, _pushUp);
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

    private float AddPower(float powerToCheck)
    {
        if (!_initialExplosion)
        {
            powerToCheck += 200;
        }
        return powerToCheck;
    }

    /// <summary>
    /// Sends out a spherecast to find all objects within its radius with an IExplodable interface. Then calls their explode methods. This object is then destroyed.
    /// </summary>
    /// <param name="power"></param> The power of the explosion.
    /// <param name="explosionPos"></param> The origin of the explosion.
    /// <param name="radius"></param> The radisu of the explosion.
    /// <param name="pushUp"></param> The upward force of this explosion.
    public override void Explode(float power, Vector3 explosionPos, float radius, float pushUp)
    {
        power = AddPower(power);
        _boxCollider.enabled = false;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
        PlayParticle();
        PlayAfterDestroy(_specialActionClip);

        foreach (Collider hit in colliders)
        {
            if (hit.gameObject != gameObject)
            {
                var objectToExplode = hit.gameObject.GetComponent<IExplodable>();
                if (objectToExplode != null)
                {
                    objectToExplode.Explode(power, explosionPos, _radius, pushUp);
                }
            }
        }
        Destroy(gameObject);
    }
    private void PlayParticle()
    {
        _particle = objectPooler.PopFromPool(objectPooler.tntParticle, false, true, objectPooler.tntParticleParent, false);
        _particle.transform.position = transform.position;
        _particle.SetActive(true);
    }
}
