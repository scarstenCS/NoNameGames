using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretEnemy : MonoBehaviour
{

    public int hp = 2;
    public int atk = 0;
    public string attackTag = "PlayerAttack";
    public float cooldown = 1.0f;

    public GameObject player;
    private Transform playerPos;
    public GameObject bulletPrefab;

    public float shootCooldown = 1.25f;
    public float bulletSpeed = 7f;
    public float bulletLifetime = 3f;
    public int bulletDamage = 1;

    private float _nextShootTime;
    private float offset = 0.35f;

    void Start()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
        playerPos = player.GetComponent<Transform>();
    }

    void Update()
    {
        if (hp <= 0)
        {
            Destroy(gameObject);
            WaveManager.enemiesLeft--;
        }

        if (Time.time >= _nextShootTime)
        {
            FireOnce();
            _nextShootTime = Time.time + shootCooldown;
        }
        
    }


    void FireOnce()
    {
        // aiming at playr
        Vector2 dir = ((Vector2)(playerPos.position - transform.position)).normalized;

        // bullet spawn position
        Vector3 spawnPos = transform.position + (Vector3)(dir * offset);

        GameObject go = Instantiate(bulletPrefab, spawnPos, Quaternion.identity);

        if (go.TryGetComponent<TurretBullet>(out var tb)) {
            tb.initialDirection = dir;
            tb.speed = bulletSpeed;
            tb.lifetime = bulletLifetime;
            tb.damage = bulletDamage;
        } else if (go.TryGetComponent<Rigidbody2D>(out var rb)) {
            rb.velocity = dir * bulletSpeed;
            Destroy(go, bulletLifetime);
        }

        // // avoid instant self-hit
        // if (TryGetComponent<Collider2D>(out var turretCol) &&
        //     go.TryGetComponent<Collider2D>(out var bulletCol)) {
        //     Physics2D.IgnoreCollision(turretCol, bulletCol, true);
        // }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == attackTag && BasicAttack.atkStage != 0)
        {
            AudioManager.SfxEnemyHit();
            BasicAttack.atkStage = 2;
            hp--;
        }
    }


}