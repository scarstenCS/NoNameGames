using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : MonoBehaviour
{
    private SpriteRenderer sr;
    public int hp = 1;
    public int atk = 1;
    public string attackTag = "PlayerAttack";

    public GameObject player;
    private Transform playerPos;
    private Rigidbody2D rb;
    Animator animator;
    public Animation idle;
    private bool isDead = false;
    public AudioClip deathClip;
    // Start is called before the first frame update
    void Start()
    {
        //AudioManager.SfxEnemy1Spawn();
        transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
        playerPos = player.GetComponent<Transform>();
        sr = gameObject.GetComponent<SpriteRenderer>();
        //animator = gameObject.GetComponent<Animator>();
        //animator.SetBool("Dead", false);
        //animator.SetFloat("WalkSpeed", 1f + speed / 1.5f);
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        var proj = other.GetComponent<BasicAttack>();
        proj = other.GetComponentInParent<BasicAttack>();
        if (other.tag == attackTag && proj.atkStage != 0)
        {
            //AudioManager.SfxEnemy2Hit();
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

       
}
