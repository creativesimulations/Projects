
namespace Furry
{

    public interface IUseAbilities
    {
        void Use(string ability);
        string GetAbilityName();
        void CheckImmunity(string ability);
    }

}