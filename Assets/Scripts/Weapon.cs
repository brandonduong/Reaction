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
    public Vector2 recoilForceForward;
    public Vector2 recoilForceDownward;

    private Vector2 dampenedRecoil;

    public float fireRate = 2f; // Amount of bullets in 1 second
    private float fireCounter = 0f;

    public int maxAmmo = 2;
    private int currentAmmo;

    private bool fireForward = false;
    private bool fireDownward = false;

    public float recoilTime = 5.0f;
    private bool recoil = false;
    private float recoilCounter = 0f;
    private Vector2 recoilDirection;

    private void Start()
    {
        controller = GetComponent<PlayerController2D>();
        currentAmmo = maxAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        // If gun can fire
        if (fireCounter <= 0f && currentAmmo > 0)
        {
            // Whenever fire button is pressed
            if (Input.GetButtonDown("Fire1"))
            {
                fireForward = true;
                fireCounter = 1.0f;
                currentAmmo -= 1;
            }

            else if (Input.GetButtonDown("Fire2"))
            {
                fireDownward = true;
                fireCounter = 1.0f;
                currentAmmo -= 1;
            }
        }
        
        // Counts down for the next time the gun can be fired
        if (fireCounter >= 0f)
        {
            // Handles gun cooldown
            fireCounter -= Time.deltaTime * fireRate;
        }

        // Reload gun if player is on the ground
        if (controller.m_Grounded)
        {
            currentAmmo = 2;
        }
    }

    private void FixedUpdate()
    {
        if (fireForward)
        {
            ShootForward();
            fireForward = false;
        }

        if (fireDownward)
        {
            ShootDownward();
            fireDownward = false;
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
        bullet.GetComponent<Bullet>().direction = Vector2.right;
        recoil = true;

        // Recoil backward
        recoilDirection = -transform.right;
        recoilForce = recoilForceForward;
        dampenedRecoil = recoilForce;
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
        recoilForce = recoilForceDownward;
        dampenedRecoil = recoilForce;
    }

    void Recoil()
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
            recoilCounter -= Time.deltaTime * recoilTime;
        }
    }
}
