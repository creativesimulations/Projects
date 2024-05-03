using UnityEngine;
using UnityEngine.AI;

public class MoveCommand : Command
{
    private readonly Vector3 _destination;
    private readonly NavMeshAgent _agent;

    /// <summary>
    /// Moves to the desired location
    /// </summary>
    /// <param name="destination"></param> Location to move to.
    /// <param name="_agent"></param> Agent to move.
    public MoveCommand (Vector3 destination, NavMeshAgent _agent)
    {
        _destination = destination;
        this._agent = _agent;
    }

    /// <summary>
    /// Returns if the agent has reached the desired location.
    /// </summary>
    public override bool IsFinished => _agent.remainingDistance <= 0.1f;

    /// <summary>
    /// Does the command.
    /// </summary>
    public override void Execute()
    {
        _agent.SetDestination(_destination);
    }
}
