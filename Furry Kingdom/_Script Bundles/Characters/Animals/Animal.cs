using UnityEngine;

namespace Furry
{

    [RequireComponent(typeof(NavMeshMovementNPC))]
    public abstract class Animal : Character
    {
        [Tooltip("The maximum time the animal will idle in one location.")]
        [SerializeField] protected float _maxIdleTime;
        [Tooltip("The radius within which the animal will choose a new location to walk to.")]
        [SerializeField] protected float _walkRadius;
        [Tooltip("The radius at which the aminal will run from the player and also the distance it will run from the player.")]
        [SerializeField] protected float _runRange;
        [Tooltip("The inactive time after an animal chases or runs from a player.")]
        [SerializeField] protected float _coolDownTime;

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