using System;
using UnityEngine;

namespace Furry
{

    public class Predator : Animal, ICanAttack
    {
        [Header("Attack fields.")]
        [Tooltip("Maximum range possible to attack.")]
        [SerializeField] private float _attackRange;
        [Tooltip("Percentage change of adding effectiveness to an attack.")]
        [SerializeField] private int _percentCritChance;

        private StateMachine _stateMachine;
        private PlayerDetector _playerDetector;
        private Animator _animator;

        public float AttackRange
        {
            get { return _attackRange; }
            set { _attackRange = value; }
        }
        public int PercentCritChance
        {
            get { return _percentCritChance; }
            set { _percentCritChance = value; }
        }

        protected override void Awake()
        {
            base.Awake();
            _playerDetector = GetComponentInChildren<PlayerDetector>();
            _playerDetector.SetTriggerRaidus(_runRange);
            _animator = GetComponent<Animator>();

            // Create the state machine.
            _stateMachine = new StateMachine();

            // Available states.
            var idle = new Idle(_animator, _maxIdleTime);
            var walk = new Walk(_navMeshMovement, _animator, _walkRadius);
            var chase = new Chase(_navMeshMovement, _animator, _runParticleSystem, _playerDetector, _attackRange, Speed);
            var attack = new Attack(_animator, Agility, _playerDetector, this);
            var die = new Die(_animator);

            // States added.
            At(idle, walk, Bored());
            At(walk, idle, Tired());
            At(chase, idle, Calm());
            At(chase, attack, Attacking());

            _stateMachine.AddAnyTransition(chase, Angry());
            //  _stateMachine.AddAnyTransition(die, Killed());

            // The initial state.
            _stateMachine.SetState(idle);

            // Add transitions to the state machine.
            void At(IState to, IState from, Func<bool> condition) => _stateMachine.AddTransition(to, from, condition);

            // Conditionas that are used to change the states.
            Func<bool> Tired() => () => _navMeshMovement.Arrived == true && _playerDetector.PlayerDetected == false;
            Func<bool> Calm() => () => _playerDetector.PlayerDetected == false;
            Func<bool> Angry() => () => _playerDetector.PlayerDetected == true && _playerDetector.PlayerInAttackRange == false;
            Func<bool> Attacking() => () => _playerDetector.PlayerInAttackRange == true;
            Func<bool> Bored() => () => idle.Restless == true && _playerDetector.PlayerDetected == false;
            //  Func<bool> Killed() => () => CurrentHealth <= 0;   *** NEED TO IMPLEMENT HEALTH SYSTEM ***

        }

        /// <summary>
        /// Run the Tick method on the state machine every update.
        /// </summary>
        private void Update() => _stateMachine.Tick();

        /// <summary>
        /// Initialize attack sequence.
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="defender"></param>
        public void Attack(Character attacker, Character defender)
        {
            // NOT IMPLEMENTED YET  ***
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, _walkRadius);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _runRange);
        }

    }
}
