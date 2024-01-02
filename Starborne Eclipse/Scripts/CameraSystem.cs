using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace EpicDisasters
{

    /* this code is designed to be placed on the Camera System, which is an empty object. The Cinemachine Virtual Camera
     'follows' and 'looks at' the Camera System, but uses the main camera.
*/

    public class CameraSystem : MonoBehaviour
    {
        [Tooltip("Put the Cinemachine virtual Camera here.")]
        [SerializeField] private CinemachineVirtualCamera _cinemachineVirtualCamera;

        [Tooltip("The maximum distance the camera can zoom out.")]
        [SerializeField] private float _fieldOfViewMax = 50f;
        [Tooltip("The minimum distance the camera can zoom in.")]
        [SerializeField] private float _fieldOfViewMin = 10f;

        [Tooltip("The maximum distance the camera offset can be.")]
        [SerializeField] private float _followOffsetMax = 50f;
        [Tooltip("The minimum distance the camera offset can be.")]
        [SerializeField] private float _followOffsetMin = 10f;
        [Tooltip("The distance each tick of the mouse wheel will zoom the camera. To be used with 'CameraZoomForward' and 'CameraZoomAndLower'.")]
        [SerializeField] private float _zoomAmount = 5f;
        [Tooltip("The speed the camera zooms in and out.")]
        [SerializeField] private float _zoomSpeed = 10f;

        [Tooltip("Set as 'true' if you want to use drag pan for camera movement.")]
        [SerializeField] private bool _useDrag = false;
        [Tooltip("This is the speed of the camera mouse drag movement.")]
        [SerializeField] private float _dragSpeed = 5;
        [Tooltip("Set as 'true' if you want to use edge scrolling with the mouse.")]
        [SerializeField] private bool _useEdgeScrolling = false;
        [Tooltip("The size of the area from the edge of the screen to begin scrolling when the mouse pointer enters this area.")]
        [SerializeField] private int _edgeScrollSize = 20;

        [Tooltip("This is the speed of the camera movement.")]
        [SerializeField] private float _moveSpeed = 50;
        [Tooltip("This is the speed of the camera rotation.")]
        [SerializeField] private float _rotSpeed = 300;

        private bool _dragPanActive = false;
        private Vector2 _lastMousePos;
        private float _targetFieldOfView = 50;
        private Vector3 _followOffset;

        private void Awake()
        {
            _followOffset = _cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset;
        }

        void Update()
        {
            CameraMovement();
            CameraRotation();
            ZoomAndLower();
            // If edge scrolling is being used with the mouse.
            if (_useEdgeScrolling)
            {
                EdgeScrolling();
            }
            // If drag pan is being used with the mouse.
            
            if (_useDrag)
            {
                DragPan();
            }
            // CameraFOVZoom();
           //  CameraZoomForward();
            // CameraLower();
        }

        private void EdgeScrolling()
        {
            // If a key is pressed the new direction will be registered.
            Vector3 inputDir = new Vector3(0, 0, 0);
            /*
             If the mouse pointer is in the area size defined by '_edgeScrollSize' near the edges of the screen,
             then register the desired direction change.
            */

            if (Input.mousePosition.x < _edgeScrollSize)
            {
                inputDir.x = -1f;
            }
            if (Input.mousePosition.y < _edgeScrollSize)
            {
                inputDir.z = -1f;
            }
            if (Input.mousePosition.x > Screen.width - _edgeScrollSize)
            {
                inputDir.x = +1f;
            }
            if (Input.mousePosition.y > Screen.height - _edgeScrollSize)
            {
                inputDir.z = +1f;
            }

            // Set the local (NOT global) move directions to the new input directions. 
            Vector3 moveDir = transform.forward * inputDir.z + transform.right * inputDir.x;

            // Change the position of the camera according to the input direction.
            transform.position += moveDir * _moveSpeed * Time.deltaTime;
        }

        private void DragPan()
        {

            // If the right mouse button is held down and the mouse pointer is moved, then check for mouse movement.
            if (Input.GetMouseButtonDown(1))
            {
                _dragPanActive = true;
                _lastMousePos = Input.mousePosition;
            }

            // If the right mouse button is released, stop checking for mouse movement and stop panning the camera.
            if (Input.GetMouseButtonUp(1))
            {
                _dragPanActive = false;
            }

            // While drag is active, move the camera is the direction the mouse is being moved from the last mouse position.
            if (_dragPanActive)
            {
                // If a key is pressed the new direction will be registered.
                Vector3 inputDir = new Vector3(0, 0, 0);
                Vector2 mouseMovementDelta = (Vector2)Input.mousePosition - _lastMousePos;

                inputDir.x = mouseMovementDelta.x * _dragSpeed;
                inputDir.z = mouseMovementDelta.y * _dragSpeed;

                _lastMousePos = Input.mousePosition;

                // Set the local (NOT global) move directions to the new input directions. 
                Vector3 moveDir = transform.forward * inputDir.z + transform.right * inputDir.x;

                // Change the position of the camera according to the input direction.
                transform.position += moveDir * _moveSpeed * Time.deltaTime;
            }
        }

        private void CameraMovement()
        {
            // If a key is pressed the new direction will be registered.
            Vector3 inputDir = new Vector3(0, 0, 0);

            if (Input.GetKey(KeyCode.W)) inputDir.z = +1f;
            if (Input.GetKey(KeyCode.S)) inputDir.z = -1f;
            if (Input.GetKey(KeyCode.A)) inputDir.x = +1f;
            if (Input.GetKey(KeyCode.D)) inputDir.x = -1f;



            // Set the local (NOT global) move directions to the new input directions. 
            Vector3 moveDir = transform.forward * inputDir.z + transform.right * inputDir.x;

            // Change the position of the camera according to the input direction.
            transform.position += moveDir * _moveSpeed * Time.deltaTime;
        }

        private void CameraRotation()
        {
            // If a key is pressed the new rotation will be registered.
            float rotateDir = 0f;
            if (Input.GetKey(KeyCode.Q)) rotateDir = +1;
            if (Input.GetKey(KeyCode.E)) rotateDir = -1;

            // Set the local (NOT global) rotation. 
            transform.eulerAngles += new Vector3(0, rotateDir * _rotSpeed * Time.deltaTime, 0);


        }

        private void CameraFOVZoom()
        {
            // If the mouse scroll button is rolled up then zoom out.
            if (Input.mouseScrollDelta.y < 0)
            {
                _targetFieldOfView += 5;
            }
            // If the mouse scroll button is rolled down then zoom in.
            if (Input.mouseScrollDelta.y > 0)
            {
                _targetFieldOfView -= 5;
            }
            // Set the taget field of view and limit it to the maximum and minimum zoom.
            _targetFieldOfView = Mathf.Clamp(_targetFieldOfView, _fieldOfViewMin, _fieldOfViewMax);

            // Set the virtual camera field of view distance to the target distance and smooth the movement by using lerp.
            _cinemachineVirtualCamera.m_Lens.FieldOfView = Mathf.Lerp(_cinemachineVirtualCamera.m_Lens.FieldOfView, _targetFieldOfView, Time.deltaTime * _zoomSpeed);

        }

        private void CameraZoomForward()
        {
            Vector3 zoomDir = _followOffset.normalized;

            // If the mouse scroll button is rolled up then zoom out.
            if (Input.mouseScrollDelta.y < 0)
            {
                Debug.Log("CameraZoomForward y < 0");
                _followOffset += zoomDir * _zoomAmount;
            }
            // If the mouse scroll button is rolled down then zoom out.
            if (Input.mouseScrollDelta.y > 0)
            {
                Debug.Log("CameraZoomForward y > 0");
                _followOffset -= zoomDir * _zoomAmount;
            }

            // If the follow offset would be less than the offset minimum, set it to the minimum offest.
            if (_followOffset.magnitude < _followOffsetMin)
            {
                Debug.Log("CameraZoomForward magnitude < _followOffsetMax");
                _followOffset = zoomDir * _followOffsetMin;
            }
            // If the follow offset would be more than the offset maximum, set it to the maximum offest.
            if (_followOffset.magnitude > _followOffsetMax)
            {
                Debug.Log("CameraZoomForward magnitude > _followOffsetMax");
                _followOffset = zoomDir * _followOffsetMax;
            }

            // Set and lerp the cinemachine virtual camera offset.
            _cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = 
                Vector3.Lerp(_cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset, _followOffset, Time.deltaTime * _zoomSpeed);
        }
        private void CameraLower()
        {
            // If the mouse scroll button is rolled up then lower the camera.
            if (Input.mouseScrollDelta.y < 0)
            {
                Debug.Log("CameraLower y < 0");
                _followOffset.y += _zoomAmount;
            }
            // If the mouse scroll button is rolled down then raise the camera.
            else if (Input.mouseScrollDelta.y > 0)
            {
                Debug.Log("CameraLower y > 0");
                _followOffset.y -= _zoomAmount;
            }

            // Clamp the offset between the offset minimum and maximum.
            _followOffset.y = Mathf.Clamp(_followOffset.y, _followOffsetMin, _followOffsetMax);

            // Set and lerp the cinemachine virtual camera offset.
            _cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset =
                Vector3.Lerp(_cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset, _followOffset, Time.deltaTime * _zoomSpeed);
        }
        private void ZoomAndLower()
        {
            Vector3 zoomDir = _followOffset.normalized;

            // If the mouse scroll button is rolled up then zoom out and lower the camera.
            if (Input.mouseScrollDelta.y < 0)
            {
                _followOffset.y += _zoomAmount;
                _followOffset += zoomDir * _zoomAmount;
            }
            // If the mouse scroll button is rolled down then zoom out and raise the camera.
            else if (Input.mouseScrollDelta.y > 0)
            {
                _followOffset.y -= _zoomAmount;
                _followOffset -= zoomDir * _zoomAmount;
            }

            // If the follow offset would be less than the offset minimum, set it to the minimum offest.
            if (_followOffset.magnitude < _followOffsetMin)
            {
                _followOffset = zoomDir * _followOffsetMin;
            }
            // If the follow offset would be more than the offset maximum, set it to the maximum offest.
            else if (_followOffset.magnitude > _followOffsetMax)
            {
                _followOffset = zoomDir * _followOffsetMax;
            }

            // Set and lerp the cinemachine virtual camera offset.
            _cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset =
                Vector3.Lerp(_cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset, _followOffset, Time.deltaTime * _zoomSpeed);

        }
    }
}
