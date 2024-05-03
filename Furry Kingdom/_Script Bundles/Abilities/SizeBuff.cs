using UnityEngine;

namespace Furry
{
    [CreateAssetMenu(fileName = "SizeBuff", menuName = "Furry/Abilities/Resize")]

    public class SizeBuff : Ability
    {

        [Header("Size fields.")]
        [Tooltip("The percentage of size that will be changed.")]
        [SerializeField, Range(0.1f, 5f)] float _sizePercent = .5f;

        /// <summary>
        /// This activates the ability. NOT FINSIHED ***
        /// </summary>
        /// <param name="owner"></param> The object using the ability.
        /// <param name="target"></param> The object the ability is to be used on.
        public override void Use(GameObject owner, GameObject target)
        {
            // target.transform.localScale = Vector3.one * _sizePercent;
            throw new System.NotImplementedException();
        }

    }
}