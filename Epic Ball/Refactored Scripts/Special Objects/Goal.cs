using UnityEngine;

namespace EpicBall
{
    public class Goal : MonoBehaviour
    {
        [Header("Setup parameters for the goal.")]
        [Tooltip("Particle to play on completing the level.")]
        [SerializeField] private ParticleSystem _fireworks;
        [Tooltip("The particle used to show the goal upon collecting all of the gems.")]
        [SerializeField] private ParticleSystem _winParticle;
        [Tooltip("The particle used to show the goal.")]
        [SerializeField] private ParticleSystem _startingParticle;

        private AudioSource _audioSource;
        private bool _allGemsCollected;

        private void Awake()
        {
            _audioSource = GetComponentInChildren<AudioSource>();
        }

        private void Start()
        {
            GemCounterUI.CollectedAllGems += ActivateGoal;

            _startingParticle = Instantiate(_startingParticle, transform.position, Quaternion.identity) as ParticleSystem;
        }

        /// <summary>
        /// Stops the current goal particle and starts the goal particle to show that all of the gems have been colected.
        /// </summary>
        public void ActivateGoal()
        {
            _allGemsCollected = true;
            _startingParticle.Stop();
            _winParticle = Instantiate(_winParticle, transform.position, Quaternion.identity) as ParticleSystem;
        }

        /// <summary>
        /// Checks if the player enters the goal trigger and all of the gems are collected.
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter(Collider other)
        {
            if (_allGemsCollected)
            {
                // If all of the gems are collected and the player touches the goal...
                if (other.gameObject.CompareTag(GlobalConstants.PLAYER))
                {
                    ReachedGoal();
                }
            }
        }

        /// <summary>
        /// If the player reached the goal and all of the gems are collected, the level complete particles are started and the Game Manager is set to 'Complete level'.
        /// </summary>
        private void ReachedGoal()
        {
            // Run win sequence.
            _fireworks = Instantiate(_fireworks, transform.position, Quaternion.Euler(-90, 0, 0)) as ParticleSystem;
            ParticleSystem fireworks2 = Instantiate(_fireworks, transform.position, Quaternion.Euler(-90, 0, 0)) as ParticleSystem;
            _audioSource.PlayOneShot(_audioSource.clip);

            // Change the game state to completed level.
            GameManager.SetGameState(GameManager.GameStates.CompleteLvl);
        }

        private void OnDisable()
        {
            GemCounterUI.CollectedAllGems -= ActivateGoal;
        }
    }
}