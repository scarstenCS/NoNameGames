using System.Collections;
using System;                 
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Transform t;
    private SpriteRenderer sr;
    public const string enemyTag = "Enemy";

    private int _maxHealth =20;
    private bool shieldGenerated;
    public int MaxHealth
    {

        get
        {
            return _maxHealth;
        }
        set
        {
            _maxHealth = value;
            HealthChanged?.Invoke(health, MaxHealth);
        }
    }
    private int health;
    public int Health => health;


    /// <summary>
    /// Damage for basic attack
    /// </summary>
    public int basicWeaponDmg
    {
        get
        {
            return ba.Damage;
        }

        set
        {
            ba.Damage = value;
        }
    }
    /// <summary>
    /// Max distance for basic weapon
    /// </summary>
    public float basicWeaponDistance
    {
        get
        {
            return ba.projectileMaxDistance;
        }
        set
        {
            ba.projectileMaxDistance = value;
        }
    }
    
    public float basicWeaponSpeed
    {
        get
        {
            return ba.projectileSpeed;
        }
        set
        {
            ba.projectileSpeed = value;
        }
    }

    public int basicWeaponPierce
    {
        get
        {
            return ba.maxPierce;
        }
        set
        {
            ba.maxPierce = value;
        }
    }
    public Vector3 basicWeaponSize
    {
        get
        {
            return ba.transform.localScale;
        }
        set
        {
            ba.transform.localScale = value;
        }
    } 
    public event Action<int, int> HealthChanged;
    public event Action OnDied;
    bool isDead = false;

    public float startSpeed = 1;

    private float playerSpeed;
    public int totalShieldCount;
    private int currentShieldCount;
    public float sheildRegenerateTime = 5f;

    public float Speed
    {
        get
        {
            return playerSpeed;
        }
        set
        {
            playerSpeed = value;
        }
    }
    public int totalBasicAttacksCount
    {
        get
        {
            return ba.numberOfProjectiles;
        }
        set
        {
            ba.numberOfProjectiles = value;
        }
    }   
   
    public PlayerControls controls;

    private InputAction move;

    private InputAction basicAtkAction;

    private InputAction pauseGame;

    public GameObject basicAttackObj;

    private BasicAttack ba;
    public Animator animator;

    public Animation idle;
    private UserInterface ui;
    private Color color;


    private void Awake()
    {
        controls = new PlayerControls();
    }
    private void OnEnable()
    {
        move = controls.Player.Move;
        basicAtkAction = controls.Player.BasicAttack;
        pauseGame = controls.Player.Pause;
        basicAtkAction.Enable();
        pauseGame.Enable();
        move.Enable();
    }

    private void OnDisable()
    {
        move.Disable();
        basicAtkAction.Disable();
        pauseGame.Disable();
    }
    /// <summary>
    /// Makes the player take damage.
    /// </summary>
    /// <param name="amount">the ammount of damage to do to player</param>
    public void TakeDamage(int amount)
    {
        if(currentShieldCount > 0)
        {
            currentShieldCount--;
            //flash blue and play sfx sound
            sr.color = new Color(0.0f, 0.0f, 1.0f, 0.7f); // semi-transparent blue
            Invoke("ResetColor", 0.2f);
            return;
        }
        if (amount <= 0) return;
        sr.color = new Color(1.0f, 0.0f, 0.0f, 0.7f); // semi-transparent red
        Invoke("ResetColor", 0.2f);
        health = Mathf.Max(0, health - amount);
        HealthChanged?.Invoke(health, MaxHealth);
        StopCoroutine("regenerateBlocks");
        shieldGenerated = false;
        if (health == 0 && !isDead)
        {
            isDead = true;
            AudioManager.SfxPlayerDeath();
            AudioManager.Instance.StopMusic();
            move.Disable();
            basicAtkAction.Disable();
            animator.SetBool("Dead", true);
            

        } 
        if (ui != null)
        {
            ui.ReduceScore(5*amount);
        }
    }
    /// <summary>
    /// heals the player
    /// </summary>
    /// <param name="amount">the ammount to heal player by</param>
    public void Heal(int amount)
    {
        if (amount <= 0) return;
        health = Mathf.Min(MaxHealth, health + amount);
        HealthChanged?.Invoke(health, MaxHealth);
        //assumed when healed its the end of wave so reset blocks
        currentShieldCount = totalShieldCount;
    }


    // Start is called before the first frame update
    void Start()
    {
        health = MaxHealth;
        t = GetComponent<Transform>();
        playerSpeed = startSpeed;

        HealthChanged?.Invoke(health, MaxHealth);

        ba = basicAttackObj.GetComponent<BasicAttack>();

        animator = gameObject.GetComponent<Animator>();

        sr = GetComponent<SpriteRenderer>();
        color = sr.color;
        ui = FindObjectOfType<UserInterface>();
        currentShieldCount = totalShieldCount;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 inputVector = move.ReadValue<Vector2>();
        t.position += (Vector3)inputVector * Time.deltaTime * playerSpeed;
        t.position = new Vector3(Mathf.Clamp(t.position.x, GameManager.minX, GameManager.maxX), Mathf.Clamp(t.position.y, GameManager.minY, GameManager.maxY));
        
        sr.flipX = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()).x >= t.position.x;
        if (move.ReadValue<Vector2>() != Vector2.zero)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
        if (basicAtkAction.triggered && basicAtkAction.ReadValue<float>() > 0)
        {
            UnityEngine.Debug.Log("Attack");
            animator.SetBool("isAttacking", true);
            
            ba.Attack();
        }
        if (pauseGame.triggered && pauseGame.ReadValue<float>() > 0)
        {
            //yield return new WaitForSeconds(0.1f);
            GameManager.TogglePause();
        }
        if (currentShieldCount < totalShieldCount && !shieldGenerated)
        {
            shieldGenerated = true;
            StartCoroutine("regenerateBlocks");
        }
    }

    private void HandleDeath()
    {
        // TODO: Implement death handling (e.g., play animation, disable player controls, etc.)'
        gameObject.SetActive(false);
        isDead = true;
        OnDied?.Invoke();

    }
    public InputAction GetMove()
    {
        return move;
    }
    void ResetColor()
    {
        sr.color = color;
    }
    private IEnumerator regenerateBlocks()
    {
        if (currentShieldCount < totalShieldCount)
        {
            yield return new WaitForSeconds(sheildRegenerateTime);
            currentShieldCount++;
            sr.color = new Color(0.0f, 1.0f, 0.0f, 0.3f); // semi-transparent green
            Invoke("ResetColor", 0.2f);
            //play sfx sound
            shieldGenerated = false;
        }
    }
}
