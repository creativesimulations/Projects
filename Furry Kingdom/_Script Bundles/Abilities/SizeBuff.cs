using UnityEngine;

namespace Furry
{
    [CreateAssetMenu(fileName = "SizeBuff", menuName = "Furry/Abilities/Resize")]

    public class SizeBuff : Ability
    {

        [SerializeField, Range(0.1f, 2f)] float _sizePercent = .5f;

        public override void Use(GameObject owner, GameObject target)
        {
            // target.transform.localScale = Vector3.one * _sizePercent;
            throw new System.NotImplementedException();
        }

    }
}