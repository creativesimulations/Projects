using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

namespace Furry
{

public class Die : IState
    {
        private Animator _animator;
        public Die(Animator animator)
        {
            _animator = animator;
        }

        public void Tick()
        {
        }

        public void OnEnter()
        {
            Debug.Log("Dying");
            _animator.SetBool("isDead", true);
        }

        public void OnExit()
        {
            Debug.Log("Exit Dying");
        }


    }

}