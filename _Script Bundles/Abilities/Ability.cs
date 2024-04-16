using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace Furry
{

    public abstract class Ability : ScriptableObject
    {
        [SerializeField] public string _name = "Default Name";
        [SerializeField] public int _reuseDelay = 3;
        public bool _coolingDown;

        public abstract void Use(GameObject owner, GameObject target);
        public void UseWithCooldown(GameObject owner, GameObject target)
        {
            if (!_coolingDown)
            {
                ActivateWithCostAndReuse(owner, target);
            }
        }
        private async void ActivateWithCostAndReuse(GameObject owner, GameObject target)
        {
            Use(owner, target);
            _coolingDown = true;
            await Task.Delay(_reuseDelay * 1000);
            _coolingDown = false;
        }

    }

}