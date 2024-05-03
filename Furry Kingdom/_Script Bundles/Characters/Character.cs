using UnityEngine;

namespace Furry
{

    public abstract class Character : MonoBehaviour, IUseAbilities, IHaveStats
    {
        [Header("Stats field.")]
        [Tooltip("Stats scriptable object.")]
        [SerializeField] protected StatsScriptable _stats;
        [Tooltip("Ability controller which holds references to the abilites for the character.")]
        protected AbilityController _abilityController;

        public int Strength { get; set; }
        public int Constitution { get; set; }
        public int Stamina { get; set; }
        public int Agility { get; set; }
        public int Speed { get; set; }
        public int RegenAmount { get; set; }
        public float RegenSpeed { get; set; }

        protected virtual void Awake()
        {
            Init();
        }

        public virtual void Init()
        {
            InitializeAbilityController();
            InitializeStatsFields();
        }

        /// <summary>
        /// Set the stats as the stats scriptable object serialized fields.
        /// </summary>
        public void InitializeStatsFields()
        {
            Strength = _stats._strength;
            Constitution = _stats._constitution;
            Stamina = _stats._stamina;
            Agility = _stats._agility;
            Speed = _stats._speed;
            RegenAmount = _stats._regenAmount;
            RegenSpeed = _stats._regenSpeed;
        }

        /// <summary>
        /// Increase the strength stat by a specirfic amount.
        /// </summary>
        /// <param name="modifyAmount"></param> Amount to increase by.
        public void IncreaseStrength(int modifyAmount)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Increase the constitution stat by a specific amount.
        /// </summary>
        /// <param name="modifyAmount"></param> Amount to increase by.
        public void IncreaseConstitution(int modifyAmount)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Increase the stamina stat by a specific amount.
        /// </summary>
        /// <param name="modifyAmount"></param> Amount to increase by.
        public void IncreaseStamina(int modifyAmount)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Increase the agility stat by a specific amount.
        /// </summary>
        /// <param name="modifyAmount"></param> Amount to increase by.
        public void IncreaseAgility(int modifyAmount)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Increase the speed stat by a specific amount.
        /// </summary>
        /// <param name="modifyAmount"></param> Amount to increase by.
        public void IncreaseSpeed(int modifyAmount)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Increase the regeneration stat by a specific amount.
        /// </summary>
        /// <param name="modifyAmount"></param> Amount to increase by.
        public void IncreaseRegenAmount(int modifyAmount)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Increase the regeneration speed stat by a specific amount.
        /// </summary>
        /// <param name="modifyAmount"></param> Amount to increase by.
        public void IncreaseRegenSpeed(float modifyAmount)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Decrease the regeneration speed stat by a specific amount.
        /// </summary>
        /// <param name="modifyAmount"></param> Amount to decrease by.
        public void DecreaseStrength(int modifyAmount)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Decrease the consitution stat by a specific amount.
        /// </summary>
        /// <param name="modifyAmount"></param> Amount to decrease by.
        public void DecreaseConstitution(int modifyAmount)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Decrease the stamina stat by a specific amount.
        /// </summary>
        /// <param name="modifyAmount"></param> Amount to decrease by.
        public void DecreaseStamina(int modifyAmount)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Decrease the agility stat by a specific amount.
        /// </summary>
        /// <param name="modifyAmount"></param> Amount to decrease by.
        public void DecreaseAgility(int modifyAmount)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Decrease the speed stat by a specific amount.
        /// </summary>
        /// <param name="modifyAmount"></param> Amount to decrease by.
        public void DecreaseSpeed(int modifyAmount)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Decrease the regeneration stat by a specific amount.
        /// </summary>
        /// <param name="modifyAmount"></param> Amount to decrease by.
        public void DecreaseRegenAmount(int modifyAmount)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Decrease the regeneration speed stat by a specific amount.
        /// </summary>
        /// <param name="modifyAmount"></param> Amount to decrease by.
        public void DecreaseRegenSpeed(float modifyAmount)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Get current ability name.
        /// </summary>
        /// <returns></returns>
        public string GetAbilityName()
        {
            return _abilityController.GetAbilityName();
        }

        /// <summary>
        /// Returns whether the character is immune to the ability.
        /// </summary>
        /// <param name="ability"></param> Ability name to check.
        /// <returns></returns>
        public bool CheckImmunity(string ability)
        {
            return _abilityController.CheckImmunity(ability);
        }

        /// <summary>
        /// Sets the ability controller on the gameobject so that it can be referenced.
        /// </summary>
        public void InitializeAbilityController()
        {
            _abilityController = GetComponent<AbilityController>();
        }

        public void Use()
        {
            throw new System.NotImplementedException();
        }
    }
}