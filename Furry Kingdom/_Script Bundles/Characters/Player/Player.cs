
using UnityEngine;

namespace Furry
{
    public class Player : Character, IChangeAbilities
    {

        public Player()
        {
        }

        protected override void Awake()
        {
            base.Awake();
            Init();
        }

        private void Start()
        {
        }

        public override void Init()
        {
            RegenAmount = _stats.RegenAmount;
            RegenSpeed = _stats.RegenSpeed;

        }

        private void OnTriggerEnter(Collider other)
        {
            other.gameObject.TryGetComponent<IUseAbilities>(out IUseAbilities _useAbilities);
            if (_useAbilities != null)
            {
                _abilityController.SetNewAbility(_useAbilities.GetAbilityName());
            }
        }

        public void ChangeAbility(string abilityName)
        {
            _abilityController.SetNewAbility(abilityName);
        }
    }

}