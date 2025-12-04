using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private SpriteRenderer sr;
    public int hp = 1;
    public int atk = 1;
    public string attackTag = "PlayerAttack";

    public GameObject player;
    private Transform playerPos;
    public float speed = 3;
    public float cooldown = 1.5f;
    public float _lastAtkTime;
    public float followSkill;
    private Rigidbody2D rb;
    Animator animator;
    public Animation idle;
    private bool isDead = false;
    public AudioClip deathClip;
    private Color colour;
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.SfxEnemy1Spawn();
        transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
        //Determine if part of boss
        var boss = GetComponentInParent<BossEnemy>();
        if (boss != null) playerPos = boss.playerPos;
        else playerPos = player.GetComponent<Transform>();
        
        sr = gameObject.GetComponent<SpriteRenderer>();
        animator = gameObject.GetComponent<Animator>();
        animator.SetBool("Dead", false);
        animator.SetFloat("WalkSpeed", 1f + speed / 1.5f);
        colour = sr.color;
        if (followSkill >= 0.5)
        {
            colour = Color.red;
            this.GetComponent<Renderer>().material.SetColor("_Color", colour);
        }
    }

    void Awake()
    {
        float multiplier = Random.Range(0f, 1.25f);
        speed += multiplier * 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead) return;

        if (hp <= 0 && !isDead) Die(); 
        
        Vector3 inputVec = (Vector3)player.GetComponent<Player>().GetMove().ReadValue<Vector2>();
        Vector3 futurePos = playerPos.position + inputVec * followSkill * player.GetComponent<Player>().Speed;
        Vector3 change;
        if ((futurePos-transform.position).magnitude <= 5)
        {
            change = playerPos.position - transform.position;
        } else
        {
            change = futurePos - transform.position;
        }
        change = Vector3.Normalize(change) * Time.deltaTime * speed;
        transform.position += change;
        sr.flipX = change.x >= 0;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var proj = other.GetComponent<BasicAttack>();
        proj = other.GetComponentInParent<BasicAttack>();
        
        if (other.tag == attackTag && proj.atkStage != 0)
        {
            
            AudioManager.SfxEnemy1Hit();
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
    private void OnCollisionStay2D(Collision2D coll)
    {
        GameObject other = coll.collider.gameObject;
        if (other.tag == "Player")
        {
            animator.SetTrigger("Punch");
        }
    }
    private void Punch()
    {
        int dmg = atk;
        GameManager.PlayerTakeDamage(dmg);
        AudioManager.SfxPlayerHit();
        animator.ResetTrigger("Punch");
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
                ui.AddScore(10);
            }
        }

        AudioManager.SfxEnemy1Death();
        animator.SetBool("Dead", true);
        isDead = true;
    }
    public void AnimEventDestroySelf() {
        Destroy(gameObject);
    }
    void Flash()
    {
        sr.color = sr.color = new Color(0.0f, 0.0f, 1.0f, 0.4f); // semi-transparent blue
        Invoke("ResetColor", 0.2f);
    }
    void ResetColor()
    {
        sr.color = colour;
    }
}
