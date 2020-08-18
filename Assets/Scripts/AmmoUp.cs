using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoUp : MonoBehaviour
{
    private GunManager weapon;
    public float activeRate = 0.1f; // Amount of refreshes in 1 sec
    private float cooldownCounter = 0;

    private void Start()
    {
        weapon = FindObjectOfType<GunManager>();
    }

    void Update()
    {
        // Countdown cooldown counter
        if (cooldownCounter >= 0)
        {
            cooldownCounter -= Time.deltaTime * activeRate;
        }

        // If off cooldown, show
        else
        {
            GetComponent<Animator>().enabled = true;
            GetComponent<Collider2D>().enabled = true;
            GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If player gets power up, and is able to make use of it
        if (collision.tag == "Player" && cooldownCounter <= 0 &&
            weapon.currentGun.CurrentAmmo < weapon.currentGun.MaxAmmo)
        {
            // Audio
            AudioManager.instance.PlaySound("AmmoUpGet");

            // Increase current weapon's ammo by 1
            weapon.currentGun.CurrentAmmo += 1;

            // Take gun off cooldown
            weapon.currentGun.FireCounter = 0f;

            // Reset cooldown counter
            cooldownCounter = 1;

            // Hide object
            GetComponent<Animator>().enabled = false;
            GetComponent<Collider2D>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}
