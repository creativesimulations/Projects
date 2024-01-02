using UnityEngine;
using MilkShake.Demo;
using System;
using Core;

namespace EpicBall
{

    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(ShakeButton))]

    public class Ball : MonoBehaviour, IExplodable, ITeleportable, IBump
    {
        public static event Action OnDestroyed;
        public static event Action OnFall;
        public static event Action<GameObject> OnChangeLocation;

        [Header("The paramater settings for the player.")]
        [Tooltip("The upward force applied for the player to jump.")]
        [SerializeField] private float _jumpPower = 400f;
        [Tooltip("The desired maximum speed of the player.")]
        [SerializeField] private float _desiredSpeed = 10f;
        [Tooltip("The amount of force to be applied to the player for movement.")]
        [SerializeField] private float _forceConstant = 5f;
        [Tooltip("The particle to be played when the player jumps.")]
        [SerializeField] private GameObject _jumpParticle;
        [Tooltip("The particle to be played when the player is teleported.")]
        [SerializeField] private GameObject _teleportParticle;
        [Tooltip("The particle to be played when the player is killed.")]
        [SerializeField] private ParticleSystem _deathParticle;

        [Tooltip("Array of skins for the player")]
        [SerializeField] private GameObject[] _skins;
        [Tooltip("Array of short audio clips for when the player bumps into something.")]
        [SerializeField] private AudioClip[] _bumpClips;
        [Tooltip("Array of short audio clips for when the player jumps.")]
        [SerializeField] private AudioClip[] _jumpClips;
        [Tooltip("A short audio clip for when the player teleports.")]
        [SerializeField] private AudioClip _teleClip;
        [Tooltip("A short audio clip for when the player explodes.")]
        [SerializeField] private AudioClip _explodeClip;

        private AudioSource _audioSource;
        [SerializeField] private float _xVelocityDrag = .001f;
        [SerializeField] private float _zVelocityDrag = .001f;
        private int _thresholdToPlayBump = 15;
        private bool _win = false;
        private Rigidbody _rb;
        private Vector3 _startPosition;
        private bool _clipPlayed;
        private ObjectPooler _objectPooler;
        private GameObject _currentSkin;

        private Vector3 _vel;
        private float _forceMultiplier;
        private GameObject _particle;
        private int _index;

        private void Awake()
        {
            _audioSource = GetComponentInChildren<AudioSource>();
            _rb = GetComponent<Rigidbody>();
            _objectPooler = Singleton.instance.GetComponent<ObjectPooler>();
        }

        private void Start()
        {
            GameManager.Die += DestroyPlayer;
            GameManager.CompleteLvl += WinGame;
            PlaneDetector.FellToDeath += FallToDeath;
            BallSkinController.ChoseSkin += SetSkin;
            Hole.OnPlayerFall += FallDownHole;
            SetSkin();
            _startPosition = transform.position;
            OnChangeLocation?.Invoke(gameObject);
        }

        /// <summary>
        /// Sets the final velocity of the ball on the x axis.
        /// </summary>
        #region Mechanics
        public void SlowBallx()
        {
            _vel = _rb.velocity;
            _vel.x *= _xVelocityDrag;
            _rb.velocity = _vel;
        }

        /// <summary>
        /// Sets the final velocity of the ball on the z axis.
        /// </summary>
        public void SlowBallz()
        {
            _vel = _rb.velocity;
            _vel.z *= _zVelocityDrag;
            _rb.velocity = _vel;
        }

        /// <summary>
        /// Replaces the current skin of the ball with the chosen skin that is saved in the PlayerPrefs.
        /// </summary>
        private void SetSkin()
        {
            if (_skins.Length > 0)
            {
                _currentSkin = GetComponentInChildren<Skin>().gameObject;
                Vector3 currentPosition = _currentSkin.transform.position;
                Destroy(_currentSkin);
                _currentSkin = Instantiate(_skins[PlayerPrefsController.GetChosenSkin()], currentPosition, Quaternion.identity, gameObject.transform);
            }
            else
            {
                ExceptionManager.instance.SendEmptyContainerMessage("_skins", GetType().ToString(), name);
            }
        }
        #endregion

        #region Actions
        /// <summary>
        /// Adds force to the player to move it in the desired direction.
        /// </summary>
        /// <param name="moveDirection"></param> The desired direction for the player to move.
        public void Move(Vector3 moveDirection, float currentSpeed)
        {
            _forceMultiplier = Mathf.Clamp01((_desiredSpeed - _rb.velocity.magnitude) / _desiredSpeed);
            _rb.AddForce(moveDirection * (_forceMultiplier * Time.deltaTime * _forceConstant * currentSpeed), ForceMode.Impulse);
        }

        /// <summary>
        /// Adds upward force to the player to make it jump and plays an audio and particle effect.
        /// </summary>
        public void BallJump()
        {
            _rb.velocity = _rb.velocity + Vector3.up * _jumpPower;
            PlayParticle(_jumpParticle, _objectPooler.jumpParticleParent);

            if (_jumpClips.Length != 0)
            {
                PlayJumpClip(_jumpClips);
            }
            else
            {
                ExceptionManager.instance.SendEmptyContainerMessage("_jumpClips", GetType().ToString(), name);
            }
        }

        /// <summary>
        /// Sets a desired particle effect and activates it.
        /// </summary>
        /// <param name="particleToPlay"></param> The particle to play.
        /// <param name="particleParent"></param> The parent of the particle.
        public void PlayParticle(GameObject particleToPlay, Transform particleParent)
        {
            if (particleToPlay != null && particleParent != null)
            {
                _particle = _objectPooler.PopFromPool(particleToPlay, false, true, particleParent, false);
                _particle.transform.position = transform.position;
                _particle.SetActive(true);
            }
            else
            {
                if (particleToPlay == null)
                {
                    ExceptionManager.instance.SendMissingObjectMessage(particleToPlay.name, GetType().ToString(), name);
                }
                if (particleParent == null)
                {
                    ExceptionManager.instance.SendMissingObjectMessage(particleParent.name, GetType().ToString(), name);
                }
            }
        }

        /// <summary>
        /// Adds explosive forces to the player.
        /// </summary>
        /// <param name="_power"></param> The power of the explosion.
        /// <param name="explosionPos"></param> The origin position of the explosion.
        /// <param name="_radius"></param> the radius of the explosion.
        /// <param name="_pushUp"></param> The upward force of the explosion.
        public void Explode(float _power, Vector3 explosionPos, float _radius, float _pushUp)
        {
            _rb.AddExplosionForce(_power, explosionPos, _radius, _pushUp);
        }

        /// <summary>
        /// If the level isn't yet complete, the player is teleported to the start position and an audio and particle are played.
        /// </summary>
        public void Teleport()
        {
            if (!_win)
            {
                transform.position = _startPosition;
                OnChangeLocation?.Invoke(gameObject);
                if (_teleClip != null)
                {
                    _audioSource.PlayOneShot(_teleClip);
                }
                else
                {
                    ExceptionManager.instance.SendMissingObjectMessage("_teleClip", GetType().ToString(), name);
                }
                PlayParticle(_teleportParticle, _objectPooler.teleportParticleParent);
            }
        }

        /// <summary>
        /// Teleports the player to a desired position.
        /// </summary>
        /// <param name="teleDestination"></param> The desired position.
        public void Teleport(Vector3 teleDestination)
        {
            if (!_win)
            {
                transform.position = teleDestination;
                OnChangeLocation?.Invoke(gameObject);
            }
        }

        /// <summary>
        /// Sends a notification that the player's position has changed. This is used to run the method for checking the height of the player against the planes.
        /// </summary>
        private void FallDownHole()
        {
            OnChangeLocation?.Invoke(gameObject);
        }

        /// <summary>
        /// Separates the audio source object from the player, destroys the player object, plays an audio clip and a particle effect and shakes the camera.
        /// </summary>
        public void DestroyPlayer()
        {
            OnDestroyed?.Invoke();
            PlayDeathParticle();
            PlayAfterDestroy(_explodeClip);
            GetComponent<ShakeButton>().UIShakeOnce();
            _audioSource.gameObject.transform.parent = null;
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Sends a notification that the player has fallen below the lowest plane.
        /// </summary>
        private void FallToDeath()
        {
            OnFall?.Invoke();
        }

        /// <summary>
        /// Sets the player as having completed the level and stops the player object where it is.
        /// </summary>
        public void WinGame()
        {
            if (!_win)
            {
                _win = true;
                _rb.useGravity = false;
                _rb.velocity = Vector3.zero;
                _rb.constraints = RigidbodyConstraints.FreezeAll;
            }
        }
        #endregion

        #region Internal Methods
        private void OnCollisionEnter(Collision other)
        {
            OnBump(other);
        }

        /// <summary>
        /// A bump audio clip is played if the collision velocity is above the threshold and neither object has already played an audio upon this collision.
        /// If one of the objects has already played an audio on this collision, another audio isn't played and both objects are set to not have played an audio.
        /// </summary>
        /// <param name="other"></param> The collision of the other game object.
        private void OnBump(Collision other)
        {
            if (other.relativeVelocity.magnitude > _thresholdToPlayBump)
            {
                if (GetClipPlayed())
                {
                    SetClipPlayed(false);
                }
                var bump = other.gameObject.GetComponent<IBump>();

                if (bump == null)
                {
                    _audioSource.volume = other.relativeVelocity.magnitude / 10;
                    PlayBumpClip(_bumpClips);
                }
                else if (!bump.GetClipPlayed())
                {
                    _audioSource.volume = other.relativeVelocity.magnitude / 10;
                    PlayBumpClip(_bumpClips);
                    SetClipPlayed(true);
                }
            }
        }

        /// <summary>
        /// Sets a desired array of audio clips.
        /// </summary>
        /// <param name="clipsToPlay"></param> The audio clip array.
        private void PlayJumpClip(AudioClip[] clipsToPlay)
        {
            PlayRandomClip(clipsToPlay);
        }

        /// <summary>
        /// Sets a desired array of audio clips.
        /// </summary>
        /// <param name="clipsToPlay"></param> The audio clip array.
        public void PlayBumpClip(AudioClip[] clipsToPlay)
        {
            PlayRandomClip(_bumpClips);
        }

        /// <summary>
        /// Plays a random audio clip from an array.
        /// </summary>
        /// <param name="clipsToPlay"></param> The audio clip array to pick from.
        public void PlayRandomClip(AudioClip[] clipsToPlay)
        {
            _index = UnityEngine.Random.Range(0, _bumpClips.Length);
            AudioClip bumpClip = _bumpClips[_index];
            _audioSource.PlayOneShot(bumpClip);
        }

        /// <summary>
        /// Plays the death particle for when the player is destroyed. 
        /// </summary>
        public void PlayDeathParticle()
        {
            Instantiate(_deathParticle, transform.position, Quaternion.identity);
        }

        /// <summary>
        /// Plays the audio clip for when the player is destroyed.
        /// </summary>
        /// <param name="clipToPlay"></param> The audio clip to be played.
        public void PlayAfterDestroy(AudioClip clipToPlay)
        {
            if (clipToPlay != null)
            {
                AudioSourceExtensions.PlayAfterDestroy(_audioSource, clipToPlay);
            }
            else
            {
                ExceptionManager.instance.SendMissingObjectMessage(clipToPlay.name, GetType().ToString(), name);
            }
        }

        /// <summary>
        /// Plays a desired audio clip.
        /// </summary>
        /// <param name="clipToPlay"></param> The audio clip to be played.
        public void PlaySpecialActionClip(AudioClip clipToPlay)
        {
            if (clipToPlay != null)
            {
                _audioSource.PlayOneShot(clipToPlay);
            }
            else
            {
                ExceptionManager.instance.SendMissingObjectMessage(clipToPlay.name, GetType().ToString(), name);
            }
        }

        /// <summary>
        /// Returns the state whether a bump audio clip has been played or not.
        /// </summary>
        /// <returns></returns>
        public bool GetClipPlayed()
        {
            return _clipPlayed;
        }

        /// <summary>
        /// Sets the state whether a bump audio clip has been played or not.
        /// </summary>
        /// <param name="state"></param>
        public void SetClipPlayed(bool state)
        {
            _clipPlayed = state;
        }
        private void OnDisable()
        {
            GameManager.Die -= DestroyPlayer;
            GameManager.CompleteLvl -= WinGame;
            PlaneDetector.FellToDeath -= FallToDeath;
            BallSkinController.ChoseSkin -= SetSkin;
            Hole.OnPlayerFall -= FallDownHole;
        }
        #endregion
    }
}