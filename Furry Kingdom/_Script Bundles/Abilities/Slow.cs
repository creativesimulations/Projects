using UnityEngine;

namespace Furry
{
    [CreateAssetMenu(fileName = "Slow", menuName = "Furry/Abilities/Slow")]

    public class Slow : Ability
    {

        [Header("Slow fields.")]
        [Tooltip("The percentage of speed that will be reduced.")]
        [SerializeField, Range(1, 15f)] uint _slowPercent = 3;

        /// <summary>
        /// This activates the ability. NOT FINSIHED ***
        /// </summary>
        /// <param name="owner"></param> The object using the ability.
        /// <param name="target"></param> The object the ability is to be used on.
        public override void Use(GameObject owner, GameObject target)
        {
            Debug.Log("Slow target ability!");
        }

    }
}