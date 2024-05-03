using UnityEngine;

namespace Furry
{

    [CreateAssetMenu(fileName = "Stats", menuName = "Furry/Characters/Stats")]
    public class StatsScriptable : ScriptableObject
    {
        [Header("Stats Fields")]
        [Tooltip("Effects damage amount.")]
        [SerializeField] public int _strength;
        [Tooltip("Effects maximum health amount.")]
        [SerializeField] public int _constitution;
        [Tooltip("Effects attack resistance.")]
        [SerializeField] public int _stamina;
        [Tooltip("Effects attack speed.")]
        [SerializeField] public int _agility;
        [Tooltip("Effects movement speed.")]
        [SerializeField] public int _speed;
        [Tooltip("Effects regengeration amount per second.")]
        [SerializeField] public int _regenAmount;
        [Tooltip("Effects regeneration speed over time.")]
        [SerializeField] public float _regenSpeed;

        /// <summary>
        /// Decrease an amount from the strength stat.
        /// </summary>
        /// <param name="Amount"></param> the amount to decrease from the strength stat.
        public void DecreaseStrength(int Amount)
        {
        }

        /// <summary>
        /// Decrease an amount from the constitution stat.
        /// </summary>
        /// <param name="Amount"></param> the amount to decrease from the constitution stat.
        public void DecreaseConstitution(int Amount)
        {
        }

        /// <summary>
        /// Decrease an amount from the stamina stat.
        /// </summary>
        /// <param name="Amount"></param> the amount to decrease from the stamina stat.
        public void DecreaseStamina(int Amount)
        {
        }

        /// <summary>
        /// Decrease an amount from the agility stat.
        /// </summary>
        /// <param name="Amount"></param> the amount to decrease from the agility stat.
        public void DecreaseAgility(int Amount)
        {
        }

        /// <summary>
        /// Decrease an amount from the speed stat.
        /// </summary>
        /// <param name="Amount"></param> the amount to decrease from the speed stat.
        public void DecreaseSpeed(int Amount)
        {
        }
        /// <summary>
        /// Decrease an amount from the regeneration stat.
        /// </summary>
        /// <param name="Amount"></param> the amount to decrease from the regeneration stat.
        public void DecreaseRegenAmount(int Amount)
        {
        }

        /// <summary>
        /// Decrease an amount from the regeneration speed stat.
        /// </summary>
        /// <param name="Amount"></param> the amount to decrease from the regeneration speed stat.
        public void DecreaseRegenSpeed(float Amount)
        {
        }



        /// <summary>
        /// Increase an amount from the strength stat.
        /// </summary>
        /// <param name="Amount"></param> the amount to increase from the strength stat.
        public void IncreaseStrength(int Amount)
        {
        }

        /// <summary>
        /// Increase an amount from the constitution stat.
        /// </summary>
        /// <param name="Amount"></param> the amount to increase from the constitution stat.
        public void IncreaseConstitution(int Amount)
        {
        }

        /// <summary>
        /// Increase an amount from the stamina stat.
        /// </summary>
        /// <param name="Amount"></param> the amount to increase from the stamina stat.
        public void IncreaseStamina(int Amount)
        {
        }

        /// <summary>
        /// Increase an amount from the agility stat.
        /// </summary>
        /// <param name="Amount"></param> the amount to increase from the agility stat.
        public void IncreaseAgility(int Amount)
        {
        }

        /// <summary>
        /// Increase an amount from the speed stat.
        /// </summary>
        /// <param name="Amount"></param> the amount to increase from the speed stat.
        public void IncreaseSpeed(int Amount)
        {
        }

        /// <summary>
        /// Increase an amount from the regeneration stat.
        /// </summary>
        /// <param name="Amount"></param> the amount to increase from the regeneration stat.
        public void IncreaseRegenAmount(int Amount)
        {
        }

        /// <summary>
        /// Increase an amount from the regeneration speed stat.
        /// </summary>
        /// <param name="Amount"></param> the amount to increase from the regeneration speed stat.
        public void IncreaseRegenSpeed(float Amount)
        {
        }

    }

}