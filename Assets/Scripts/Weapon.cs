using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public Rigidbody2D rb;
    private PlayerController2D controller;

    private Vector2 recoilForce = Vector2.zero;
    public Vector2 recoilForceForward; // Recoil when shooting forward
    public Vector2 recoilForceDownward; // Recoil when shooting downward
    public Vector2 recoilForceUpward; // Recoil when shooting upward

    private Vector2 dampenedRecoil;

    public float fireRate = 2f; // Amount of bullets in 1 second
    public float fireCounter = 0f;

    public int maxAmmo = 2;
    public int currentAmmo;

    private bool fireForward = false;
    private bool fireDownward = false;
    private bool fireUpward = false;

    public float recoilTime = 5.0f;
    public bool recoil = false;
    private float recoilCounter = 0f;
    private Vector2 recoilDirection;

    private bool reloadAvailable = false; // True if player lands on ground after shooting

    private void Start()
    {
        controller = GetComponent<PlayerController2D>();
        currentAmmo = maxAmmo;
    }

    // Update is called once per frame
    private void Update()
    {
        // If gun can fire
        if (fireCounter <= 0f && currentAmmo > 0)
        {
            // Whenever fire button is pressed
            if (Input.GetButtonDown("Fire1"))
            {
                fireForward = true;
            }

            else if (Input.GetButtonDown("Fire2"))
            {
                fireDownward = true;
            }

            else if (Input.GetButtonDown("Fire3"))
            {
                fireUpward = true;
            }

            if (fireForward || fireDownward || fireUpward)
            {
                // Handle cooldown and ammo of gun
                fireCounter = 1.0f;
                currentAmmo -= 1;

                // Prevents player from shooting and jumping at same time
                // Set recently shot to true
                controller.recentlyShot = true;

                // Fail safe
                controller.m_Grounded = false;

                // Set recently shot to false after delay of x
                Invoke(nameof(ResetRecentlyShot), 0.1f);
            }
        }
        
        // Counts down for the next time the gun can be fired
        if (fireCounter >= 0f)
        {
            // Handles gun cooldown
            fireCounter -= Time.deltaTime * fireRate;
        }

        // If on ground, trigger flag to be able to reload
        if (controller.m_Grounded)
        {
            reloadAvailable = true;
        }

        else
        {
            reloadAvailable = false;
        }

        // Actually reload after not shooting for a small delay
        if (reloadAvailable)
        {
            Reload();
        }
    }

    private void FixedUpdate()
    {
        if (fireForward)
        {
            ShootForward();
            fireForward = false;
            controller.recentlyRecoiledBackwards = true;
        }

        else if (fireDownward)
        {
            ShootDownward();
            fireDownward = false;
            controller.recentlyRecoiledUpwards = true;
        }

        else if (fireUpward)
        {
            ShootUpward();
            fireUpward = false;
            controller.recentlyRecoiledDownwards = true;
        }

        if (recoil)
        {
            Recoil();
        }
    }

    void ShootForward()
    {
        // Create bullet prefab + set up recoil function
        GameObject bullet = (GameObject)Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // Change diirection of bullet to down
        bullet.GetComponent<Bullet>().direction = transform.right;
        recoil = true;

        // Recoil backward
        recoilDirection = -transform.right;
        recoilForce = recoilForceForward;
        dampenedRecoil = recoilForce;

        // Camera shake
        GetComponent<CinemachineImpulseSource>().GenerateImpulse(1f);
    }

    void ShootDownward()
    {
        // Create bullet prefab + set up recoil function
        GameObject bullet = (GameObject)Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // Change diirection of bullet to down
        bullet.GetComponent<Bullet>().direction = Vector2.down;
        recoil = true;

        // Recoil backward
        recoilDirection = transform.up;
        
        // Reset y velocity to be 0
        rb.velocity = new Vector2(rb.velocity.x, 0); ;
        recoilForce = recoilForceDownward;

        dampenedRecoil = recoilForce;

        // Camera shake
        GetComponent<CinemachineImpulseSource>().GenerateImpulse(1f);
    }

    void ShootUpward()
    {
        // Create bullet prefab + set up recoil function
        GameObject bullet = (GameObject)Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // Change diirection of bullet to down
        bullet.GetComponent<Bullet>().direction = Vector2.up;
        recoil = true;

        // Recoil backward
        recoilDirection = -transform.up;

        // Reset y velocity to be 0
        rb.velocity = new Vector2(rb.velocity.x, 0); ;
        recoilForce = recoilForceUpward;

        dampenedRecoil = recoilForce;

        // Camera shake
        GetComponent<CinemachineImpulseSource>().GenerateImpulse(1f);
    }

    private void Recoil()
    {
        if (recoilCounter <= 0f)
        {
            // Set recoil in player controller script to this weapon's recoil
            controller.recoil = dampenedRecoil * recoilDirection;

            // Reduce second recoil for smoothing
            dampenedRecoil *= 0.95f;
            recoilCounter = 0.1f;

            // If grounded, reduce recoil
            if (controller.m_Grounded)
            {
                dampenedRecoil *= 0.9f;
                recoilCounter = 0.05f;
            }

            // If next recoil is negligible, reset all
            if (dampenedRecoil.x <= 25f)
            {
                // Debug.Log("Reset");
                recoil = false;
                dampenedRecoil = recoilForce;
                recoilCounter = 0f;
            }
        }

        // Counts down til the next part of the smooth recoil
        if (recoilCounter >= 0f)
        {
            recoilCounter -= Time.fixedDeltaTime * recoilTime;
        }
    }

    private void Reload()
    {
        currentAmmo = maxAmmo;
    }

    private void ResetRecentlyShot()
    {
        controller.recentlyShot = false;
    }
}
