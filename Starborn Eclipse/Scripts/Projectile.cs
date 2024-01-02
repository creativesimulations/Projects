using Core;
using Scriptables;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapon
{


public class Projectile : MonoBehaviour
    {
        [SerializeField] private ProjectileScriptable _projectileScriptable;
        [Tooltip("The radisu of the explosion.")]
        [SerializeField] private float _radius = 5.0F;

        // Start is called before the first frame update
        void Start()
    {
        GetComponent<SphereCollider>().radius = _radius;
    }

    // Update is called once per frame
    void Update()
    {
             transform.Translate(_projectileScriptable.movementSpeed * Time.deltaTime * 0, 0, 1, gameObject.transform);
            
        }
        private void OnCollisionEnter(Collision other)
        {
            Explode();
        }
        public void Explode()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, _radius);
            PlayParticle();

            foreach (Collider hit in colliders)
            {
                if (hit.CompareTag("Explodable"))
                {
                    Rigidbody rb = hit.GetComponent<Rigidbody>();
                    if (rb.isKinematic)
                    {
                        rb.isKinematic = false;
                    }
                    hit.GetComponent<Rigidbody>().AddExplosionForce(_projectileScriptable._power, transform.position, _radius, _projectileScriptable._pushUp);
                }
            }
            Destroy(gameObject);
        }

        private void PlayParticle()
        {
            ObjectPooler oP = Singleton.instance.GetComponent<ObjectPooler>();
            var particle = oP.PopFromPool(oP.GetObjectInExplosionsDictionary(_projectileScriptable.explosion.name), false, true, oP.GetTransformInParentsDictionary(_projectileScriptable.explosion.name), false);
            particle.transform.position = transform.position;
            particle.SetActive(true);
        }
    }
}
