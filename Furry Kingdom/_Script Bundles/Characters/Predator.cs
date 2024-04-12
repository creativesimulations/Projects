using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Furry
{

    public class Predator : Animals
    {

        [SerializeField] public float AttackRange;
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

            var idle = new Idle(_animator, _idleTime);
            var mosey = new Mosey(_navMeshMovement, _animator, _moseyRange);
            var chase = new Chase(_navMeshMovement, _animator, _runParticleSystem, _playerDetector, this);
            var attack = new Attack(_animator, Agility, _playerDetector, this);
            var die = new Die(_animator);

              At(idle, mosey, Bored());
            At(mosey, idle, Tired());
            At(chase, idle, Calm());
            At(attack, idle, Calm());
            At(chase, attack, Attacking());

            _stateMachine.AddAnyTransition(chase, Angry());
            _stateMachine.AddAnyTransition(die, Killed());

            _stateMachine.SetState(idle); // initial state

            void At(IState to, IState from, Func<bool> condition) => _stateMachine.AddTransition(to, from, condition);

            Func<bool> Tired() => () => _navMeshMovement.Arrived == true && _playerDetector.PlayerDetected == false;
            Func<bool> Calm() => () => _playerDetector.PlayerDetected == false;
            Func<bool> Angry() => () => _playerDetector.PlayerDetected == true && _playerDetector.PlayerInAttackRange == false;
            Func<bool> Attacking() => () => _playerDetector.PlayerInAttackRange == true;
            Func<bool> Bored() => () => idle.Ancy == true && _playerDetector.PlayerDetected == false;
            Func<bool> Killed() => () => CurrentHealth <= 0;

        }



        private void Update() => _stateMachine.Tick();

        private void OnTriggerEnter(Collider other)
        {
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, _moseyRange);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _runRange);
        }

    }
}
