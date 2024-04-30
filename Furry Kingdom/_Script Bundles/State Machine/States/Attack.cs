using UnityEditorInternal.VR;
using UnityEngine;
using UnityEngine.AI;

namespace Furry
{

    public class Attack : IState
    {
        private Animator _animator;
        private Player _target;
        private float _nextAttackTime = 0;
        private float _agility;
        private PlayerDetector _playerDetector;
        private Predator _predator;
        public Attack(Animator animator, float agility, PlayerDetector playerDetector, Predator predator)
        {
            _animator = animator;
            _agility = agility;
            _playerDetector = playerDetector;
            _predator = predator;
        }

        public void Tick()
        {
            _predator.gameObject.transform.LookAt(_target.transform.position);

            if (_predator.AttackRange < Vector3.Distance(_predator.transform.position, _target.transform.position))
            {
                _playerDetector.PlayerInAttackRange = false;
            }
            if (_nextAttackTime <= Time.time)
                {
                    AttackTarget();
                    _nextAttackTime = Time.time + 10f / _agility;
                }
        }
        public void OnEnter()
        {
            _target = _playerDetector.PlayerDetected;
            _animator.SetBool("isAttacking", true);
         //   Debug.Log("Attacking");
        }


        public void OnExit()
        {
            _animator.SetBool("isAttacking", false);
          //  Debug.Log("Exit Attacking");
        }

        private void AttackTarget()
        {
            _animator.SetTrigger("isBiting");
        }


    }

}