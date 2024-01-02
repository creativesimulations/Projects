using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Scriptables
{

  [CreateAssetMenu(fileName = "Contagion", menuName = "ScriptableObjects/Contagion")]
  public class ContagionScriptableObject : ScriptableObject
  {
    [Header("Setup stats for the projectile")]
    [Tooltip("Likelyhood of freeroaming, as opposed to moving only in a specific area. 0 means all are free roaming, 1 means none are")]
    [SerializeField, Range(0f, 1f)] public float freeroaming;
    [Tooltip("Name of infection")]
    [SerializeField] public string contagionName;
    [Tooltip("Radius of contractibility. Begins at 0.5, which is the radius of the character")]
    [SerializeField, Range(1f, 10f)] public float infectionRadius;
    [Tooltip("If contagion can be contracted again after getting well")]
    [SerializeField] public bool RepeatInfections;
    [Tooltip("Percentage chance of contraction")]
    [SerializeField, Range(1f, 100f)] public float contagionRate;
    [Tooltip("How long contagion lasts. 1 second = 1 day")]
    [SerializeField, Range(1f, 100f)] public float minContagionDuration;
    [SerializeField, Range(1f, 200f)] public float maxContagionDuration;
    [Tooltip("Time until illness shows")]
    [SerializeField, Range(1f, 100f)] public float minIncubationPeriod;
    [SerializeField, Range(1f, 100f)] public float maxIncubationPeriod;
    [Tooltip("Percentage of chance to die from illness")]
    [SerializeField, Range(0f, 100f)] public float Lethality;
  }
}