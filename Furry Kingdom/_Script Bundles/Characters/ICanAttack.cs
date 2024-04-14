
using UnityEngine;

namespace Furry
{

    public interface ICanAttack
    {
        public int PercentCritChance { get; set; }
        public float AttackRange { get; set; }
        public void Attack(Player player, Animal animal);

    }

}