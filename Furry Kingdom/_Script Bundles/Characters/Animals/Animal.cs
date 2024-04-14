using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace Furry
{

    [RequireComponent(typeof(NavMeshMovementNPC))]
    public class Animal : Character
    {
        [Tooltip("The maximum time the animal will idle in one location.")]
        [SerializeField] protected float _maxIdleTime;
        [Tooltip("The radius within which the animal will choose a new location to walk to.")]
        [SerializeField] protected float _walkRadius;
        [Tooltip("The radius at which the aminal will run from the player and also the distance it will run from the player.")]
        [SerializeField] protected float _runRange;

        protected ParticleSystem _runParticleSystem;
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