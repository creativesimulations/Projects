using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scriptables
{


[CreateAssetMenu(fileName = "WeaponScriptable", menuName = "EpicSpace/Weapon")]
public class WeaponScriptable : ScriptableObject
{
    [Header("Setup parameters for the weapon.")]
    [Tooltip("The object that is fired from the weapon.")]
    [SerializeField] public GameObject projectile;
    [Tooltip("The game object with the particle effect to play on shooting.")]
    [SerializeField] public GameObject muzzleFlash;
    [Tooltip("The game object with the Particle effect to play on hit.")]
    [SerializeField] public GameObject explosion;
    [Tooltip("The rate at which the weapon fires.")]
    [SerializeField] public float fireRate;
    [Tooltip("The rate at which the weapon fires bursts.")]
    [SerializeField] public float coolDownRate;
    [Tooltip("The amount of damage inflicted by each charge.")]
    [SerializeField] public float damage;
    [Tooltip("The area of effect.")]
    [SerializeField] public float damageRadius;
}
}