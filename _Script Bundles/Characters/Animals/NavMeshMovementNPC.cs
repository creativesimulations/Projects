using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NavMeshMovementNPC : MonoBehaviour
{
    public NavMeshAgent Agent { get; private set; }
    public bool Arrived { get; private set; }
    private bool _reachablePath;
    private NavMeshPath _path;

    private CancellationTokenSource _getNewLocationCTS;
    private CancellationTokenSource _moveCTS;
    private Vector3 _tryLocation;
    private Vector3 _newLocation;
    private Vector3 _directionToPlayer;

    private void Awake() { Agent = GetComponent<NavMeshAgent>(); _path = new NavMeshPath(); }
    private void Start() { ReachedDestination(); }
    public void SetMovementSpeed(float newSpeed) { Agent.speed = newSpeed; }
    public void PendingArrival() { Arrived = false; }

    public void Walk(float range)
    {
        PendingArrival();
        SearchForLocation(range);
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
    public void Flee(Vector3 playerPosition, float runRange)
    {
        PendingArrival();
        _directionToPlayer = playerPosition - transform.position;
        _tryLocation = transform.position - _directionToPlayer;
        _newLocation = TestNewLocation(_tryLocation, runRange);

        if (_newLocation != Vector3.zero)
        {
            Move(_newLocation);
        }
        else
        {
            SearchForLocation(runRange);
        }
    }
    public void SearchForLocation(float range)
    {
        _getNewLocationCTS = new CancellationTokenSource();
        GetNewLocation(_getNewLocationCTS.Token, range);
    }

    public async void GetNewLocation(CancellationToken ct, float range)
    {
        while (!ct.IsCancellationRequested && !Agent.hasPath)
        {
            _newLocation = transform.position + Random.insideUnitSphere * range;

            if (TestNewLocation(_newLocation, range) != Vector3.zero)
            {
                Move(_newLocation);
                CancelGetNewLocation();
            }
            await Task.Yield();
        }
    }
    private Vector3 TestNewLocation(Vector3 location, float range)
    {
        NavMeshHit hit;

        if (NavMesh.SamplePosition(location, out hit, range, NavMesh.AllAreas))
        {
            Debug.DrawRay(hit.position, Vector3.up, Color.blue, 2);

            if (CheckPathReachable(hit.position))
            {
                return hit.position;
            }
        }
        return Vector3.zero;
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

      //  Debug.DrawRay(destination, Vector3.up, Color.red, 2);

        while (!Arrived && !_moveCTS.IsCancellationRequested)
        {
            if (!Agent.pathPending && Agent.remainingDistance <= Agent.stoppingDistance)
            {
                CancelMovingToDestination();
            }
                await Task.Yield();
        }
    }


    public void ReachedDestination() { Arrived = true; }
    public void CancelGetNewLocation() { _getNewLocationCTS?.Cancel(); }
    public void CancelMovingToDestination()
    {
        if (Agent.hasPath)
        {
            Agent.SetDestination(transform.position);
        }
        ReachedDestination();
    }
    private void OnDisable()
    {
        CancelGetNewLocation();
        CancelMovingToDestination();
    }


}
