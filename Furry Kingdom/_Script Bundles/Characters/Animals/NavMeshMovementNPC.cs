using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

[RequireComponent(typeof(NavMeshAgent))]
public class NavMeshMovementNPC : MonoBehaviour
{
    public NavMeshAgent Agent { get; private set; }
    public bool Arrived { get; private set; }
    private bool _reachablePath;
    private NavMeshPath _path;

    private CancellationTokenSource _searchCTS;
    private CancellationTokenSource _moveCTS;

    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        _path = new NavMeshPath();
    }

    private void Start()
    {
        _searchCTS = new CancellationTokenSource();
        ReachedDestination();
    }

    public void EnableAgent(bool value)
    {
        Agent.enabled = value;
    }

    public void SetMovementSpeed(float newSpeed)
    {
        Agent.speed = newSpeed;
    }

    public void Chase(Vector3 destination)
    {
        if (Arrived)
        {
            PendingArrival();
            Move(destination);
        }
        else
        {
            Agent.SetDestination(destination);
        }
    }
    public void SearchForLocation(float range)
    {
        PendingArrival();
        _searchCTS = new CancellationTokenSource();
        GetNewLocation(_searchCTS.Token, range);
    }

    public async void GetNewLocation(CancellationToken ct, float moseyRange)
    {
        while (!ct.IsCancellationRequested)
        {
            Vector3 randomPoint = transform.position + Random.insideUnitSphere * moseyRange;
            NavMeshHit hit;

            if (NavMesh.SamplePosition(randomPoint, out hit, moseyRange, NavMesh.AllAreas))
            {
                Debug.DrawRay(hit.position, Vector3.up, Color.blue, 2);

                if (CheckPathReachable(hit.position))
                {
                    Move(hit.position);
                    CancelSearchForLocation();
                }
            }
            await Task.Yield();
        }
    }
    private bool CheckPathReachable(Vector3 position)
    {
        Agent.CalculatePath(position, _path);

        if (_path.status == NavMeshPathStatus.PathComplete)
        {
            _reachablePath = true;
        }
        else
        {
            _reachablePath = false;
        }

        return _reachablePath;
    }
    private async void Move(Vector3 destination)
    {
        _moveCTS = new CancellationTokenSource();
        Agent.SetDestination(destination);

        Debug.DrawRay(destination, Vector3.up, Color.red, 2);

        while (!Arrived && !_moveCTS.IsCancellationRequested)
        {
            if (!Agent.hasPath)
            {
                await Task.Yield();
            }
            if (Agent.remainingDistance <= Agent.stoppingDistance)
            {
                ReachedDestination();
            }
                await Task.Yield();
        }
    }

    public void PendingArrival()
    {
        Debug.Log("NOT Arrived");
        Arrived = false;
    }

    public void CancelMovingToDestination()
    {
        Debug.Log("CancelMovingToDestination");
        if (Agent.hasPath)
        {
            Agent.SetDestination(transform.position);
        }
        ReachedDestination();
    }
    public void CancelSearchForLocation()
    {
        _searchCTS?.Cancel();
    }

    public void ReachedDestination()
    {
        Debug.Log("Arrived");
        Arrived = true;
    }

    private void OnDisable()
    {
        CancelSearchForLocation();
        CancelMovingToDestination();
    }


}
