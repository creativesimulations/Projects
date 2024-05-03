using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Furry
{
    [CreateAssetMenu(fileName = "HealthBuff", menuName = "Furry/Abilities/HealthBuff")]

    public class HealthBuff : Ability
    {

        [Header("Health fields.")]
        [Tooltip("The amount of extra health that will be gained.")]
        [SerializeField] public uint ExtraHealth = 100;

        /// <summary>
        /// This activates the ability. NOT FINSIHED ***
        /// </summary>
        /// <param name="owner"></param> The object using the ability.
        /// <param name="target"></param> The object the ability is to be used on.
        public override void Use(GameObject owner, GameObject target)
        {
        }

    }
}