using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;

namespace Scriptables
{

    [CreateAssetMenu(fileName = "Contagion Text", menuName = "ScriptableObjects/Contagion Text")]
    public class ContagionTextScriptableObject : ScriptableObject
    {
        [Header("Place this component on the Canvas game object.")]
        [Tooltip("Input the name of the contagion.")]
        [SerializeField] public string _contagionName;
        [Tooltip("Input the total amount of characters wanted in the scene.")]
        [SerializeField] public string _populationSample;
        [Tooltip("Input the percentage rate of contagion.")]
        [SerializeField] public string _contagionRate;
        [Tooltip("Input the percentage of lethality of the contagion.")]
        [SerializeField] public string _lethality;
        [Tooltip("Input the incubation period of the contagion in number of days.")]
        [SerializeField] public string _incubationPeriod;
        [Tooltip("Input the infection of the contagion in number of days.")]
        [SerializeField] public string _infectionPeriod;
    }
}