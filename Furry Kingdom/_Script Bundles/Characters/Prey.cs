using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AI;

namespace Furry
{

    public class Prey : Animals
    {
        private StateMachine _stateMachine;
        private PlayerDetector _playerDetector;
        private Animator _animator;
        private void Awake()
        {
            _playerDetector = GetComponentInChildren<PlayerDetector>();
            _playerDetector.SetTriggerRaidus(_runRange);
            _animator = GetComponent<Animator>();

            _stateMachine = new StateMachine();

            var idle = new Idle(_animator, _idleTime);
            var mosey = new Mosey(_navMeshMovement, _animator, _moseyRange);
            var flee = new Flee(_navMeshMovement, _animator, _runParticleSystem, _runRange, _playerDetector.PlayerDetected);
            var die = new Die( _animator);
            var jump = new Jump(_navMeshMovement, _animator);

            At(idle, flee, Safe());
            At(idle, mosey, Safe());
            At(mosey, idle, Safe());


            _stateMachine.AddAnyTransition(flee, () => _playerDetector.PlayerDetected);
            _stateMachine.AddAnyTransition(die, Killed());
            At(flee, idle, () => _playerDetector.PlayerDetected == true);

            _stateMachine.SetState(idle); // initial state

            void At(IState to, IState from, Func<bool> condition) => _stateMachine.AddTransition(to, from, condition);

            Func<bool> Tired() => () => _navMeshMovement.Arrived == true;
            Func<bool> Safe() => () => _playerDetector.PlayerDetected == false;
            Func<bool> Killed() => () => CurrentHealth <= 0;
        }

            void Start()
        {

        }

        private void Update() => _stateMachine.Tick();

        private void OnTriggerEnter(Collider other)
        {
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, _moseyRange);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, _runRange);
        }
    }
}