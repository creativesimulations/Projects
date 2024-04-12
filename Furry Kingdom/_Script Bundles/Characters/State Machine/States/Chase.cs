using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

namespace Furry
{

public class Chase : IState
    {
        private NavMeshMovementNPC _navMovement;
        private Animator _animator;
        ParticleSystem _runParticle;
        private Player _target;
        private Predator _predator;
        private float _speedModifier = 1.5f;
        private PlayerDetector _playerDetector;
        public Chase(NavMeshMovementNPC npcNavMovement, Animator animator, ParticleSystem runParticle, PlayerDetector playerDetector, Predator predator)
        {
            _navMovement = npcNavMovement;
            _animator = animator;
            _runParticle = runParticle;
            _playerDetector = playerDetector;
            _predator = predator;
        }

        public void Tick()
        {
            _navMovement.Chase(_target.transform.position);
            if (_predator.AttackRange >= Vector3.Distance(_predator.transform.position, _target.transform.position))
            {
                _playerDetector.PlayerInAttackRange = true;
            }
        }
        public void OnEnter()
        {
            _target = _playerDetector.PlayerDetected;
            Debug.Log("Chasing");
            _navMovement.SetMovementSpeed(_predator.Speed * _speedModifier);
            
            _runParticle.Play();
            _animator.SetBool("isRunning", true);
        }

        public void OnExit()
        {
            Debug.Log("Exit Chasing");
            _navMovement.SetMovementSpeed(_predator.Speed);
            _navMovement.CancelMovingToDestination();
            _runParticle.Stop();
            _animator.SetBool("isRunning", false);
        }


    }

}