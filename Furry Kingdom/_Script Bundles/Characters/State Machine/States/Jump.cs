using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

namespace Furry
{

public class Jump : IState
{
        private NavMeshMovementNPC _navMovement;
        private Animator _animator;
        private bool _isJumping;

        public Jump(NavMeshMovementNPC npcNavMovement, Animator animator)
        {
            _navMovement = npcNavMovement;
            _animator = animator;
        }

        public void Tick()
        {
            Debug.Log("Jumping");
        }

        public void JumpNav()
        {
        }

        public void OnEnter()
        {
            JumpNav();
            //  _navMovement.EnableAgent(true);
            _animator.SetBool("isJumping", true);
            Debug.Log("Entering Mosying state");
        }


        public void OnExit()
        {
            Debug.Log("Exiting Mosying state");
          //  _navMovement.EnableAgent(false);
            //  _animator.SetFloat(Speed, 0);
        }
    }

}