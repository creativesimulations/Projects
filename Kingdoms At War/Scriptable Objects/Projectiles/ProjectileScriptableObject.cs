using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scriptables
{

    [CreateAssetMenu(fileName = "Projectiles", menuName = "ScriptableObjects/Projectile")]
    public class ProjectileScriptableObject : ScriptableObject
    {
        [Header("Setup stats for the projectile")]
        [Tooltip("Explosive radius")]
        public float hitRadius = 2f;
        [Tooltip("Amount of upward force exerted on rigid bodies in the explosive radius")]
        public float hitUpwardsForce = 1f;
        [Tooltip("Amount of force exerted on rigid bodies in the explosive radius")]
        public float hitForce = 20f;
        [Tooltip("How much damage a direct hit causes to the target")]
        public int hitnDamage = 100;
        [Tooltip("Falloff ratio of damage and explosive force done to nearby objects")]
        public float explosionDamageFalloff = 1f;

        [HideInInspector] public int team;

       // [Tooltip("Prefab of projectile to be used by this team")]
       // public GameObject projectileToSpawn;
       // [Tooltip("Initial number of projectile to pool")]
       // [Range(0, 1000)] public int projectileInitialSpawnCount = 0;
    }
}