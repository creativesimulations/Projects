using UnityEngine;

namespace Furry
{
    public class Player : Character, IChangeAbilities, ICanAttack
    {
        [Header("Player fields")]
        [Tooltip("Flag material.")]
        [SerializeField] private Material _flagMat;
        [Tooltip("Maximum possible range to attack.")]
        [SerializeField] private float _attackRange;
        [Tooltip("Percentage change of adding effectiveness to an attack.")]
        [SerializeField] private int _percentCritChance;

        public float AttackRange
        {
            get { return _attackRange; }
            set { _attackRange = value; }
        }
        public int PercentCritChance
        {
            get { return _percentCritChance; }
            set { _percentCritChance = value; }
        }

        protected override void Awake()
        {
            base.Awake();
        }


        private void OnCollisionEnter(Collision collision)
        {
            SetFlag(collision);
        }

        /// <summary>
        /// Sets tile flag as players color if it is a different color.
        /// </summary>
        /// <param name="collision"></param>
        private void SetFlag(Collision collision)
        {
            TileManager tM;
            tM = collision.gameObject.GetComponentInParent<TileManager>();
            if (tM != null)
            {
                tM.ChangeFlag(_flagMat);
            }
        }

        public void ChangeAbility(string abilityName)
        {
            _abilityController.SetNewAbility(abilityName);
        }

        /// <summary>
        /// Initializes the attack sequence.
        /// </summary>
        /// <param name="attacker"></param> Attacker.
        /// <param name="defender"></param> Defender.
        /// <exception cref="System.NotImplementedException"></exception>
        public void Attack(Character attacker, Character defender)
        {
            // THIS IS NOT IMPLEMENTED YET  ***
        }
    }

}