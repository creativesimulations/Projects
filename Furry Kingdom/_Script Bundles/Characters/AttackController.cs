
using System.Threading.Tasks;
using UnityEngine;

namespace Furry
{

public class AttackController : MonoBehaviour, ICanAttack
    {
    [SerializeField] public float PercentCritChance { get; set; }
        [SerializeField] public float AttackDistance { get; set; }


        private void Awake()
        {
        }

        void Start()
    {
           // _states.OnChase += Chase;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ActivateAbility(string abilityName)
    {
         //   AbilityFactory.GetAbility(newAbility).Use(gameObject, gameObject);
        }

        public int DamageAmount(int strength)
        {
            throw new System.NotImplementedException();
        }

        public void Chase(Transform target)
        {
            ChasePlayer(target);
        }

        public async virtual void ChasePlayer(Transform target)
        {
            float distance = Vector3.Distance(transform.position, target.position);
            while (distance > AttackDistance)
                {
                    distance = Vector3.Distance(transform.position, target.position);
                    await Task.Yield();
                    Debug.Log("distance is " + distance);
                }

            Debug.Log("Got you!");
        }
    }

}