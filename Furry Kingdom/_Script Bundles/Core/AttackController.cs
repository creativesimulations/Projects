using System.Threading.Tasks;
using UnityEngine;

namespace Furry
{

public static class AttackController 
    {
    private static void ActivateAbility(string abilityName)
    {
         //   AbilityFactory.GetAbility(newAbility).Use(gameObject, gameObject);
        }

        public static int DamageAmount(int strength)
        {
            throw new System.NotImplementedException();
        }

        public static async void ChasePlayer(Transform target, Transform attacker, float attackDistance)
        {
            float distance = Vector3.Distance(attacker.position, target.position);
            while (distance > attackDistance)
                {
                    distance = Vector3.Distance(attacker.position, target.position);
                    await Task.Yield();
                    Debug.Log("distance is " + distance);
                }

            Debug.Log("Got you!");
        }
    }

}