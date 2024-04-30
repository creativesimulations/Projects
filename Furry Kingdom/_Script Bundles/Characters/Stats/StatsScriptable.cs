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
        public int CurrentHealth { get; private set; }

        public void ModifyMaxHealth(int amount)
        {
            MaxHealth += amount;
        }
        public void ModifyCurrentHealth(bool add, int amount)
        {
            if (add)
            {
                CurrentHealth += amount;
            }
            else
            {
                CurrentHealth -= amount;
            }
        }

public void ModifyStrength(bool add, int amount)
        {
            if (add)
            {
                Strength += amount;
            }
            else
            {
                Strength -= amount;
            }
        }

        public void ModifyConstitution(bool add, int amount)
        {
            if (add)
            {
                Constitution += amount;
            }
            else
            {
                Constitution -= amount;
            }
        }

        public void ModifyStamina(bool add, int amount)
        {
            if (add)
            {
                Stamina += amount;
            }
            else
            {
                Stamina -= amount;
            }
        }

        public void ModifyAgility(bool add, int amount)
        {
            if (add)
            {
                Agility += amount;
            }
            else
            {
                Agility -= amount;
            }
        }

        public void ModifyRegenSpeed(bool add, float amount)
        {
            if (add)
            {
                RegenSpeed += amount;
            }
            else
            {
                RegenSpeed -= amount;
            }
        }

        public void ModifyRegenAmount(bool add, int amount)
        {
            if (add)
            {
                RegenAmount += amount;
            }
            else
            {
                RegenAmount -= amount;
            }
        }
        public void ModifySpeed(bool add, int amount)
        {
            if (add)
            {
                RegenAmount += amount;
            }
            else
            {
                RegenAmount -= amount;
            }
        }

    }

}