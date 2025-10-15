using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public int damage = 1;
    public float speed = 7f;
    public float lifetime = 3f;

    private Rigidbody2D rb;

    public Vector2 initialDirection = Vector2.right;


    void Awake()
    {
        // just for safety
        rb = GetComponent<Rigidbody2D>();
        var col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    void Start()
    {
        Vector2 dir = initialDirection.sqrMagnitude > Mathf.Epsilon
                        ? initialDirection.normalized
                        : Vector2.right;    // fallback, just shoot right

        rb.velocity = dir * speed;


        // auto-despawn so bullets do not live forever
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.PlayerTakeDamage(damage);
            Destroy(gameObject);
        }

    }

}
