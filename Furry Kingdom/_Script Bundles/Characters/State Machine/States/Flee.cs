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
        private bool _isJumping;
        ParticleSystem _runParticle;
        private float _fleeRange;
        private Player _target;
        public Flee(NavMeshMovementNPC npcNavMovement, Animator animator, ParticleSystem runParticle, float fleeRange, Player target)
        {
            _navMovement = npcNavMovement;
            _animator = animator;
            _runParticle = runParticle;
            _fleeRange = fleeRange;
            _target = target;
        }

        public void Tick()
        {
            Debug.Log("Fleeing");
        }


        public void OnEnter()
        {
            Debug.Log("Fleeing");
            _runParticle.Play();
            _animator.SetBool("isRunning", true);
        }

        public void OnExit()
        {
            Debug.Log("Exit Fleeing");
            _runParticle.Stop();
            _animator.SetBool("isRunning", false);
        }


    }

}