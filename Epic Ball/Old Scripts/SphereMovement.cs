using System.Collections;
using UnityEngine;

    public class SphereMovement : MonoBehaviour
    {
        [SerializeField] private float desiredSpeed = 10f;
        [SerializeField] private float maximumDrag = .5f;
        [SerializeField] private float forceConstant = 10f;

        private Rigidbody rigidbody;
        public bool activate;
    

        private void Start()
        {
        rigidbody = GetComponent<Rigidbody>();
    }
    
        private void Move(Vector3 moveDirection)
        {
                rigidbody.drag = Mathf.Lerp(maximumDrag, 0, moveDirection.magnitude);
                float forceMultiplier = Mathf.Clamp01((desiredSpeed - rigidbody.velocity.magnitude) / desiredSpeed);
                rigidbody.AddForce(moveDirection * (forceMultiplier * Time.deltaTime * forceConstant));
        }
    
     public IEnumerator ChasePlayer(Ball ball)
        {
        while (activate == true && ball.isAlive && !ball.win)
        {
            if ((Physics.Raycast(transform.position, -Vector3.up, transform.localScale.z)))
            {
            Vector3 direction = ball.gameObject.transform.position - transform.position;
            direction = direction.normalized;
            Move(direction);
            }
            yield return new WaitForEndOfFrame();
        }
        }

    public void LostPlayer()
        {
            activate = false;
        }

    }