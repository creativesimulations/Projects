using UnityEngine;

namespace Furry
{

public class CoolDown : IState
{
        private NavMeshMovementNPC _navMovement;
        private Animator _animator;
        private float _walkRange;
        private PlayerDetector _playerDetector;
        private float _coolDownTime;
        private float _endCoolDownTime;

        public CoolDown(Animator animator, PlayerDetector playerDetector, float coolDownTime)
        {
            _animator = animator;
            _playerDetector = playerDetector;
            _coolDownTime = coolDownTime;
        }

        public void Tick()
        {
            if (Time.time >= _endCoolDownTime)
            {
            }
        }

        public void OnEnter()
        {
          //  Debug.Log("Cooling Down");
            _endCoolDownTime = Time.time + _coolDownTime;
            _animator.SetBool("isIdle", true);
        }

        public void OnExit()
        {
        //    Debug.Log("EXITING Cooling Down");
            _animator.SetBool("isIdle", false);
        }

    }

}