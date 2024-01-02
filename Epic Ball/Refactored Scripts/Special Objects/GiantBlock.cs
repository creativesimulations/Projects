using EpicBall;
using MilkShake;
using MilkShake.Demo;
using UnityEngine;

[RequireComponent(typeof(CylinderMovement))]
public class GiantBlock : Block
{

    private ShakeButton _shakeButton;
    private Camera _mainCamera;
    private float _distance;
    private float _shakeStrength;

    private void Awake()
    {
        _startAwake = true;
        Rb = GetComponent<Rigidbody>();
        audioSource = GetComponentInChildren<AudioSource>();
    }

    private void Start()
    {
        _mainCamera = Camera.main;
        _shakeButton = GetComponent<ShakeButton>();
        _shakeButton.UIToggleShake(true);
        GetComponent<CylinderMovement>().Chase(GameObject.FindGameObjectWithTag(GlobalConstants.PLAYER));
    }

    private void Update()
    {
        UpdateShakeDistance();
    }

    /// <summary>
    /// Updates the distance between the camera(which has the "Shaker" component) and the giant block (Which has the "ShakeButton" component).
    /// </summary>
    private void UpdateShakeDistance()
    {
        float _distance = Vector3.Distance(_mainCamera.transform.position, transform.position);
        float _shakeStrength = Mathf.InverseLerp(40, 0, _distance);
        _shakeButton.GetShakeInstance().StrengthScale = _shakeStrength;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag(GlobalConstants.PLAYER))
        {
            GameManager.SetGameState(GameManager.GameStates.Dead);
        }
    }
}
