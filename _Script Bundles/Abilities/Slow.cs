using UnityEngine;

namespace Furry
{
    [CreateAssetMenu(fileName = "Slow", menuName = "Furry/Abilities/Slow")]

    public class Slow : Ability
    {

        [SerializeField, Range(1, 5f)] uint _slowPercent = 3;

        public override void Use(GameObject owner, GameObject target)
        {
            Debug.Log("Slow target ability!");
        }

    }
}