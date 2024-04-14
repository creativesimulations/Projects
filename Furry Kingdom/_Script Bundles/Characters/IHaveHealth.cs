
namespace Furry
{

    public interface IHaveHealth
{

        int MaxHealth { get; set; }
        int CurrentHealth { get; set; }

        int ModifyMaxHealth(int amount);
        int ModifyCurrentHealth(int amount);
    }

}