using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shoot : MonoBehaviour
{
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private Transform _gunOffset;
    [SerializeField] private float _timeBetweenShots = 0;

    private bool fireContinuously;
    private float lbf;
    private bool firesingle;

    public weapon currentWeapon;

    void Start()
    {
        PushWeaponToHud();
    }

    void Update()
    {
        if (fireContinuously || firesingle)
        {
            if (Time.time >= _timeBetweenShots)
            {
                currentWeapon.Shoot();

                // ✅ plays shoot SFX through AudioManager
                AudioManager.Instance?.PlayPlayerShoot();

                _timeBetweenShots = Time.time + 1 / currentWeapon.fireRate;
                firesingle = false;
            }
        }
    }

    private void PushWeaponToHud()
    {
        if (currentWeapon != null)
        {
            HudOverlayController.Instance?.SetCurrentWeapon(currentWeapon);
        }
    }

    // Call this from weaponpickup.cs when picking up a new weapon
    public void SwitchWeapon(weapon newWeapon)
    {
        currentWeapon = newWeapon;
        PushWeaponToHud();
    }

    private void FireBullet()
    {
        GameObject bullet = Instantiate(_bulletPrefab, _gunOffset.position, transform.rotation);
        Rigidbody2D rigidbody = bullet.GetComponent<Rigidbody2D>();
        rigidbody.linearVelocity = bulletSpeed * transform.up;
    }

    private void OnFire(InputValue inputValue)
    {
        fireContinuously = inputValue.isPressed;
        if (inputValue.isPressed) firesingle = true;
    }

    public void OnFireButtonDown()
    {
        fireContinuously = true;
        firesingle = true;
    }

    public void OnFireButtonUp()
    {
        fireContinuously = false;
    }
}