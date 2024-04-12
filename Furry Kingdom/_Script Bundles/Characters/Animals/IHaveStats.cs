
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
    }

}