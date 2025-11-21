using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class TurretEnemy : MonoBehaviour
{

    public int hp = 2;
    public int atk = 0;
    public string attackTag = "PlayerAttack";
    //public float cooldown = 1.0f;
    Animator animator;

    public GameObject player;
    private Transform playerPos;
    public GameObject bulletPrefab;
    private SpriteRenderer sr;
    public float shootCooldown = 1.25f;
    public float bulletSpeed = 7f;
    public float bulletLifetime = 3f;
    public int bulletDamage = 1;
    public float skill;
    private float _nextShootTime;
    private bool isDead = false;
     public bool IsDead => isDead; 
    public bool isPartOfBoss;
    private int hpMax;
    private int atkInitial;
    private float cooldownInitial;
    private float bulletSpeedInitial;
    private int bulletDamageInitial;
    private float bulletshootCooldownInitial;
    public float deathAnimSpeed = 1.75f;
    private Color colour;


    void Start()
    {
        AudioManager.SfxEnemy2Spawn();
        _nextShootTime = Time.time + shootCooldown;
        transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
        
        //Determine if part of boss prefab
        var boss = GetComponentInParent<BossEnemy>();
        if (boss != null) {
            playerPos = boss.playerPos;
            isPartOfBoss = true;
        }
        else {
            playerPos = player.GetComponent<Transform>();
            isPartOfBoss = false;
        }   
        hpMax = hp;
        atkInitial = atk;
        cooldownInitial = shootCooldown;
        bulletSpeedInitial = bulletSpeed;
        bulletDamageInitial = bulletDamage;
        bulletshootCooldownInitial = shootCooldown;

        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        animator.SetBool("Dead", false);
        animator.SetFloat("AnimationSpeed", 1.25f / shootCooldown);
        colour = sr.color;
        if (skill >= 0.5 && !isPartOfBoss)
        {
            colour = Color.red;
            this.GetComponent<Renderer>().material.SetColor("_Color", colour);
        }
        //animator.SetFloat("ShootSpeed", clipLength / shootCooldown);
    }

    void Update()
    {
        if (isDead) return;
        
        if (playerPos != null)
        {
            Vector3 move = Vector3.Normalize(playerPos.position - transform.position);
            sr.flipX = move.x > 0;
        }

        
        if (hp <= 0 && !isDead) Die(); 

        if (Time.time >= _nextShootTime)
        {
            
            animator.SetTrigger("Shoot");
            _nextShootTime = Time.time + shootCooldown;
        }
    }
    public void onShootAnimationEvent()
    {
        if (playerPos == null) return;
        if (hp <= 0) return;
        FireOnce();
        animator.ResetTrigger("Shoot");
    }
    public void AnimEventDestroySelf() {
        if (!isDead) return;
        UnityEngine.Debug.Log("part of boss: " + isPartOfBoss);
        if(isPartOfBoss == true) gameObject.SetActive(false);
        else
        {
            Destroy(gameObject);
        }
        animator.ResetTrigger("Shoot");
        animator.SetFloat("DeathSpeed", 1f);
    }


    void FireOnce()
    {
        // aiming ahead of player
        Vector3 inputVec = (Vector3)player.GetComponent<Player>().GetMove().ReadValue<Vector2>();
        Vector3 futurePos = playerPos.position + inputVec * skill * player.GetComponent<Player>().Speed;
        Vector3 dir = Vector3.Normalize(futurePos - transform.position);

        // bullet spawn position
        Vector3 spawnPos = transform.position + dir;

        GameObject go = Instantiate(bulletPrefab, spawnPos, Quaternion.identity);
        TurretBullet tb = go.GetComponent<TurretBullet>();
        tb.initialDirection = dir;
        tb.speed = bulletSpeed;
        tb.lifetime = bulletLifetime;
        tb.damage = bulletDamage;
        AudioManager.SfxTurretShoot();
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        var proj = other.GetComponent<BasicAttack>();
        proj = other.GetComponentInParent<BasicAttack>();
        if (other.tag == attackTag && proj.atkStage != 0)
        {
            if (isPartOfBoss) {
                AudioManager.SfxMaskHit();
            }
            else
            {
               AudioManager.SfxEnemy2Hit(); 
            }
            if (proj.pierce <= 0)
            {
                proj.atkStage = 2;
            }
            else
            {
                proj.pierce -= 1;
            }
            hp -= proj.Damage;
            if (hp > 0) Flash();

        }
    }
    
    private void Die()
    {
        if (isDead) return;

        bool isBossTurret = GetComponentInParent<BossEnemy>() != null;

        if (!isBossTurret)
        {
            // Only standalone turrets affect the wave counter
            WaveManager.enemiesLeft--;
            UserInterface ui = FindObjectOfType<UserInterface>();
            if (ui != null)
            {
                ui.AddScore(20);
            }
        }
        else
        {
            BossEnemy boss = GetComponentInParent<BossEnemy>();
            boss.numOfTurretsAlive--;
        }

        if (!isPartOfBoss) AudioManager.SfxEnemy2Death();
        animator.SetBool("Dead", true);
        isDead = true;
    }
    public void ResetMasks(int difficulty)
    {
        BossEnemy boss = GetComponentInParent<BossEnemy>();
        this.hp = hpMax * difficulty;
        isDead = false;
        animator.SetBool("Dead", false);
        // this.atk = atkInitial * difficulty;
        this.shootCooldown = cooldownInitial/difficulty;
        this.bulletSpeed = bulletSpeedInitial;
        // this.bulletDamage = bulletDamageInitial*difficulty;
        boss.numOfTurretsAlive=3;
    }
    void Flash()
    {
        sr.color = new Color(0.0f, 0.0f, 1.0f, 0.4f); // semi-transparent blue
        Invoke("ResetColor", 0.2f);
    }
    void ResetColor()
    {
        sr.color = colour;
    }
    public void Kill(bool forTeleport = false)
    {
        if (forTeleport)
        {
            if (isDead) return;

            isDead = true;
            animator.ResetTrigger("Shoot");
            animator.SetFloat("DeathSpeed", deathAnimSpeed);
            animator.SetBool("Dead", true);
            
            
            var boss = GetComponentInParent<BossEnemy>();
            if (boss != null)
            {
                boss.numOfTurretsAlive--;
            }

            return;
        }

        Die();
    }


}