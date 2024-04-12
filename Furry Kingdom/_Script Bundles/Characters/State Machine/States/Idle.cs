using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

namespace Furry
{

public class Idle : IState
{
        private Animator _animator;
        private float _moveAgainTime;

        private float _idleTime;
        public bool Ancy => _moveAgainTime < 0;

        public Idle(Animator animator, float idleTime)
        {
            _animator = animator;
            _idleTime = idleTime;
        }

        public void Tick()
        {
            _moveAgainTime -= Time.deltaTime;
        }
        public void OnEnter()
        {
            Debug.Log("Idling");
            _animator.SetBool("isIdle", true);
        }

        public void OnExit()
        {
            Debug.Log("Exit Idling");
            _moveAgainTime = UnityEngine.Random.Range(_idleTime, (_idleTime * 1.5f));
            _animator.SetBool("isIdle", false);
        }


    }

}