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
    private Vector3 _tryLocation;
    private Vector3 _newLocation;
    private Vector3 _directionToPlayer;

    private void Awake() { Agent = GetComponent<NavMeshAgent>(); _path = new NavMeshPath(); }
    private void Start() { ReachedDestination(); }
    public void SetMovementSpeed(float newSpeed) { Agent.speed = newSpeed; }
    public void PendingArrival() { Arrived = false; }
    public void ReachedDestination() { Arrived = true; }

    public void Walk(float range)
    {
        Debug.Log("Walk");
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
    public void Flee(Vector3 playerPosition, float walkRange)
    {
        PendingArrival();
        _directionToPlayer = playerPosition - transform.position;
        _tryLocation = transform.position + _directionToPlayer;
        _newLocation = TestNewLocation(_tryLocation, walkRange);

        if (_newLocation != Vector3.zero)
        {
            Move(_newLocation);
        }
        else
        {
            SearchForLocation(walkRange);
        }
    }
    public void SearchForLocation(float range)
    {
        Debug.Log("search");
        _searchCTS = new CancellationTokenSource();
        GetNewLocation(_searchCTS.Token, range);
    }

    public async void GetNewLocation(CancellationToken ct, float walkRadius)
    {
        while (!ct.IsCancellationRequested)
        {
            _newLocation = transform.position + Random.insideUnitSphere * walkRadius;

            if (TestNewLocation(_newLocation, walkRadius) != Vector3.zero)
            {
                Move(_newLocation);
                CancelSearchForLocation();
            }
            await Task.Yield();
        }
    }
    private Vector3 TestNewLocation(Vector3 location, float walkRadius)
    {
        NavMeshHit hit;

        if (NavMesh.SamplePosition(location, out hit, walkRadius, NavMesh.AllAreas))
        {
            Debug.DrawRay(hit.position, Vector3.up, Color.blue, 2);

            Debug.Log("TestNewLocation = " + hit.position);
            if (CheckPathReachable(hit.position))
            {
                Debug.Log("REACHABLE");
                return hit.position;
            }
        }
        return Vector3.zero;
    }
    private bool CheckPathReachable(Vector3 position)
    {
        Debug.Log("Checking if path is reachable");
        Agent.CalculatePath(position, _path);

        if (_path.status == NavMeshPathStatus.PathComplete)
        {
            Debug.Log("Path Complete");
            _reachablePath = true;
        }
        else
        {
            Debug.Log("Path NOT Complete");
            _reachablePath = false;
        }

        return _reachablePath;
    }
    private async void Move(Vector3 destination)
    {
        Debug.Log("Moving");
        _moveCTS = new CancellationTokenSource();
        Agent.SetDestination(destination);

        Debug.DrawRay(destination, Vector3.up, Color.red, 2);

        while (!Arrived && !_moveCTS.IsCancellationRequested)
        {
            if (!Agent.hasPath)
            {
                Debug.Log("NO PATH!!!!");
                await Task.Yield();
            }
            if (Agent.remainingDistance <= Agent.stoppingDistance)
            {
                Debug.Log("reached destination");
                ReachedDestination();
            }
                await Task.Yield();
        }
    }


    public void CancelSearchForLocation() { _searchCTS?.Cancel(); }
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
        CancelSearchForLocation();
        CancelMovingToDestination();
    }


}
