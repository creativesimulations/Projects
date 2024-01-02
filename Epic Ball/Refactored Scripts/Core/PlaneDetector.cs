using System;
using UnityEngine;

namespace EpicBall
{
    public class PlaneDetector : MonoBehaviour
    {

        public static event Action FellToDeath;

        [Tooltip("If this is the lowest plane in the scene.")]
        [SerializeField] private bool _isBottom = false;

        private MeshRenderer _mesh;
        private MeshRenderer _mesh2;
        private bool _isAbove { get; set; }

        private void Awake()
        {
            _mesh = GetComponent<MeshRenderer>();
            _mesh2 = GetComponentsInChildren<MeshRenderer>()[1];
            if (_mesh == null || _mesh2 == null)
            {
                ExceptionManager.instance.SendMissingComponentMessage("mesh", GetType().ToString(), name);
            }
            Ball.OnChangeLocation += CheckHeight;
            if (_isBottom )
            {
                BoxCollider boxCollider = GetComponent<BoxCollider>();
                boxCollider.center = new Vector3(0,-5,0);
                boxCollider.size = new Vector3(boxCollider.size.x, 4, boxCollider.size.z);
            }
        }


        private void OnTriggerExit(Collider other)
        {
            if (other.isTrigger)
            {
                return;
            }
            if (other.gameObject.CompareTag(GlobalConstants.PLAYER))
            {
                CheckHeight(other.gameObject);

                if (_isBottom && !_isAbove)
                {
                    FellToDeath?.Invoke();
                }
            }
            else if (_isBottom)
            {
                Destroy(other.gameObject, 10f);
            }
        }

        public void CheckHeight(GameObject player)
        {
            if (player.transform.position.y < transform.position.y)
            {
                _isAbove = false;
            }
            else
            {
                _isAbove = true;
            }
            SetRender(_isAbove);
        }

        public void SetRender(bool above)
        {
            if (!above)
            {
                _mesh.enabled = false;
                _mesh2.enabled = true;
            }
            else if (above)
            {
                _mesh.enabled = true;
                _mesh2.enabled = false;
            }
        }

        private void OnDisable()
        {
            Ball.OnChangeLocation -= CheckHeight;
        }
    }
}