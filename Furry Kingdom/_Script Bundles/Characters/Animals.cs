using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace Furry
{

    [RequireComponent(typeof(NavMeshMovementNPC))]
    public class Animals : Character
    {
        [SerializeField] protected float _idleTime;
        [SerializeField] protected float _moseyRange;
        [SerializeField] protected float _runRange;

        [SerializeField] protected ParticleSystem _runParticleSystem;
        protected NavMeshMovementNPC _navMeshMovement;

        protected override void Awake()
        {
            base.Awake();
            _runParticleSystem = GetComponentInChildren<ParticleSystem>();
            _navMeshMovement = GetComponent<NavMeshMovementNPC>();
        }

        protected virtual void Start()
        {
            _navMeshMovement.SetMovementSpeed(Speed);
        }

    }
}