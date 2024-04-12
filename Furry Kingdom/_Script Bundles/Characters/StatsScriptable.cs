using UnityEngine;

namespace Furry
{

    [CreateAssetMenu(fileName = "Stats", menuName = "Furry/Characters/Stats")]
    public class StatsScriptable : ScriptableObject
    {
        [SerializeField] public int MaxHealth;
        [SerializeField] public int Strength;
        [SerializeField] public int Constitution;
        [SerializeField] public int Stamina;
        [SerializeField] public int Agility;
        [SerializeField] public int Speed;
        [SerializeField] public int RegenAmount;
        [SerializeField] public float RegenSpeed;

        public void ModifyMaxHealth(int amount)
        {
            MaxHealth += amount;
        }

        public void ModifyStrength(int amount)
        {
            Strength += amount;
        }

        public void ModifyConstitution(int amount)
        {
            Constitution += amount;
        }

        public void ModifyStamina(int amount)
        {
            Stamina += amount;
        }

        public void ModifyAgility(int amount)
        {
            Agility += amount;
        }

        public void ModifyRegenSpeed(float amount)
        {
            RegenSpeed += amount;
        }

        public void ModifyRegenAmount(int amount)
        {
            RegenAmount += amount;
        }
        public void ModifySpeed(int amount)
        {
            RegenAmount += amount;
        }

    }

}