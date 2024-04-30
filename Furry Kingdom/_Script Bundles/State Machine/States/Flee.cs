using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

namespace Furry
{

public class Flee : IState
{
        private NavMeshMovementNPC _navMovement;
        private Animator _animator;
        ParticleSystem _runParticle;
        private Player _target;
        private Prey _prey;
        private float _speedModifier = 1.5f;
        private PlayerDetector _playerDetector;
        private float _runRange;
        public Flee(NavMeshMovementNPC npcNavMovement, Animator animator, ParticleSystem runParticle, PlayerDetector playerDetector, Prey prey, float runRange)
        {
            _navMovement = npcNavMovement;
            _animator = animator;
            _runParticle = runParticle;
            _playerDetector = playerDetector;
            _prey = prey;
            _runRange = runRange;
        }

        public void Tick()
        {
            if (_navMovement.Arrived && !_playerDetector.IsPlayerLeaving)
            {
                _navMovement.Flee(_target.transform.position, _runRange);
            }
        }


        public void OnEnter()
        {
         //   Debug.Log("Fleeing");
            _target = _playerDetector.PlayerDetected;
            _navMovement.SetMovementSpeed(_prey.Speed * _speedModifier);
            _runParticle.Play();
            _animator.SetBool("isFleeing", true);
        }

        public void OnExit()
        {
         //   Debug.Log("Exit Fleeing");
            _navMovement.SetMovementSpeed(_prey.Speed);
            _navMovement.CancelGetNewLocation();
            _runParticle.Stop();
            _animator.SetBool("isFleeing", false);
        }


    }

}