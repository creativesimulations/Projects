
namespace Furry
{

    public interface IHaveHealth
{

        int MaxHealth { get; set; }
        int CurrentHealth { get; set; }

        void ModifyMaxHealth(int amount);
        void ModifyCurrentHealth(int amount);
    }

}