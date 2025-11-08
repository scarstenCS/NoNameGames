using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

//using System.ComponentModel;
using System.Runtime.Serialization;
using UnityEngine;
public class BossEnemy : MonoBehaviour
{
    private SpriteRenderer sr;
    public int hp = 10;
    public int atk = 1;
    public string attackTag = "PlayerAttack";

    public GameObject player;
    private Transform playerPos;
    private Rigidbody2D rb;
    Animator animator;
    public Animation idle;
    private bool isDead = false;
    public AudioClip deathClip;
    public Component[] maskEnemies;
    // Start is called before the first frame update
    void Start()
    {
        //AudioManager.SfxEnemy1Spawn();
        transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
        playerPos = player.GetComponent<Transform>();
        sr = gameObject.GetComponent<SpriteRenderer>();
        //rb = gameObject.GetComponent<Rigidbody2D>();
        maskEnemies = GetComponentsInChildren<HingeJoint>();
        UnityEngine.Debug.Log("Start");

        //animator = gameObject.GetComponent<Animator>();
        //animator.SetBool("Dead", false);
        //animator.SetFloat("WalkSpeed", 1f + speed / 1.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (hp <= 0 && !isDead) Die(); 
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        UnityEngine.Debug.Log("GotHurt");
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
            UnityEngine.Debug.Log("hp is: " + hp);

        }
    }
    private void Die()
    {
        if (isDead) return;

        WaveManager.enemiesLeft--;
        //AudioManager.SfxEnemy2Death();
        //animator.SetBool("Dead", true);
        isDead = true;
        GameObject foo;
        foo=transform.parent.gameObject;
        transform.parent=null;
        Destroy(foo);
        //TODO: Let animation do this
        AnimEventDestroySelf();
    }
    public void AnimEventDestroySelf() {
        Destroy(gameObject);
    }

       
}
