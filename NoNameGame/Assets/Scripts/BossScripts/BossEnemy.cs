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
    public Transform playerPos;
    private Rigidbody2D rb;
    Animator animator;
    public Animation idle;
    private bool isDead = false;
    public AudioClip deathClip;
    public Component[] maskEnemies;
    public Transform targetObject;
    public float rotationSpeed = 360f;
    public float speed = .25f;
    public int teleportTrigger = 0;
    public int currentHealthLost = 0;
    public Transform bossRoot;
    // Start is called before the first frame update
    void Start()
    {
        //AudioManager.SfxEnemy1Spawn();
        transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
        playerPos = player.GetComponent<Transform>();
        sr = gameObject.GetComponent<SpriteRenderer>();

        //rb = gameObject.GetComponent<Rigidbody2D>();
        maskEnemies = GetComponentsInChildren<TurretEnemy>();
        UnityEngine.Debug.Log("maskEnemies Count: " + maskEnemies.Length);
        CachePlayerPos();
        teleportTrigger = Mathf.FloorToInt(hp/3);
        if (bossRoot == null) {
            bossRoot = transform.parent != null ? transform.parent : transform;
        }
        //animator = gameObject.GetComponent<Animator>();
        //animator.SetBool("Dead", false);
        //animator.SetFloat("WalkSpeed", 1f + speed / 1.5f);
        }

        // Update is called once per frame
        void Update()
        {
            if (hp <= 0 && !isDead) Die();

            if (playerPos == null) CachePlayerPos();

            Vector3 inputVec = (Vector3)player.GetComponent<Player>().GetMove().ReadValue<Vector2>();
            Vector3 futurePos = playerPos.position + inputVec * player.GetComponent<Player>().Speed;
            Vector3 change;
            if ((futurePos-transform.position).magnitude <= 5)
            {
                change = playerPos.position - transform.position;
            } else
            {
                change = futurePos - transform.position;
            }
            change = Vector3.Normalize(change) * Time.deltaTime *speed;
            //moves and rotates all turret enemies around boss
            if (maskEnemies.Length > 0)
            {
                foreach (TurretEnemy turret in maskEnemies)
                {
                if (turret != null)
                {
                    turret.transform.RotateAround(transform.position, Vector3.forward, rotationSpeed * Time.deltaTime);
                    transform.position += (change);
                }
                }
            }
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
            currentHealthLost += proj.Damage;
            if (hp > 0 && teleportTrigger - currentHealthLost <= 0 && hp > 3)
            {
                //play teleport sound
                //TODO: move entire prefab to random location
                float minDistance = 7f;

                CachePlayerPos();
                Vector3 teleportLocation;
                do
                {
                    float randX = Random.Range(GameManager.minX, GameManager.maxX);
                    float randY = Random.Range(GameManager.minY, GameManager.maxY);
                    teleportLocation = new Vector3(randX, randY, 0f);
                }
                while (Vector3.Distance(teleportLocation, playerPos.position) < minDistance);

                UnityEngine.Debug.Log("Teleporting Boss to: " + teleportLocation);
                UnityEngine.Debug.Log("Player is at: " + playerPos.position);

                if (bossRoot != null)
                {
                    bossRoot.position = teleportLocation;
                }
                currentHealthLost = 0;
            }

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
    void CachePlayerPos()
    {
        var p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) playerPos = p.transform;
    }
       
}
