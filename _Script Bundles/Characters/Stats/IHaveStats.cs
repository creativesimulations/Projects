
namespace Furry
{

    public interface IHaveStats
    {
        // Affects damage
        int Strength { get; set; }
        // Affects hp
        int Constitution { get; set; }
        // Affects movement speed
        int Stamina { get; set; }
        // Affects attack speed
        int Agility { get; set; }


        public int ModifyStrength(int modifyAmount)
        {
            return Strength += modifyAmount;
        }
        public int ModifyConstitution(int modifyAmount)
        {
            return Constitution += modifyAmount;
        }
        public int ModifyStamina(int modifyAmount)
        {
            return Stamina += modifyAmount;
        }
        public int ModifyAgility(int modifyAmount)
        {
            return Agility += modifyAmount;
        }

    }

}