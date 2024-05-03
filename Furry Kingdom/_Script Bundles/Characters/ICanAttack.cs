
using UnityEngine;

namespace Furry
{

    public interface ICanAttack
    {
        /// <summary>
        /// Percentage change that the attack will be more effective.
        /// </summary>
        public int PercentCritChance { get; set; }

        /// <summary>
        /// The maximum range to make an attack.
        /// </summary>
        public float AttackRange { get; set; }

        /// <summary>
        /// Attack a target;
        /// </summary>
        /// <param name="attacker"></param> The character that is attacking.
        /// <param name="defender"></param> The character that is being attacked.
        public void Attack(Character attacker, Character defender);

    }

}