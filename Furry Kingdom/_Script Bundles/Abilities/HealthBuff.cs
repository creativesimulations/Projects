using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Furry
{
    [CreateAssetMenu(fileName = "HealthBuff", menuName = "Furry/Abilities/HealthBuff")]

    public class HealthBuff : Ability
    {

        [SerializeField] public uint ExtraHealth = 100;

        public override void Use(GameObject owner, GameObject target)
        {
        }

    }
}