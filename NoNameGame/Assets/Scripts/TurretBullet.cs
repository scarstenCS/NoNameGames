﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBullet : MonoBehaviour
{
    public int damage = 1;
    public float speed = 2.5f;
    public float lifetime = 3f;
    public string attackTag = "PlayerAttack";
    private Rigidbody2D rb;
    private float startTime;
    public Vector3 initialDirection;


    void Awake()
    {
        // just for safety
        rb = GetComponent<Rigidbody2D>();
        var col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    void Start()
    {
        startTime = Time.time;
        // dir = initialDirection.sqrMagnitude > Mathf.Epsilon
        //                 ? initialDirection.normalized
        //                 : Vector2.right;    // fallback, just shoot right

        // rb.velocity = dir * speed;
    }
    void Update()
    {
        if (Time.time - startTime >= lifetime)
        {
            // auto-despawn so bullets do not live forever
            Destroy(this);
        }
        transform.position += Vector3.Normalize(initialDirection) * speed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            GameManager.PlayerTakeDamage(damage);
            Destroy(gameObject);
        }
        else if (other.tag == attackTag && BasicAttack.atkStage != 0)
        {
            Destroy(gameObject);
            BasicAttack.atkStage = 2;
        }

    }
}