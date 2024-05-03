using UnityEngine;

namespace Furry
{
    [CreateAssetMenu(fileName = "Poison", menuName = "Furry/Abilities/Poison")]

    public class Poison : Ability
    {
        [Header("Poision fields.")]
        [Tooltip("The amount of damage the poision will deal per second.")]
        [SerializeField, Range(1f, 10f)] int _dmgAmountPerSecond = 2;
        [Tooltip("The duration of the poison.")]
        [SerializeField, Range(1, 10f)] int _poisonDuration = 2;

        /// <summary>
        /// This activates the ability. NOT FINSIHED ***
        /// </summary>
        /// <param name="owner"></param> The object using the ability.
        /// <param name="target"></param> The object the ability is to be used on.
        public override void Use(GameObject owner, GameObject target)
        {
            Debug.Log(" Poison!");
        }

    }
}