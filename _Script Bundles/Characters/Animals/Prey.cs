using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AI;

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
            _playerDetector.SetTriggerRaidus(_runRange);
            _animator = GetComponent<Animator>();

            _stateMachine = new StateMachine();

            var idle = new Idle(_animator, _maxIdleTime);
            var walk = new Walk(_navMeshMovement, _animator, _walkRadius);
            var flee = new Flee(_navMeshMovement, _animator, _runParticleSystem, _playerDetector, this, _runRange);
            var coolDown = new CoolDown(_animator, _playerDetector, _coolDownTime);
            var die = new Die( _animator);

            At(idle, walk, Bored());
            At(walk, idle, Tired());
            At(flee, idle, Calm());
            At(coolDown, idle, Calm());
            At(flee, coolDown, Resting());

            _stateMachine.AddAnyTransition(flee, Scared());
            _stateMachine.AddAnyTransition(die, Killed());

            _stateMachine.SetState(idle); // initial state

            void At(IState to, IState from, Func<bool> condition) => _stateMachine.AddTransition(to, from, condition);

            Func<bool> Tired() => () => _navMeshMovement.Arrived == true && _playerDetector.PlayerDetected == false;
            Func<bool> Calm() => () => _playerDetector.PlayerDetected == null;
            Func<bool> Scared() => () => _playerDetector.PlayerDetected == true && _playerDetector.IsPlayerLeaving == false;
            Func<bool> Resting() => () => _playerDetector.PlayerDetected == true && _playerDetector.IsPlayerLeaving == true;
            Func<bool> Bored() => () => idle.Restless == true && _playerDetector.PlayerDetected == false;
            Func<bool> Killed() => () => CurrentHealth <= 0;
        }

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