using UnityEngine;

namespace Furry
{

public class Walk : IState
{
        private NavMeshMovementNPC _navMovement;
        private Animator _animator;
        private float _walkRange;

        public Walk(NavMeshMovementNPC npcNavMovement, Animator animator, float walkRange)
        {
            _navMovement = npcNavMovement;
            _animator = animator;
            _walkRange = walkRange;
        }

        public void Tick()
        {
        }

        public void OnEnter()
        {
            Debug.Log("Walking");
            _animator.SetBool("isWalking", true);
            _navMovement.Walk(_walkRange);
        }

        public void OnExit()
        {
            Debug.Log("Exit Walking");
            _navMovement.CancelSearchForLocation();
            _navMovement.CancelMovingToDestination();
            _animator.SetBool("isWalking", false);
        }

    }

}