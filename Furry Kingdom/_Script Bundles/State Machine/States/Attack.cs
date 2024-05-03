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

        /// <summary>
        /// This is run every updated while this state is active.
        /// </summary>
        public void Tick()
        {
            if (_predator.AttackRange < Vector3.Distance(_predator.transform.position, _target.transform.position))
            {
                _playerDetector.PlayerInAttackRange = false;
            }
            PrepareForAttack();
        }

        /// <summary>
        /// Faces toward target and initiates attack when required.
        /// </summary>
        private void PrepareForAttack()
        {
            _predator.gameObject.transform.LookAt(_target.transform.position);

            if (_nextAttackTime <= Time.time)
            {
                AttackTarget();
                _nextAttackTime = Time.time + 10f / _agility;
            }
        }

        /// <summary>
        /// This is run each time this state is entered.
        /// </summary>
        public void OnEnter()
        {
            _target = _playerDetector.PlayerDetected;
            _animator.SetBool("isAttacking", true);
            //   Debug.Log("Attacking");
        }

        /// <summary>
        /// This is run each time this state is exited.
        /// </summary>
        public void OnExit()
        {
            _animator.SetBool("isAttacking", false);
            //  Debug.Log("Exit Attacking");
        }

        /// <summary>
        /// Initiates the attack sequence.
        /// </summary>
        private void AttackTarget()
        {
            _animator.SetTrigger("isBiting");
            //  ATTACK SEQUENCE IS IS NOT IMPLEMENTED YET  ***
        }


    }

}