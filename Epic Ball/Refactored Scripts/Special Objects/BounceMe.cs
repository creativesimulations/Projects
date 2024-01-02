using UnityEngine;

namespace not
{
    public class BounceMe : MonoBehaviour
    {

        [SerializeField] private float pushUp = 1.0F;
        private Rigidbody rb;

        void Start()
        {
            rb = GetComponent<Rigidbody>();

        }

        private void OnCollisionEnter(Collision other)
        {
            if (!other.gameObject.CompareTag("Untagged"))
            {
                ContactPoint contact = other.contacts[0];
                Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
                Vector3 pos = contact.point;
                Bounce(gameObject);
            }
        }

        private void Bounce(GameObject other)
        {
            Vector3 explosionPos = other.transform.position;
            rb.AddExplosionForce(Random.Range(200, 500), explosionPos, 0, pushUp);
        }
    }
}