using UnityEngine;

namespace Furry
{

    public class Chase : IState
    {
        private NavMeshMovementNPC _navMovement;
        private Animator _animator;
        ParticleSystem _runParticle;
        private Player _target;
        private float _speedModifier = 1.5f;
        private PlayerDetector _playerDetector;
        private float _attackRange;
        private int _speed;

        public Chase(NavMeshMovementNPC npcNavMovement, Animator animator, ParticleSystem runParticle, PlayerDetector playerDetector, float attackRange, int speed)
        {
            _navMovement = npcNavMovement;
            _animator = animator;
            _runParticle = runParticle;
            _playerDetector = playerDetector;
            _attackRange = attackRange;
            _speed = speed;
        }

        public void Tick()
        {
            _navMovement.Chase(_target.transform.position);
            _playerDetector.CheckAttackRadius(_attackRange);
        }
        public void OnEnter()
        {
            _target = _playerDetector.PlayerDetected;
            _navMovement.SetMovementSpeed(_speed * _speedModifier);
            _runParticle.Play();
            _animator.SetBool("isRunning", true);
        }

        public void OnExit()
        {
            _navMovement.SetMovementSpeed(_speed);
            _navMovement.CancelMovingToDestination();
            _runParticle.Stop();
            _animator.SetBool("isRunning", false);
        }


    }

}