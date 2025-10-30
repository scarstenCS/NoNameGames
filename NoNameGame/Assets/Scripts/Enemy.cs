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
    private float followSkill;
    private Rigidbody2D rb;
    Animator animator;
    public Animation idle;
    private bool isDead = false;
    public AudioClip deathClip;
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.SfxEnemy1Spawn();
        transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
        playerPos = player.GetComponent<Transform>();
        sr = gameObject.GetComponent<SpriteRenderer>();
        animator = gameObject.GetComponent<Animator>();
        animator.SetBool("Dead", false);
        animator.SetFloat("WalkSpeed", 1f + speed / 1.5f);
        followSkill = Random.Range(0f, 1f);
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
        Vector3 change = Vector3.Normalize(futurePos - transform.position) * Time.deltaTime * speed;
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
        WaveManager.enemiesLeft--;

        if (!isDead)
        {
            AudioManager.SfxEnemy1Death();
            animator.SetBool("Dead", true);
        }
        isDead = true;
    }
    public void AnimEventDestroySelf() {
        Destroy(gameObject);
    }
}
