using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace Furry
{
    [RequireComponent(typeof(Animator))]
    public class CharacterStateController : MonoBehaviour
    {
        private StateMachine _stateMachine;
        private NavMeshAgent _agent;
        private PlayerDetector _playerDetector;
        private Animator _animator;

        void Start()
        {

        }
        private void Update() => _stateMachine.Tick();
    }
}