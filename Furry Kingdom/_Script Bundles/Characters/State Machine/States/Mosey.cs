using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

namespace Furry
{

public class Mosey : IState
{
        private NavMeshMovementNPC _navMovement;
        private Animator _animator;
        private float _moseyRange;

        public Mosey(NavMeshMovementNPC npcNavMovement, Animator animator, float moseyRange)
        {
            _navMovement = npcNavMovement;
            _animator = animator;
            _moseyRange = moseyRange;
        }

        public void Tick()
        {
        }

        public void OnEnter()
        {
            Debug.Log("Mosing");
            _animator.SetBool("isWalking", true);
            _navMovement.SearchForLocation(_moseyRange);
        }

        public void OnExit()
        {
            Debug.Log("Exit Mosying");
            _navMovement.CancelSearchForLocation();
            _navMovement.CancelMovingToDestination();
            _animator.SetBool("isWalking", false);
        }

    }

}