using Core;
using Mechanics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Scriptables;
using UnityEngine.InputSystem;


namespace Weapon
{


    public class TurretController : MonoBehaviour, Ishoot
    {

        [SerializeField] private WeaponScriptable _turretScriptable;
        [SerializeField] private float _animSpeedIncrease = .01f;
        [SerializeField] private Transform _turretMuzzle;
        [SerializeField] private float _warmUpAnimation = 2; 
        [SerializeField] private int _burstSize = 4;
        [SerializeField] private float _radiusToAim = 5;
        [SerializeField] private Transform _target;

        private TurretAim _turretAim;
        private Animator _animator;
        private ObjectPooler _pool;
        private InputManager _inputManager;
        bool canShoot;
        bool isStrafing;

        // Start is called before the first frame update
        void Start()
        {
            _turretAim = GetComponent<TurretAim>();
            _turretAim.enabled = false;
            _animator = GetComponentInChildren<Animator>();
            _pool = Singleton.instance.GetComponent<ObjectPooler>();

            _inputManager = Singleton.instance.GetComponent<InputManager>();
            _inputManager._inputActions.PlayerInput.InitializeWeaponTest.performed += InitializeWeapon;
            _inputManager._inputActions.PlayerInput.WarmUpWeapon.performed += WarmUpWeapon;
            _inputManager._inputActions.PlayerInput.Shoot.performed += CallShoot;
            _inputManager._inputActions.PlayerInput.Strafe.performed += Strafe;
        }
        public void InitializeWeapon(InputAction.CallbackContext context)
        {
            _turretAim.AimPosition = _target.position;
            _turretAim.enabled = true;
            _turretAim.IsIdle = true;
            _turretAim.AimPosition = Vector3.zero;
        }

        private void WarmUpWeapon(InputAction.CallbackContext context)
        {
            StartCoroutine(IncreaseAnimSpeed());
        }

        private IEnumerator IncreaseAnimSpeed()
        {
            while (_animator.GetFloat("animSpeed") < _warmUpAnimation)
            {
                _animator.SetFloat("animSpeed", _animator.GetFloat("animSpeed") + _animSpeedIncrease);
                yield return new WaitForEndOfFrame();
            }
            _turretAim.IsIdle = false;
        }

        public void Aim()
        {
            _turretAim.AimPosition = Random.insideUnitSphere * _radiusToAim;
        }

        public void CallShoot(InputAction.CallbackContext context)
        {
            Shoot();
        }

        private void Shoot()
        {
            var projectile = _pool.PopFromPool(_pool.GetObjectInProjectilesDictionary(_turretScriptable.projectile.name), false, true, _pool.GetTransformInParentsDictionary(_turretScriptable.projectile.name), false);
            projectile.transform.SetPositionAndRotation(_turretMuzzle.transform.position, _turretMuzzle.transform.rotation);
            projectile.SetActive(true);
        }

        public void Strafe(InputAction.CallbackContext context)
        {
            if (isStrafing)
            {
                canShoot = false;
                isStrafing = false;
            }
            else
            {
                isStrafing = true;
                Aim();
                StartCoroutine(Strafe());
            }
        }

        private IEnumerator Strafe()
        {
            int rest = 0;
            canShoot = true;
            int shotsMade = 0;
            while (isStrafing)
            {
                if (shotsMade == _burstSize)
                {
                    canShoot = false;
                    rest++;
                }
                if (canShoot)
                {
                    Shoot();
                    shotsMade++;
                }
                yield return new WaitForSeconds(_turretScriptable.fireRate); // wait till the next round
                if (rest >= _turretScriptable.coolDownRate)
                {
                    canShoot = true;
                    rest = 0;
                    shotsMade = 0;
                }
            }
        }

        private void OnDisable()
        {
            _inputManager._inputActions.PlayerInput.InitializeWeaponTest.performed -= InitializeWeapon;
            _inputManager._inputActions.PlayerInput.WarmUpWeapon.performed -= WarmUpWeapon;
            _inputManager._inputActions.PlayerInput.Shoot.performed -= CallShoot;
            _inputManager._inputActions.PlayerInput.Shoot.started -= Strafe;
        }
    }

}