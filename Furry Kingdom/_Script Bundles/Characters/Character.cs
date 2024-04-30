using System.Threading.Tasks;
using UnityEngine;

namespace Furry
{

    [RequireComponent(typeof(AbilityController))]
    public abstract class Character : MonoBehaviour, IHaveStats, IUseAbilities, ICanRegen
    {
        public int MaxHealth { get; set; }
        public int Strength { get; set; }
        public int Constitution { get; set; }
        public int Stamina { get; set; }
        public int Agility { get; set; }
        public float Speed { get; set; }

        public int CurrentHealth { get; set; }
        public int RegenAmount { get; set; }
        public float RegenSpeed { get; set; }

        [SerializeField] protected HealthBar HealthBar;

        [SerializeField] protected StatsScriptable _stats;

        protected AbilityController _abilityController;

        protected virtual void Awake()
        {
            HealthBar = GetComponentInChildren<HealthBar>();
            _abilityController = GetComponent<AbilityController>();
            Init();
        }

        public virtual void Init()
        {
            MaxHealth = _stats.MaxHealth;
            Strength = _stats.Strength;
            Constitution = _stats.Constitution;
            Stamina = _stats.Stamina;
            Agility = _stats.Agility;
            Speed = _stats.Speed;
            CurrentHealth = MaxHealth;
            RegenAmount = _stats.RegenAmount;
            RegenSpeed = _stats.RegenSpeed;
        }

        public virtual float ModifySpeed(float newSpeed)
        {
            return Speed = newSpeed;
        }

        public virtual int ModifyMaxHealth(int amount)
        {
            return MaxHealth += amount;
        }

        public virtual void ModifyCurrentHealth(int amount)
        {
            CurrentHealth += amount;
            HealthBar.UpdateHealthBar(CurrentHealth, MaxHealth);
        }

        public virtual float ModifyRegenAmount(float amount)
        {
            throw new System.NotImplementedException();
        }


        public void Use(string ability)
        {
            throw new System.NotImplementedException();
        }

        public string GetAbilityName()
        {
            throw new System.NotImplementedException();
        }

        public void CheckImmunity(string ability)
        {
            if (_abilityController.ImmuneToAbilitiesDictionary.ContainsKey(ability))
            {
                // run another method to activate ability.
            }
        }
    }
}