using UnityEngine;

namespace Furry
{
    [CreateAssetMenu(fileName = "RegenBuff", menuName = "Furry/Abilities/RegenBuff")]

    public class RegenBuff : Ability
    {

        [Header("Regeneration fields.")]
        [Tooltip("The amount of additional regeneration that will be gained.")]
        [SerializeField] private int _additionalRegen = 1;

        /// <summary>
        /// This activates the ability. NOT FINSIHED ***
        /// </summary>
        /// <param name="owner"></param> The object using the ability.
        /// <param name="target"></param> The object the ability is to be used on.
        public override void Use(GameObject owner, GameObject target)
        {
            Debug.Log("More Premanent Regen!");
        }

    }
}