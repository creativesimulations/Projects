using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class CommandControlledBot : MonoBehaviour
{
    private NavMeshAgent _agent;
    private Queue<Command> _commands = new Queue<Command>();
    private Command _currentCommand;

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        ListenForCommands();
        ProcessCommands();
    }

    /// <summary>
    /// Processes the next command that is queued.
    /// </summary>
    private void ProcessCommands()
    {
        if (_currentCommand != null && _currentCommand.IsFinished == false)
        {
            return;
        }
        if (_commands.Any() == false )
        {
            return;
        }
        _currentCommand = _commands.Dequeue();
        _currentCommand.Execute();
    }

    /// <summary>
    /// Adds new command locations to move to when the mouse clicks on the navmesh.
    /// </summary>
    private void ListenForCommands()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hitInfo))
            {
                _commands.Enqueue(new MoveCommand(hitInfo.point, _agent));
            }
        }
    }
}
