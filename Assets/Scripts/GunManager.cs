﻿using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RecoilType
{
    Gradual,
    Static
}

public class GunManager : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;

    public GameObject playerGhostPrefab;
    public int numGhosts = 3;
    public float timeBetweenGhosts = 0.5f;

    private Rigidbody2D rb;
    private PlayerController2D controller;

    private Vector2 recoilForce = Vector2.zero;

    private Vector2 dampenedRecoil;

    public GunType startingGun = GunType.Pistol; // Default gun
    public GunType gunType; // Keeps track of current gun's type
    public IGun currentGun; // Holds actual gun object
    public List<IGun> guns = new List<IGun>(); // List of all guns in player's loadout

    private bool fireForward = false;
    private bool fireDownward = false;
    private bool fireUpward = false;

    public float recoilTime = 5.0f;
    public bool recoil = false;
    private float recoilCounter = 0f;
    private Vector2 recoilDirection;

    private bool reloadAvailable = false; // True if player lands on ground after shooting

    // Controls whether or not these guns are in the player's possesion
    [SerializeField] public bool updateGuns = false;
    public GunType pickedupGun = GunType.None;
    public GameObject gunPickupParticles;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        controller = GetComponent<PlayerController2D>();

        // Add weapons to player loadout
        CheckGuns();

        // Set default gun
        if (guns.Count > 0)
        {
            gunType = startingGun;
            currentGun = guns[(int)gunType];
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetButtonDown("SwitchGuns") && !Pause.isPaused)
        {
            SwitchGuns();
        }

        // If gun can fire
        if (currentGun != null && currentGun.FireCounter <= 0f && currentGun.CurrentAmmo > 0 && !Pause.isPaused)
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
                currentGun.FireCounter = 1.0f;
                currentGun.CurrentAmmo -= 1;

                // Prevents player from shooting and jumping at same time
                // Set recently shot to true
                controller.recentlyShot = true;

                // Fail safe
                controller.m_Grounded = false;

                // Set recently shot to false after delay of x
                Invoke(nameof(ResetRecentlyShot), 0.1f);
            }
        }
        
        // Countdown the next time ALL guns can be fired
        foreach (IGun gun in guns)
        {
            // Handles gun cooldown
            if (gun.FireCounter >= 0f)
            {
                gun.FireCounter -= Time.deltaTime * gun.FireRate;
            }
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

        // If gun has been picked up
        if (updateGuns)
        {
            CheckGuns();
        }
    }

    private void FixedUpdate()
    {
        if (fireForward)
        {
            Shoot(transform.right, -transform.right, currentGun.RecoilForceForward);
            fireForward = false;
            GetComponent<PlayerMovement>().ResetRecentlyRecoiled();
            controller.recentlyRecoiledBackwards = true;
        }

        else if (fireDownward)
        {
            Shoot(Vector2.down, transform.up, currentGun.RecoilForceDownward);
            fireDownward = false;
            GetComponent<PlayerMovement>().ResetRecentlyRecoiled();
            controller.recentlyRecoiledUpwards = true;
        }

        else if (fireUpward)
        {
            Shoot(Vector2.up, -transform.up, currentGun.RecoilForceUpward);
            fireUpward = false;
            GetComponent<PlayerMovement>().ResetRecentlyRecoiled();
            controller.recentlyRecoiledDownwards = true;
        }

        if (recoil)
        {
            Recoil();
        }
    }

    private void Shoot(Vector2 bulletDir, Vector2 recoilDir, Vector2 recoilFor)
    {
        // Create bullet prefab + set up recoil function
        GameObject bullet = (GameObject)Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // Handle direction of bullet
        bullet.GetComponent<Bullet>().direction = bulletDir;

        // Set up recoil properties
        recoil = true;
        recoilDirection = recoilDir;

        // Reset y velocity to be 0 if recoil is to effect it
        if (recoilFor.y != 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }

        recoilForce = recoilFor;

        dampenedRecoil = recoilForce;

        // Camera shake
        GetComponent<CinemachineImpulseSource>().GenerateImpulse(currentGun.RecoilScreenShake);

        // Audio
        AudioManager.instance.PlaySound(currentGun.ShotSound);

        // Run particle system
        GetComponentInChildren<ParticleSystem>().Play();

        // Create ghost effect of player
        for (int i = 0; i < numGhosts; i++)
        {
            Invoke(nameof(GhostEffects), i * timeBetweenGhosts * Time.fixedDeltaTime);
        }
    }

    private void GhostEffects()
    {
        Instantiate(playerGhostPrefab, transform.position, transform.rotation);
    }

    private void Recoil()
    {
        if (recoilCounter <= 0f)
        {
            // Handle different recoil types
            switch (currentGun.RecoilType)
            {
                case RecoilType.Gradual:
                    GradualRecoil();
                    break;

                case RecoilType.Static:
                    StaticRecoil();
                    break;

                default:
                    GradualRecoil();
                    break;
            }
        }

        // Counts down til the next part of the smooth recoil
        if (recoilCounter >= 0f)
        {
            recoilCounter -= Time.fixedDeltaTime * recoilTime;
        }
    }

    private void GradualRecoil()
    {
        // Set recoil in player controller script to this weapon's recoil
        controller.recoil = dampenedRecoil * recoilDirection;

        // Reduce second recoil for smoothing
        dampenedRecoil *= 0.95f;
        recoilCounter = 0.1f;

        // If grounded, reduce recoil
        if (controller.m_Grounded)
        {
            dampenedRecoil *= 0.7f;

            // If player is holding against recoil, reduce even harder
            if ((GetComponent<PlayerMovement>().horizontalMovement > 0 && recoilDirection.x < 0)
                || (GetComponent<PlayerMovement>().horizontalMovement < 0 && recoilDirection.x > 0))
            {
                dampenedRecoil *= 0.7f;
            }
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

    private void StaticRecoil()
    {
        rb.AddForce(dampenedRecoil * recoilDirection);
        recoil = false;
        dampenedRecoil = recoilForce;
        recoilCounter = 0f;
    }

    private void Reload()
    {
        // Only play reload sound if actually reloaded
        bool reloaded = false;

        // Reload all guns
        foreach (IGun gun in guns)
        {
            if (gun.CurrentAmmo != gun.MaxAmmo)
            {
                gun.CurrentAmmo = gun.MaxAmmo;
                reloaded = true;
            }
        }

        if (reloaded)
        {
            AudioManager.instance.PlaySound("Reload");
        }
    }

    private void ResetRecentlyShot()
    {
        controller.recentlyShot = false;
    }

    private void SwitchGuns(int equip = -1)
    {
        // To specify which gun to equip
        if (equip > -1)
        {
            gunType = (GunType)equip;
            currentGun = guns[equip];
            return;
        }

        // If none specified, cycle to next gun
        gunType += 1;

        // Reset cycle if 
        if ((int)gunType > (guns.Count - 1))
        {
            gunType = 0;
        }

        // Equip next gun in cycle if there are guns in loadout
        if (guns.Count > 0 && equip == -1)
        {
            currentGun = guns[(int)gunType];
        }
    }

    public void CheckGuns()
    {
        updateGuns = false;
        string droppedGun = "";

        // Add weapons to player loadout
        if (pickedupGun == GunType.Pistol)
        {
            // Throw out old version and pick up new
            if (GetComponent<Pistol>() != null)
            {
                guns[(int)pickedupGun].CurrentAmmo = guns[(int)pickedupGun].MaxAmmo;

                droppedGun = "Pistol";
            }

            // Add to inventory
            else
            {
                gameObject.AddComponent<Pistol>();
                guns.Insert((int)pickedupGun, GetComponent<Pistol>());
            }
        }

        else if (pickedupGun == GunType.Deagle)
        {
            // Throw out old version and pick up new
            if (GetComponent<Deagle>() != null)
            {
                guns[(int)pickedupGun].CurrentAmmo = guns[(int)pickedupGun].MaxAmmo;

                droppedGun = "Deagle";
            }

            // Add to inventory
            else
            {
                gameObject.AddComponent<Deagle>();
                guns.Insert((int)pickedupGun, GetComponent<Deagle>());
            }
        }

        else if (pickedupGun == GunType.TeleGun)
        {
            // Throw out old version and pick up new
            if (GetComponent<TeleGun>() != null)
            {
                guns[(int)pickedupGun].CurrentAmmo = guns[(int)pickedupGun].MaxAmmo;

                droppedGun = "TeleGun";
            }

            // Add to inventory
            else
            {
                gameObject.AddComponent<TeleGun>();
                guns.Insert((int)pickedupGun, GetComponent<TeleGun>());
            }
        }

        // Equip new gun
        SwitchGuns((int)pickedupGun);

        if (pickedupGun != GunType.None)
        {
            currentGun.FireCounter = 0;
        }

        if (droppedGun.Length > 0)
        {
            // Particles
            GameObject particles = Instantiate(gunPickupParticles, transform.position, transform.rotation);
            particles.GetComponent<ParticleSystem>().textureSheetAnimation.RemoveSprite(0);
            particles.GetComponent<ParticleSystem>().textureSheetAnimation.AddSprite(Resources.Load<Sprite>(droppedGun));
        }

        pickedupGun = GunType.None;
    }
}
