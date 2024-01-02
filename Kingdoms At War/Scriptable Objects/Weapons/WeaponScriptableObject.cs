using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scriptables
{

    [CreateAssetMenu(fileName = "Weapons", menuName = "ScriptableObjects/Weapons for Team Sims")]
    public class WeaponScriptableObject : ScriptableObject
    {

        public new string name;
        public string description;

        [Header("Setup for the weapon stats")]
        [Tooltip("Maximum bottom angle it can pivot to")]
        public int bottomAngle = 45;
        [Tooltip("Maximum top angle it can pivot to")]
        public int topAngle = 25;
        [Tooltip("Delay time between shots")]
        public float timeBetweenShots = 5f;
        [Tooltip("Explosive radius")]
        public float explosionRadius = 2f;
        [Tooltip("Amount of upward force exerted on rigid bodies in the explosive radius")]
        public float explosionUpwardsForce = 1f;
        [Tooltip("Amount of force exerted on rigid bodies in the explosive radius")]
        public float explosionForce = 20f;

        [Tooltip("Prefab of projectile to be used by this team")]
        public GameObject projectileToSpawn;
        [Tooltip("Initial number of projectile to pool")]
        [Range(0, 1000)] public int projectileInitialSpawnCount = 0;
    }
}