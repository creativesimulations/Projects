using Furry;
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
    private Vector3 _tryPoint;
    private Vector3 _newLocation;
    private Vector3 _directionToPlayer;

    private void Awake() { Agent = GetComponent<NavMeshAgent>(); _path = new NavMeshPath(); }
    private void Start() { ReachedDestination(); }

    /// <summary>
    /// Sets the navmesh Agent move speed.
    /// </summary>
    /// <param name="newSpeed"></param> Amount speed will be set to.
    public void SetMovementSpeed(float newSpeed) { Agent.speed = newSpeed; }

    /// <summary>
    /// Sets Arrived as false;
    /// </summary>
    public void PendingArrival() { Arrived = false; }

    /// <summary>
    /// Start process to walk to a new location.
    /// </summary>
    /// <param name="range"></param> Max range to walk in.
    public void Walk(float range)
    {
        PendingArrival();
        SearchForLocation(range);
    }

    /// <summary>
    /// Chase target by moving where target is located.
    /// </summary>
    /// <param name="destination"></param> target location.
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

    /// <summary>
    /// Flee from target.
    /// </summary>
    /// <param name="playerPosition"></param> target location to flee from.
    /// <param name="runRange"></param> maximum range to run.
    public void Flee(Vector3 playerPosition, float runRange)
    {
        PendingArrival();
        _directionToPlayer = playerPosition - transform.position;
        _tryPoint = transform.position - _directionToPlayer;
        _newLocation = Utilities.TestNewLocation(_tryPoint, runRange);

        if (_newLocation != Vector3.zero && CheckPathReachable(_newLocation))
        {
            Move(_newLocation);
        }
        else
        {
            SearchForLocation(runRange);
        }
    }

    /// <summary>
    /// Search for a new location on the navmesh.
    /// </summary>
    /// <param name="range"></param> Search within range.
    public void SearchForLocation(float range)
    {
        _getNewLocationCTS = new CancellationTokenSource();
        GetNewLocation(_getNewLocationCTS.Token, range);
    }

    /// <summary>
    /// While the task is not cancelled and the agent doesn't have a path, look for a new random location on the navmesh.
    /// </summary>
    /// <param name="ct"></param>
    /// <param name="range"></param> Maximum range to look for a new location.
    public async void GetNewLocation(CancellationToken ct, float range)
    {
        while (!ct.IsCancellationRequested && !Agent.hasPath)
        {
            _tryPoint = transform.position + Random.insideUnitSphere * range;
            _newLocation = Utilities.TestNewLocation(_tryPoint, range);

            if (CheckPathReachable(_newLocation))
            {
                Move(_newLocation);
                CancelGetNewLocation();
            }
            await Task.Yield();
        }
    }

    /// <summary>
    /// Returns true if the path is reachable.
    /// </summary>
    /// <param name="position"></param> Location to check.
    /// <returns></returns>
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

    /// <summary>
    /// Moves to the desired location while the location hasn't been reached and the task hasn't been cancelled.
    /// </summary>
    /// <param name="destination"></param> Location to move to.
    private async void Move(Vector3 destination)
    {
        _moveCTS = new CancellationTokenSource();
        Agent.SetDestination(destination);

        while (!Arrived && !_moveCTS.IsCancellationRequested)
        {
            if (!Agent.pathPending && Agent.remainingDistance <= Agent.stoppingDistance)
            {
                CancelMovingToDestination();
            }
            await Task.Yield();
        }
    }

    /// <summary>
    /// Sets Arrived as true;
    /// </summary>
    public void ReachedDestination() { Arrived = true; }

    /// <summary>
    /// Cancels that task of getting a new location.
    /// </summary>
    public void CancelGetNewLocation() { _getNewLocationCTS?.Cancel(); }

    /// <summary>
    /// Cancels the task of moving to a location.
    /// </summary>
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
