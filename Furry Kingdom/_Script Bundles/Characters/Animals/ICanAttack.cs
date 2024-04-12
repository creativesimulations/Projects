
using UnityEngine;

namespace Furry
{

    public interface ICanAttack
    {
        [SerializeField] float PercentCritChance { get; set; }
        [SerializeField] float AttackDistance { get; set; }
        int DamageAmount(int strength);
        void Chase(Transform target);
    }

}