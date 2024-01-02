using System;
using System.Collections;
using UnityEngine;

namespace EpicBall
{
    [RequireComponent(typeof(AudioSource))]

    public class CameraController : MonoBehaviour
    {
        public static event Action OnZoomed;

        [Header("Setup parameters for the camera.")]
        [Tooltip("The distance amount to keep from the player.")]
        [SerializeField] private Vector3 _followOffsetY;
        [Tooltip("The tilt amount to rotate toward the player.")]
        [SerializeField] private float _followRotationTiltX = 80;
        [Tooltip("When the player completes a level, zoom in to this height.")]
        [SerializeField] private float _zoomOnWinY = 4;
        [Tooltip("When the player completes a level, zoom in to this distance.")]
        [SerializeField] private float _zoomOnWinDistance = 20;
        [Tooltip("When the player is destroyed, zoom in to this distance.")]
        [SerializeField] private float _zoomOnDeathDistance = 8;
        private float _speed = .001f;

        private GameObject _cameraTarget;
        private AudioSource _sound;

        private void Awake()
        {
            Ball.OnDestroyed += ZoomToDeath;
            GameManager.CompleteLvl += ZoomToWin;
            GameManager.PlayGame += CameraSetUp;

            _cameraTarget = GameObject.FindWithTag(GlobalConstants.CAMERA_TARGET);
            _sound = GetComponentInChildren<AudioSource>();
        }

        /// <summary>
        /// If the camera target (which is on the player game object) is in the scene, the follow coroutine is started.
        /// </summary>
        private void CameraSetUp()
        {
            if (_cameraTarget != null)
            {
                transform.rotation = Quaternion.Euler(_followRotationTiltX, 0, 0);
                StartCoroutine(FollowPlayer());
            }
            else
            {
                ExceptionManager.instance.SendMissingObjectMessage("_cameraTarget", GetType().ToString(), name);
            }
        }

        /// <summary>
        /// While the Game Manager state is 'Play' the camera will follow the camera target.
        /// </summary>
        /// <returns></returns>
        private IEnumerator FollowPlayer()
        {
            while (GameManager._gameStates == GameManager.GameStates.Play)
            {
                transform.position = _cameraTarget.transform.position + _followOffsetY;
                yield return null;
            }
        }

        /// <summary>
        /// Stops all coroutines and begins the coroutine to zoom in toward the camera target.
        /// </summary>
        private void ZoomToDeath()
        {
            StopAllCoroutines();
            StartCoroutine(ZoomInKilled());
        }

        /// <summary>
        /// While the distance from the camera to the camera target is less than the specified amount, the camera will gradually lower down and zoom toward the camera target.
        /// </summary>
        /// <returns></returns>
        IEnumerator ZoomInKilled()
        {
            float cameraDistance = Vector3.Distance(transform.position, _cameraTarget.transform.position);

            while (cameraDistance > _zoomOnDeathDistance)
            {
                cameraDistance = Vector3.Distance(transform.position, _cameraTarget.transform.position);
                transform.localPosition = Vector3.MoveTowards(transform.position, _cameraTarget.transform.position, _speed);

                yield return new WaitForEndOfFrame();
            }
            AudioSourceExtensions.FadeOut(_sound, 4f);
            _cameraTarget.transform.parent = null;
        }

        /// <summary>
        /// Stops all coroutines and begins the coroutine to zoom in toward the camera target.
        /// </summary>
        private void ZoomToWin()
        {
            StopAllCoroutines();
            StartCoroutine(ZoomInWin());
        }

        /// <summary>
        /// While the camera has not yet reached its end position, the camera will gradually move toward the end position and rotate up on the x axis.
        /// </summary>
        /// <returns></returns>
        IEnumerator ZoomInWin()
        {
            Vector3 endPosition = new Vector3(transform.position.x, transform.position.y + _zoomOnWinY, transform.position.z - _zoomOnWinDistance);

            while (transform.position != endPosition)
            {
                transform.position = Vector3.MoveTowards(transform.position, endPosition, _speed);
                transform.RotateAround(_cameraTarget.transform.position, new Vector3(1, 0, 0), -5 * Time.deltaTime);

                yield return new WaitForEndOfFrame();
            }

            AudioSourceExtensions.FadeOut(_sound, 4f);
        }

        private void OnDisable()
        {
            GameManager.CompleteLvl -= ZoomToWin;
            Ball.OnDestroyed -= ZoomToDeath;
            GameManager.PlayGame -= CameraSetUp;
        }

    }
}