using UnityEngine;

namespace Furry
{
    [CreateAssetMenu(fileName = "RegenBuff", menuName = "Furry/Abilities/RegenBuff")]

    public class RegenBuff : Ability
    {

        [SerializeField] private int _additionalRegen = 1;

        public override void Use(GameObject owner, GameObject target)
        {
            Debug.Log("More Premanent Regen!");
        }

    }
}