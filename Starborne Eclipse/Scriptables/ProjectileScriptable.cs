using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scriptables
{


    [CreateAssetMenu(fileName = "ProjectileScriptable", menuName = "EpicSpace/Projectile")]
    public class ProjectileScriptable : ScriptableObject
    {
        [Header("Setup parameters for the projectile.")]
        [Tooltip("The speed the projectile moves at.")]
        [SerializeField] public float movementSpeed;
        [Tooltip("The amount of damage inflicted.")]
        [SerializeField] public float damage;
        [Tooltip("The area of damage effect.")]
        [SerializeField] public float damageRadius;
        [Tooltip("The power of the explosion.")]
        [SerializeField] public float _power = 400.0F;
        [Tooltip("The upward force of the explosion.")]
        [SerializeField] public float _pushUp = 4.0F;
        [Tooltip("The game object with the Particle effect to play on hit.")]
        [SerializeField] public GameObject explosion;
    }
}