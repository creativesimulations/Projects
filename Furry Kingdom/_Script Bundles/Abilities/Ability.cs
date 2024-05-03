using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace Furry
{
    /// <summary>
    /// This is the base abstract class for each ability.
    /// </summary>
    public abstract class Ability : ScriptableObject
    {
        [Header("Ability Fields")]
        [Tooltip("Ability Name")]
        [SerializeField] public string _name = "Default Name";
        [Tooltip("Delay required between uses")]
        [SerializeField] public int _reuseDelay = 3;

        public bool _coolingDown;

        /// <summary>
        /// Use the ability.
        /// </summary>
        /// <param name="owner"></param> The object using the ability.
        /// <param name="target"></param> The object the ability is to be used on.
        public abstract void Use(GameObject owner, GameObject target);

        /// <summary>
        /// 
        /// Use the ability with a cooldown delay.
        /// </summary>
        /// <param name="owner"></param> The object using the ability.
        /// <param name="target"></param> The object the ability is to be used on.
        public void UseWithCooldown(GameObject owner, GameObject target)
        {
            if (!_coolingDown)
            {
                ActivateWithCostAndReuse(owner, target);
            }
        }

        /// <summary>
        /// Use the ability with a cost to use it. THIS METHOD IS NOT FINISHED. NO COST IS IMPLEMENTED YET. ***
        /// </summary>
        /// <param name="owner"></param> The object using the ability.
        /// <param name="target"></param> The object the ability is to be used on.
        private async void ActivateWithCostAndReuse(GameObject owner, GameObject target)
        {
            Use(owner, target);
            _coolingDown = true;
            await Task.Delay(_reuseDelay * 1000);
            _coolingDown = false;
        }

    }

}