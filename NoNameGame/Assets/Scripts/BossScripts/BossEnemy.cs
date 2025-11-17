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
    public TurretEnemy[] maskEnemies;

    public Transform targetObject;
    public float rotationSpeed = 360f;
    public float speed = .25f;
    public int teleportTrigger = 0;
    public int currentHealthLost = 0;
    public Transform bossRoot;
    public  Vector3[] maskPositions;
    private bool isResummoningMasks = false;
    private bool isResummoningMasksDelayed = false;
    public int difficulty = 1;
    private float delayResummonMasks = 10f;
    public float numOfTurretsAlive;
    private Coroutine delayResummonRoutine;
    private UserInterface ui;
    private int maxHealth;
    [SerializeField] private DialogueTrigger dialogueTrigger;
    private Renderer[] bossRenderers; 

    [SerializeField] private float turretDeathAnimTime = 1f;
    readonly int finalWaveNumber = 11;   

    private bool calledFinalDialogue = false;

    void Start()
    {
        //AudioManager.SfxEnemy1Spawn();
        transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
        playerPos = player.GetComponent<Transform>();
        sr = gameObject.GetComponent<SpriteRenderer>();

        //rb = gameObject.GetComponent<Rigidbody2D>();
       
        //cache mask positions
        if (maskPositions == null || maskPositions.Length == 0)
        {
            maskPositions = new Vector3[maskEnemies.Length];
            for (int i = 0; i < maskEnemies.Length; i++)
            {
                if (maskEnemies[i] != null)
                    maskPositions[i] = maskEnemies[i].transform.localPosition;
            }
        }
        
        CachePlayerPos();
        teleportTrigger = Mathf.FloorToInt(hp/3);
        //Determine boss root for movement
        if (bossRoot == null) {
            bossRoot = transform.parent != null ? transform.parent : transform;
        }
        // Reset mask enemies' parent to boss root and position
        if (transform.parent == bossRoot)
        {
            transform.localPosition = Vector3.zero;
        }
        maskEnemies = GetComponentsInChildren<TurretEnemy>();
        UnityEngine.Debug.Log("mask enemies length: " + maskEnemies.Length);
        numOfTurretsAlive = maskEnemies.Length;
        //animator = gameObject.GetComponent<Animator>();
        //animator.SetBool("Dead", false);
        //animator.SetFloat("FloatSpeed", 1f + speed / 1.5f);
        maxHealth = hp;
        ui = FindObjectOfType<UserInterface>();
        if (ui != null)
        {
            ui.SetActiveBossHealthBar(true);
            ui.OnBossHealthChanged(hp, maxHealth);
        }

        if (dialogueTrigger == null)
        {
            dialogueTrigger = DialogueTrigger.Instance;
        }
        UnityEngine.Debug.Log("Boss dialogue trigger set to " + dialogueTrigger);

        Transform root = bossRoot != null ? bossRoot : transform;
        bossRenderers = root.GetComponentsInChildren<Renderer>(includeInactive: true);
    }

        // Update is called once per frame
        void Update()
        {
            if (hp <= 0 && !isDead) {
                foreach (var turret in maskEnemies)
                {
                    if (turret != null 
                        && turret.gameObject.activeInHierarchy  
                        && !turret.IsDead)                     
                    {
                        turret.Kill(forTeleport: true);
                    }
                }
                int cachedDifficulty = difficulty;
                hp = 0;
                speed = 0f;
                if(EnemiesAliveNow() == 1)
                {
                    UnityEngine.Debug.Log("WaveManager.enemiesLeft: " + WaveManager.enemiesLeft);
                    if(calledFinalDialogue == false)
                    {
                        calledFinalDialogue = true;
                        StartCoroutine(FinalDialogue(cachedDifficulty));
                    }
                }

                
            }
            if (playerPos == null) CachePlayerPos();
            // Boss movement logic
            // center is the reference point for the boss and its mask enemies
            MovementAndRotation();
            //UnityEngine.Debug.Log("mask enemies: " + this.numOfTurretsAlive);

            if (this.numOfTurretsAlive < 3 && delayResummonRoutine == null)
            {
                delayResummonRoutine = StartCoroutine(DelayResummonMaskEnemies());
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
            UnityEngine.Debug.Log("num of turrets alive: " + this.numOfTurretsAlive);
            if(this.numOfTurretsAlive == 0)
            {
                //double damage if no masks are alive
                hp -= proj.Damage;
                currentHealthLost += proj.Damage;
                UnityEngine.Debug.Log("Boss took normal damage!");
            }
            else
            {
                hp -= 1;
                currentHealthLost += 1;
                UnityEngine.Debug.Log("Boss took reduced damage!");
            }
            if (hp > 0 && teleportTrigger - currentHealthLost <= 0 && hp > 3)
            {
                // pick a teleport location away from player
                //gameObject.GetComponent<Renderer>().enabled = false;
                //Transform[] transform = GetComponentsInChildren<Renderer>(includeInactive: true);
                
                gameObject.GetComponent<Renderer>().enabled = true;
                //gameObject.GetComponent<Renderer>().enabled = true;
                //Invoke(nameof(Teleport), 0.5f);
                foreach (var turret in maskEnemies)
                {
                    if (turret != null 
                        && turret.gameObject.activeInHierarchy  
                        && !turret.IsDead)                     
                    {
                        turret.Kill(forTeleport: true);
                    }
                }
                int cachedDifficulty = difficulty;
                StartCoroutine(TeleportDialogue(cachedDifficulty));

                currentHealthLost = 0;

                // always resummon masks after a teleport
                //StartCoroutine(ResummonMaskEnemies(true));
            }
            if (ui != null)
            {
                ui.OnBossHealthChanged(hp, maxHealth);
            }

        }
    }
    IEnumerator Die()
    {
        UnityEngine.Debug.Log("isDead: " + isDead);
        if (!isDead)
        {
            isDead = true;
        }

        WaveManager.enemiesLeft--;
        //AudioManager.SfxBossDeath();
        //animator.SetBool("Dead", true);
        isDead = true;
        if (ui != null)
        {
            ui.SetActiveBossHealthBar(false);
        }
        //TODO: Let animation do this
        AnimEventDestroySelf();
        yield break;
    }
    public void AnimEventDestroySelf() {
        Destroy(gameObject);
    }
    void CachePlayerPos()
    {
        var p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) playerPos = p.transform;
    }
    IEnumerator ResummonMaskEnemies(bool difficultyIncrease)
    {
        
        if (isResummoningMasks) yield break; 
        isResummoningMasks = true;
        //yield return new WaitForSeconds(3f); 

        maskEnemies = bossRoot.GetComponentsInChildren<TurretEnemy>(true); //even inactive ones
        //increase difficulty and stop previous delay if player does enough damage quickly
        if (difficultyIncrease) {
            difficulty++;
        }
        if(delayResummonRoutine!=null)
        {
            StopCoroutine(delayResummonRoutine);
            delayResummonRoutine = null;
            isResummoningMasksDelayed = false;
        }
        for (int i = 0; i < maskPositions.Length; i++)
        {
            TurretEnemy turret = maskEnemies[i];
            if (turret != null)
            {
                //reset position and parent
                turret.transform.SetParent(bossRoot != null ? bossRoot : transform, worldPositionStays: false);
                turret.transform.localPosition = maskPositions[i];
                turret.gameObject.SetActive(true);

                // Reset masks and increase difficulty
                speed = speed + (difficulty*0.1f);
                turret.ResetMasks(difficulty);
        }
        this.numOfTurretsAlive = 3;
        isResummoningMasks = false;
        }

        
    }
    IEnumerator DelayResummonMaskEnemies()
    {
        if (isResummoningMasksDelayed) yield break;
        isResummoningMasksDelayed = true;
        yield return new WaitForSeconds(delayResummonMasks);
        delayResummonRoutine = null;
        isResummoningMasksDelayed = false;
        StartCoroutine(ResummonMaskEnemies(false));
    }
    void Teleport()
    {
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
        Transform center = bossRoot != null ? bossRoot : transform;
        if (center != null)
        {
            center.position = teleportLocation;
        }
        //TeleportDialogue();
    }
    void MovementAndRotation()
    {
        //set reference point for movement and rotation
        Transform center = bossRoot != null ? bossRoot : transform;
        Vector3 inputVec = (Vector3)player.GetComponent<Player>().GetMove().ReadValue<Vector2>();
        Vector3 futurePos = playerPos.position + inputVec * player.GetComponent<Player>().Speed;
        
        Vector3 change;
        //move towards player
        if ((futurePos-center.position).magnitude <= 5)
        {
            change = playerPos.position - center.position;
        } else
        {
            change = futurePos - center.position;
        }
    
        change = Vector3.Normalize(change) * Time.deltaTime *speed;
        //moves and rotates all turret enemies around boss
        center.position += change;
        if (maskEnemies != null)
        {
            foreach (TurretEnemy turret in maskEnemies)
            {
            if (turret != null && turret.gameObject.activeInHierarchy)
            {
                turret.transform.RotateAround(center.position, Vector3.forward, rotationSpeed * Time.deltaTime);
            }
            }
        }
    }
    IEnumerator TeleportDialogue(int cachedDifficulty)
    {
        cachedDifficulty = cachedDifficulty + finalWaveNumber;
        UnityEngine.Debug.Log("Starting teleport dialogue for dialogue Sequence " + cachedDifficulty);
        yield return new WaitForSeconds(turretDeathAnimTime);
        dialogueTrigger.OnWaveEnd(cachedDifficulty);
        yield return new WaitUntil(dialogueTrigger.manager.isDialogueFinished);
        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).gameObject.SetActive(true);
        
        Teleport();
        SetBossVisible(true); 
        StartCoroutine(ResummonMaskEnemies(true));
        
    }
    private void SetBossVisible(bool visible)
    {
        if (bossRenderers == null) return;

        foreach (var r in bossRenderers)
        {
            if (r != null)
                r.enabled = visible;
        }
    }
    IEnumerator FinalDialogue(int cachedDifficulty)
    {
        int index = finalWaveNumber + cachedDifficulty;
        yield return new WaitForSeconds(turretDeathAnimTime);

        dialogueTrigger.OnWaveEnd(index);

        yield return new WaitUntil(dialogueTrigger.manager.isDialogueFinished);

        yield return new WaitForSeconds(0.1f);
        yield return StartCoroutine(Die());
    }
    int EnemiesAliveNow()
    {
        return WaveManager.runnerCount 
            + WaveManager.turretCount 
            + WaveManager.bossCount 
            + WaveManager.enemiesLeft 
            - WaveManager.maxEnemies;
    }
}
