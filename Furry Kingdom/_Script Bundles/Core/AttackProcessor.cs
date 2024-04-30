
namespace Furry
{

public static class AttackProcessor
{

        public static int CalculateAttackAmount(IHaveStats attacker)
        {
            return attacker.Strength + 1;
        }
        public static void ProcessMelee(IHaveStats attacker, Character target)
        {
            int amount = CalculateAttackAmount(attacker);

        }
        public static void ProcessAttack(Character target, int amount)
        {
            target.ModifyCurrentHealth(amount);
        }

        /*
        public static bool IsImmuneToAbility(IHaveAbilityImmunities target, string abilityName)
        {
            return target.CheckImmunity(abilityName);
        }
        */
    }

}