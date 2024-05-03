
namespace Furry
{

    public interface IHaveStats
    {
        // Damage amount
        int Strength { get; set; }
        // Maximum health
        int Constitution { get; set; }
        // Attack Resistance
        int Stamina { get; set; }
        // Attack speed
        int Agility { get; set; }
        // Movement speed
        int Speed { get; set; }
        // Regeneration amount
        int RegenAmount { get; set; }
        // Regeneration speed
        float RegenSpeed { get; set; }

        void InitializeStatsFields();

        void IncreaseStrength(int modifyAmount);
        void IncreaseConstitution(int modifyAmount);
        void IncreaseStamina(int modifyAmount);
        void IncreaseAgility(int modifyAmount);
        void IncreaseSpeed(int modifyAmount);
        void IncreaseRegenAmount(int modifyAmount);
        void IncreaseRegenSpeed(float modifyAmount);

        void DecreaseStrength(int modifyAmount);
        void DecreaseConstitution(int modifyAmount);
        void DecreaseStamina(int modifyAmount);
        void DecreaseAgility(int modifyAmount);
        void DecreaseSpeed(int modifyAmount);
        void DecreaseRegenAmount(int modifyAmount);
        void DecreaseRegenSpeed(float modifyAmount);

    }

}