
namespace Furry
{
    public interface IUseAbilities
    {

        /// <summary>
        /// Initialize the ability controller.
        /// </summary>
        void InitializeAbilityController();

        /// <summary>
        /// Use the ability.
        /// </summary>
        /// <param name="ability"></param> The name of the ability to use.
        void Use();

        /// <summary>
        /// Returns the name of an ability.
        /// </summary>
        /// <returns></returns>
        string GetAbilityName();

        /// <summary>
        /// Check if an ability is in the immune dictionary.
        /// </summary>
        /// <param name="ability"></param> The name of the ability to check.
        bool CheckImmunity(string ability);
    }

}