using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scriptables;

namespace Contagion
{
    public class Contagion : MonoBehaviour
    {

        [SerializeField] private ContagionScriptableObject contagionScriptable;
        public ContagionController contagionController;
        [SerializeField] [Range(2f, 10f)] private float _topSpeed;
        private float _currentSpeed;
        private bool isMoving = false;
        public bool Isinfected { get; set; }
        private bool survived;
        private bool alreadySetUp;
        public bool freeRoaming;
        public int areaToMoveIn;
        public Vector3 goal;
        [SerializeField] public Status status;
        public enum Status // Team enum
        {
            Well,
            Ill,
            Immune
        };



        void Start()
        {
            if (!alreadySetUp && status == Status.Well)
            {
                alreadySetUp = true;
                _currentSpeed = Random.Range(1, _topSpeed);
                freeRoaming = (Random.value < contagionScriptable.freeroaming);
                contagionController = GetComponentInParent<ContagionController>();
                contagionController.wellCharacterList.Add(this.gameObject);
                transform.position = contagionController.GetARandomPosOnPlane();
                areaToMoveIn = Random.Range(0, contagionController.moveableAreas.Length);
                GetNewGoal();
            }
        }

        public void SetStats(bool free, int areaNum, ContagionController cC, Vector3 g, float firstSpeed, bool didSurvive)
        {
            freeRoaming = free;
            areaToMoveIn = areaNum;
            contagionController = cC;
            goal = g;
            _currentSpeed = firstSpeed;
            survived = didSurvive;
            if (status == Status.Well)
            {
                contagionController.wellCharacterList.Add(this.gameObject);
            }
            else if (status == Status.Immune)
            {
                contagionController.immuneCharacterList.Add(this.gameObject);
            }
            else if (status == Status.Ill)
            {
                contagionController.illCharacterList.Add(this.gameObject);
                IsInfected = true;
                InvokeRepeating("FindNearbyObjects", Random.Range(0, .49f), .5f);
                StartCoroutine(WhileIll(Random.Range(contagionScriptable.minContagionDuration, contagionScriptable.maxContagionDuration)));
            }
            StartCoroutine(MoveOverSpeed(goal));
        }

        private void GetNewGoal()
        {
            if (status == Status.Ill)
            {
            }
            if (freeRoaming)
            {
                goal = contagionController.GetARandomPosOnPlane();
            }
            else
            {
                goal = contagionController.GetPointInCirle(areaToMoveIn);
            }
            StartCoroutine(MoveOverSpeed(goal));
        }

        public IEnumerator MoveOverSpeed(Vector3 end)
        {
            isMoving = true;
            if (status == Status.Ill)
            {
            }
            // speed should be 1 unit per second
            while (transform.position != end && isMoving)
            {
                transform.LookAt(end);
                transform.position = Vector3.MoveTowards(transform.position, end, _currentSpeed * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            isMoving = false;
            GetNewGoal();
        }

        private IEnumerator WhileIll(float duration)
        {
            yield return new WaitForSeconds(duration);
            EndInfection();
        }

        private void FindNearbyObjects()
        {
            Collider[] overlaps;
            overlaps = Physics.OverlapSphere(transform.position, contagionScriptable.infectionRadius);

            if (overlaps.Length > 0)
            {
                foreach (Collider col in overlaps)
                {
                    if (col.gameObject != this.gameObject)
                    {
                        col.gameObject.GetComponent<Contagion>().OnInteraction();
                    }
                }
            }
        }

        public void OnInteraction()
        {
            if (status == Status.Well)
            {
                float chanceToBeInfected = Random.Range(0f, 100f);
                if (chanceToBeInfected <= contagionScriptable.contagionRate)
                {
                    StartCoroutine(IncubationPeriod(Random.Range(contagionScriptable.minIncubationPeriod, contagionScriptable.maxIncubationPeriod)));
                }
                else
                {
                    return;
                }
            }
        }

        private IEnumerator IncubationPeriod(float duration)
        {
            yield return new WaitForSeconds(duration);
            WellToIll(goal);
        }

        private void EndInfection()
        {
            CancelInvoke("FindNearbyObjects");
            IsInfected = false;
            isMoving = false;
            contagionController.illCharacterList.Remove(this.gameObject);
            StopAllCoroutines();
            if (!survived && Random.Range(1f, 100f) <= contagionScriptable.Lethality)
            {
                contagionController.SwitchToDead(this.gameObject, transform.position, transform.rotation, goal);
            }
            else if (contagionScriptable.RepeatInfections)
            {
                survived = true;
                contagionController.SwitchToWell(this.gameObject, transform.position, transform.rotation, goal, freeRoaming, areaToMoveIn, _currentSpeed * 2, survived);
            }
            else
            {
                contagionController.SwitchToImmune(this.gameObject, transform.position, transform.rotation, goal, freeRoaming, areaToMoveIn, _currentSpeed * 2);
            }
        }

        public void WellToIll(Vector3 g)
        {
            contagionController.wellCharacterList.Remove(this.gameObject);
            isMoving = false;
            StopAllCoroutines();
            if (_currentSpeed == 0)
            {
                _currentSpeed = _currentSpeed;
            }
            contagionController.SwitchToIll(this.gameObject, transform.position, transform.rotation, g, freeRoaming, areaToMoveIn, _currentSpeed / 2, survived);
        }

        public bool IsInfected
        {
            get
            {
                return Isinfected;
            }
            set
            {
                Isinfected = value;

            }
        }


        private void OnDrawGizmosSelected()
        {

            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(transform.position, contagionScriptable.infectionRadius);
        }
    }
}
