﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPickup : MonoBehaviour
{
    private GunManager gunManager;
    public GunType gun = GunType.Pistol;

    private void Start()
    {
        gunManager = FindObjectOfType<GunManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If player gets power up, and is able to make use of it
        if (collision.tag == "Player")
        {
            // Add gun to loadout
            switch (gun)
            {
                case (GunType.Pistol):
                    gunManager.pickedupGun = GunType.Pistol;
                    break;

                case (GunType.Deagle):
                    gunManager.pickedupGun = GunType.Deagle;
                    break;

                case (GunType.TeleGun):
                    gunManager.pickedupGun = GunType.TeleGun;
                    break;
            }

            // Update guns
            gunManager.updateGuns = true;

            // Audio
            AudioManager.instance.PlaySound("AmmoUpGet");

            // Destroy object
            Destroy(gameObject);
        }
    }
}
