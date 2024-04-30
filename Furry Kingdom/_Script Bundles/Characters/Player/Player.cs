
using UnityEngine;

namespace Furry
{
    public class Player : Character, IChangeAbilities
    {
        [SerializeField] private Material _flagMat;

        protected override void Awake()
        {
            base.Awake();
            Init();
        }

        public override void Init()
        {
            RegenAmount = _stats.RegenAmount;
            RegenSpeed = _stats.RegenSpeed;
        }

        private void OnCollisionEnter(Collision collision)
        {
            TileManager tM;
            tM = collision.gameObject.GetComponentInParent<TileManager>();
            if (tM != null)
            {
                tM.ChangeFlag(_flagMat);
            }
        }

        public void ChangeAbility(string abilityName)
        {
            _abilityController.SetNewAbility(abilityName);
        }
    }

}