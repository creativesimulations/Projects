using System;
using System.Collections;
using UnityEngine;

namespace EpicBall
{
    public class Hole : MonoBehaviour
    {
        public static event Action OnPlayerFall;

        [Header("Setup parameters.")]
        [Tooltip("Every how many seconds the hole changes position.")]
        [SerializeField] private float _changePositionEvery = 4;
        [Tooltip("The size of the hole.")]
        [SerializeField] private float _holeSize = 6f;
        [Tooltip("The raidius of the hole movement.")]
        [SerializeField] private float _movementRadius;
        [Tooltip("The speed of the hole movement.")]
        [SerializeField] private float _movementSpeed = 5f;
        [Tooltip("The audio clip to be played when a block falls through the hole.")]
        [SerializeField] private AudioClip _fallClip;

        private Vector3 _destination;
        private Vector3 _parentPosition;
        private AudioSource _audioSource;
        private ParticleSystem _particleSystem;
        private float _speed;
        private float _solidifyAt;
        private Vector3 _colliderBoundsSize;
        private float _XAxis;
        private float _ZAxis;


        private void Awake()
        {
            _parentPosition = transform.parent.position;
            _audioSource = GetComponent<AudioSource>();
            _particleSystem = GetComponentInChildren<ParticleSystem>();
        }

        void Start()
        {
            SetupScales();
            InvokeRepeating("Move", .1f, _changePositionEvery);
        }

        /// <summary>
        /// Sets the scales of this game object and the particle attached according to the hole size parameter.
        /// </summary>
        private void SetupScales()
        {
            float newParticleSize = _holeSize * 1.5f;
            transform.localScale = new Vector3(_holeSize, transform.localScale.y, _holeSize);
            _particleSystem.transform.localScale = new Vector3(newParticleSize, .01f, newParticleSize);
        }

        /// <summary>
        /// Moves the hole over time.
        /// </summary>
        private void FixedUpdate()
        {
            _speed = _movementSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, _destination, _speed);
        }

        void OnTriggerEnter(Collider other)
        {
            CheckSize(other);
        }

        /// <summary>
        /// Compares the x and y axis sizes of the colliding block with that of the hole size. If it fits the fall coroutine is run.
        /// </summary>
        private void CheckSize(Collider other)
        {
            if (other.bounds.size.x <= _holeSize && other.bounds.size.z <= _holeSize)
            {
                StartCoroutine(FallThroughHole(other));
            }
        }

        /// <summary>
        /// When an object touches a hole the collider of the object is turned off so that it falls through the plane it is on. After it falls the distance of its height the collider is turned back on.
        /// </summary>
        /// <param name="other"></param> The collider of the other object.
        /// <returns></returns>
        private IEnumerator FallThroughHole(Collider other)
        {
            _audioSource.PlayOneShot(_fallClip);
            _colliderBoundsSize = other.bounds.size;
            _solidifyAt = other.transform.position.y - _colliderBoundsSize.y;
            other.enabled = false;
            while (other != null && other.transform.position.y > _solidifyAt)
            {
                yield return new WaitForEndOfFrame();
            }
            if (other.gameObject.CompareTag(GlobalConstants.PLAYER))
            {
                OnPlayerFall?.Invoke();
            }
            other.enabled = true;
        }

        /// <summary>
        /// Moves the hole to random x and z coordinates in relation to the parent object position and the movement raidus amount.
        /// </summary>
        private void Move()
        {
            _XAxis = UnityEngine.Random.Range(_parentPosition.x + _movementRadius, _parentPosition.x - _movementRadius);
            _ZAxis = UnityEngine.Random.Range(_parentPosition.z + _movementRadius, _parentPosition.z - _movementRadius);
            _destination = new Vector3(_XAxis, _parentPosition.y, _ZAxis);
        }
        void OnDrawGizmosSelected()
        {
            float outsideOfHole = _movementRadius + _holeSize;
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(transform.position, _movementRadius);
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, _holeSize);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, outsideOfHole);
        }

    }
}