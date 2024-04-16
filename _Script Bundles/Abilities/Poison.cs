using UnityEngine;

namespace Furry
{
    [CreateAssetMenu(fileName = "Poison", menuName = "Furry/Abilities/Poison")]

    public class Poison : Ability
    {

        [SerializeField, Range(0.1f, 2f)] float _sizePercent = .5f;

        public override void Use(GameObject owner, GameObject target)
        {
            Debug.Log(" Poison!");
        }

    }
}