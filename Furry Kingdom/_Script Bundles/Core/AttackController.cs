using System.Threading.Tasks;
using UnityEngine;

// NOT IMPLEMENTED YET ***

namespace Furry
{

    public static class AttackController
    {
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
            }
        }
    }

}