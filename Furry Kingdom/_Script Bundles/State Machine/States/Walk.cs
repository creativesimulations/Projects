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
            _animator.SetBool("isWalking", true);
            _navMovement.Walk(_walkRange);
        }

        public void OnExit()
        {
            _navMovement.CancelGetNewLocation();
            _navMovement.CancelMovingToDestination();
            _animator.SetBool("isWalking", false);
        }

    }

}