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
    private Rigidbody2D rb;
    Animator animator;
    public Animation idle;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
        playerPos = player.GetComponent<Transform>();
        sr = gameObject.GetComponent<SpriteRenderer>();
        animator.SetBool("isWalking", true);
    }

    void Awake()
    {
        float multiplier = Random.Range(0f, 1f);
        speed += multiplier * 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (hp <= 0)
        {
            Destroy(gameObject);
            WaveManager.enemiesLeft--;

        }
        Vector3 change = Vector3.Normalize(playerPos.position - transform.position) * Time.deltaTime * speed;
        transform.position += change;
        sr.flipX = change.x >= 0;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        var proj = other.GetComponent<BasicAttack>();
        proj = other.GetComponentInParent<BasicAttack>();

        if (other.tag == attackTag && proj.atkStage != 0)
        {
            AudioManager.SfxEnemyHit();
            if (proj.pierce <= 0)
            {
                proj.atkStage = 2;
            }
            else
            {
                proj.pierce -= 1;
            }
            hp-=proj.Damage;
        }
    }
}
