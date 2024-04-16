
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace Furry
{

public class NavMeshMouseMovement : MonoBehaviour
{

        public NavMeshAgent _agent;
        public PlayerInputHandler _inputHandler;
        public Camera _camera;
        private RaycastHit hit;

        private void Awake()
        {
            _inputHandler = GetComponent<PlayerInputHandler>();
            _camera = Camera.main;
            _agent = GetComponent<NavMeshAgent>();
        }

        void Start()
    {
            _inputHandler.OnRightClick += ClickToMove;
    }

        private void ClickToMove()
        {
                Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
            {
                _agent.SetDestination(hit.point);
            }
        }
        private void OnDisable()
        {

            _inputHandler.OnRightClick -= ClickToMove;
        }
    }
}
