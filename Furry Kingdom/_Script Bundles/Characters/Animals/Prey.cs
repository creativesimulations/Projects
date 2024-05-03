using System;
using UnityEngine;

namespace Furry
{

    public class Prey : Animal
    {
        private StateMachine _stateMachine;
        private PlayerDetector _playerDetector;
        private Animator _animator;
        protected override void Awake()
        {
            base.Awake();
            _playerDetector = GetComponentInChildren<PlayerDetector>();
            _animator = GetComponent<Animator>();

            // Create the state machine.
            _stateMachine = new StateMachine();

            // Available states.
            var idle = new Idle(_animator, _maxIdleTime);
            var walk = new Walk(_navMeshMovement, _animator, _walkRadius);
            var flee = new Flee(_navMeshMovement, _animator, _runParticleSystem, _playerDetector, Speed, _runRange);
            var coolDown = new CoolDown(_animator, _coolDownTime);
            var die = new Die(_animator);

            // States added.
            At(idle, walk, Bored());
            At(walk, idle, Tired());
            At(flee, idle, Calm());
            At(coolDown, idle, Calm());
            At(flee, coolDown, Resting());

            _stateMachine.AddAnyTransition(flee, Scared());
            //  _stateMachine.AddAnyTransition(die, Killed());

            // The initial state.
            _stateMachine.SetState(idle);

            // Add transitions to the state machine.
            void At(IState to, IState from, Func<bool> condition) => _stateMachine.AddTransition(to, from, condition);

            // Conditionas that are used to change the states.
            Func<bool> Tired() => () => _navMeshMovement.Arrived == true && _playerDetector.PlayerDetected == false;
            Func<bool> Calm() => () => _playerDetector.PlayerDetected == null;
            Func<bool> Scared() => () => _playerDetector.PlayerDetected == true && _playerDetector.IsPlayerLeaving == false;
            Func<bool> Resting() => () => _playerDetector.PlayerDetected == true && _playerDetector.IsPlayerLeaving == true;
            Func<bool> Bored() => () => idle.Restless == true && _playerDetector.PlayerDetected == false;
            //    Func<bool> Killed() => () => CurrentHealth <= 0;   *** NEED TO IMPLEMENT HEALTH SYSTEM ***
        }

        /// <summary>
        /// Run the Tick method on the state machine every update.
        /// </summary>
        private void Update() => _stateMachine.Tick();

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, _walkRadius);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, _runRange);
        }
    }
}