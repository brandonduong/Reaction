using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Rigidbody2D rb;
    public GameObject impactEffect;

    public Vector2 direction;
    public float speed = 20f;
    public int damage = 50;

    public float lifeTime = 3f;

    // Start is called before the first frame update
    void Start()
    {
        // Move forward according to speed
        rb.velocity = direction * speed;

        // Destroy bullet after a certain amount of time to avoid prefab clutter
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player")
        {
            // Debug.Log(collision.name);

            // If collision object is breakable, reduce health
            Breakable breakable = collision.GetComponent<Breakable>();
            if (breakable != null)
            {
                breakable.TakeDamage(damage);
            }

            // Create effect + reference
            GameObject effect = (GameObject)Instantiate(impactEffect, transform.position, transform.rotation);

            // Destroy bullet
            Destroy(gameObject);

            // Destroy effect
            Destroy(effect, 1);
        }
    }
}
