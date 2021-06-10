using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Rigidbody2D rb;
    public GameObject impactEffect;

    public Vector2 direction;

    private float speed;
    public int damage;

    public float lifeTime = 3f;

    public Sprite[] bulletSprites;
    public IGun currentGun;

    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        // Gather current gun info
        player = FindObjectOfType<PlayerController2D>().gameObject;
        currentGun = FindObjectOfType<GunManager>().currentGun;
        damage = currentGun.BulletDamage;
        speed = currentGun.BulletSpeed;
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        sprite.sprite = bulletSprites[(int)currentGun.Type];

        // Move forward according to speed
        rb.velocity = direction * speed;

        // Destroy bullet after a certain amount of time to avoid prefab clutter
        Destroy(gameObject, lifeTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (currentGun.Type == GunType.TeleGun)
        {
            Debug.Log("Teleport");
            player.transform.position = gameObject.transform.position;
        }

        // Create effect + reference
        GameObject effect = (GameObject)Instantiate(impactEffect, transform.position, transform.rotation);

        // Audio
        AudioManager.instance.PlaySound("BulletCollision");

        // Destroy bullet
        Destroy(gameObject);

        // Destroy effect
        Destroy(effect, 1);
    }
}
