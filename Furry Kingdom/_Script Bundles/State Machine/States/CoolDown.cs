using UnityEngine;

namespace Furry
{

    public class CoolDown : IState
    {
        private Animator _animator;
        private float _coolDownTime;
        private float _endCoolDownTime;

        public CoolDown(Animator animator, float coolDownTime)
        {
            _animator = animator;
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