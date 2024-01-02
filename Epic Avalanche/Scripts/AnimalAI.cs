using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Animals
{

    public class AnimalAI : MonoBehaviour
    {
        Animator animator;
        NavMeshAgent agent;
        ToggleObjects tO;
        Rigidbody rb;
        AnimalGroupController AGC;

        public bool isDead = false;
        public bool isScared = false;
        public bool isMoving = false;
        public bool hasMoved = false;

        private string currentState;
        private string newState;
        private float countDownTime;
        private int walkableLayer = 1;
        private int scaredLayerLayerMask;
        [SerializeField] private int scaredColliderlayerNumber;
        [SerializeField] private int colliderlayerNumber;
        [SerializeField] float walkSpeed;
        [SerializeField] float runSpeed;
        [SerializeField] float walkRadius;
        [SerializeField] float runRadius;
        [SerializeField] float triggerRadius;
        private float stoppingDistanceOffset;

        #region Starting

        void Start()
        {
            AGC = GetComponentInParent<AnimalGroupController>();
            rb = GetComponent<Rigidbody>();
            animator = GetComponent<Animator>();
            tO = GetComponent<ToggleObjects>();
            agent = GetComponent<NavMeshAgent>();
            GetComponent<SphereCollider>().radius = triggerRadius / 5;
            tO.onHit += Die;
            tO.onColliderToFalse += TurnOffColliders;
            tO.onAnimationToFalse += DieAnim;
            if(tO.kenimaticToFalse)
            {
                tO.onKenimaticToFalse += TurnOffKenimatic;
            }
            agent.speed = walkSpeed;
            scaredLayerLayerMask = (1 << scaredColliderlayerNumber);
            SetAnimStartFrame();
            stoppingDistanceOffset = agent.stoppingDistance + 50;
        }

        private void SetAnimStartFrame()
        {
            int whichAnim = Random.Range(0, 2);
            switch (whichAnim)
            {
                case 0:
                    currentState = "isIdle";
                    animator.SetBool("isIdle", true);
                    animator.Play("idle", 0, Random.Range(0.0f, 1.0f));
                    break;
                case 1:
                    currentState = "isEating";
                    animator.SetBool("isEating", true);
                    animator.Play("eat", 0, Random.Range(0.0f, 1.0f));
                    break;
            }
        }

        #endregion

        #region AnimationControl
        private void OnTriggerEnter(Collider other)
        {
            if (!isScared && !isDead && other.gameObject.layer == scaredColliderlayerNumber)
            {
                ScaredNavSphere(transform, Random.Range(runRadius - (runRadius / 10), runRadius + (runRadius / 10)), walkableLayer, other.transform.position);
                isScared = true;
                agent.speed = runSpeed;
                SetNewState(1);
            }
        }

        public void GroupControllerSetState(int newStateInt)
        {
            if (!isMoving && !isDead)
            {
                SetNewState(newStateInt);
            }
        }

        private void SetNewState(int changeToState)
        {
            switch (changeToState)
            {
                case 1:
                    isMoving = true;
                    newState = "isRunning";
                    InvokeRepeating("ReachedDestinationOrGaveUp", 2, 2);
                    break;
                case 2:
                    RandomNavSphere(transform, Random.Range(walkRadius - (walkRadius / 10), walkRadius + (walkRadius / 10)), walkableLayer);
                    isMoving = true;
                    newState = "isWalking";
                    InvokeRepeating("ReachedDestinationOrGaveUp", 2, 2);
                    break;
                case 3:
                    newState = "isIdle";
                    break;
                case 4:
                    newState = "isEating";
                    break;
            }
            SetAnimatorState();
        }

        public void ReachedDestinationOrGaveUp()
        {
            if (isDead)
            {
                CancelInvoke("ReachedDestinationOrGaveUp");
                return;
            }
            else if (!agent.pathPending)
            {
                if (agent.remainingDistance <= stoppingDistanceOffset)
                {
                    if (isScared)
                    {
                        Collider[] scaryColliders = Physics.OverlapSphere(transform.position, triggerRadius, scaredLayerLayerMask);
                        if (scaryColliders.Length > 0)
                        {
                            agent.ResetPath();
                            ScaredNavSphere(transform, Random.Range(runRadius - (runRadius / 10), runRadius + (runRadius / 10)), walkableLayer, scaryColliders[0].gameObject.transform.position);
                            return;
                        }
                        else
                        {
                            isScared = false;
                        }
                    }
                    if (!isScared && !agent.hasPath || agent.velocity.sqrMagnitude <= 1f)
                    {
                        agent.speed = walkSpeed;
                        agent.ResetPath();
                        isMoving = false;
                        CancelInvoke("ReachedDestinationOrGaveUp");
                        SetNewState(Random.Range(3, 5));
                    }
                }
            }
        }

        #endregion

        #region Single Methods

        private void Die(Collision hitBy)
        {
            if (hitBy.gameObject.layer == scaredColliderlayerNumber)
            {
                if (isMoving)
                {
                    isMoving = false;
                }
                isDead = true;
                agent.ResetPath();
                agent.isStopped = true;
                agent.velocity = Vector3.zero;
                CancelInvoke("ReachedDestinationOrGaveUp");
                tO.onHit -= Die;
                tO.collisionUsed = false;
                //    StopAllCoroutines();
            }
        }

        private void TurnOffColliders()
        {
            tO.colliderToFalse = false;
            Collider[] colList = GetComponents<Collider>();
            foreach (Collider col in colList)
            {
                col.enabled = false;
            }
        }

        private void TurnOffKenimatic()
        {
            tO.kenimaticToFalse = false;
            rb.isKinematic = false;
            tO.onKenimaticToFalse -= TurnOffKenimatic;
        }

        private void DieAnim()
        {
            newState = "isDead";
            SetAnimatorState();
            tO.animationToFalse = false;
          //  StartCoroutine(DieAnimEnumerator());
        }

        private IEnumerator DieAnimEnumerator()
        {
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length + animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
            animator.enabled = false;
        }

        private void SetAnimatorState()
        {
            if (currentState != newState)
            {
                animator.SetBool(currentState, false);
                animator.SetBool(newState, true);
                currentState = newState;
            }
        }

        public void RandomNavSphere(Transform origin, float distance, int layer)
        {
            Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * distance;
            randomDirection += origin.position;
            NavMeshHit navHit;
            NavMesh.SamplePosition(randomDirection, out navHit, distance, layer);
            NavMeshPath path = new NavMeshPath();
            agent.CalculatePath(navHit.position, path);
            agent.SetPath(path);
        }

        public void ScaredNavSphere(Transform origin, float distance, int layer, Vector3 colliderToRunFrom)
        {
            Vector3 runTo = transform.position - colliderToRunFrom;
            runTo += origin.position;
            NavMeshHit navHit;
            NavMesh.SamplePosition(runTo, out navHit, distance, layer);
            NavMeshPath path = new NavMeshPath();
            agent.CalculatePath(navHit.position, path);
            agent.SetPath(path);
        }


        /* 
            private void ResetCountDownTime()
            {
                countDownTime = Random.Range(3f, countDownTimeMaximum);
                StartCoroutine(CountDown());
            } */
        /* 
            private IEnumerator CountDown()
            {
                yield return new WaitForSeconds(countDownTime);
                ResetCountDownTime();
                animatorStateChanger = UnityEngine.Random.Range(2, 14);
                SetNewState(animatorStateChanger);
            }
         */

        /* 
            public void LookAtTarget(GameObject target)
            {
                Vector3 targetPosition = new Vector3(target.transform.position.x, target.transform.position.y, target.transform.position.z);
                Vector3 directionToLook = targetPosition - this.transform.position;

                this.transform.rotation = Quaternion.Slerp(this.transform.rotation,
                    Quaternion.LookRotation(directionToLook),
                    Time.deltaTime * agent.speed);
            }

            public void LookAwayFromTarget(GameObject target)
            {
                Vector3 targetPosition = new Vector3(target.transform.position.x, target.transform.position.y, target.transform.position.z);
                Vector3 direction = transform.position - target.transform.position;
                transform.rotation = Quaternion.LookRotation(direction);
            } */

        void OnDrawGizmos()
        {
            // Draw attack sphere
            // Gizmos.color = new Color(255f, 0f, .5f);
            // Gizmos.DrawWireSphere(transform.position, walkRadius);
            // Draw chase sphere
            // Gizmos.color = new Color(175f, 255f, .5f);
            // Gizmos.DrawWireSphere(transform.position, runRadius);
            // Draw attention sphere
            //Gizmos.color = new Color(150f, 100f, .5f);
            //Gizmos.DrawWireSphere(transform.position, triggerRadius);
            // Draw scared sphere
        }

        #endregion



    }

}
