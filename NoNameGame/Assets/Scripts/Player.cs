using System.Collections;
using System;                 // <-- add this
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Transform t;
    public const string enemyTag = "Enemy";
    public int maxHealth = 20;
    private int health;
    public int Health => health;

    public event Action<int, int> HealthChanged;


    public float startSpeed = 1;
    private float playerSpeed;
    public int minX = -2;
    public int maxX = 2;
    public int minY = -2;
    public int maxY = 2;
    public PlayerControls controls;

    private InputAction move;

    private InputAction basicAtkAction;

    public GameObject basicAttackObj;

    private BasicAttack ba;

    private void Awake()
    {
        controls = new PlayerControls();
    }
    private void OnEnable()
    {
        move = controls.Player.Move;
        basicAtkAction = controls.Player.BasicAttack;
        basicAtkAction.Enable();
        move.Enable();
    }

    private void OnDisable()
    {
        move.Disable();
        basicAtkAction.Disable();
    }
    /// <summary>
    /// Makes the player take damage.
    /// </summary>
    /// <param name="amount">the ammount of damage to do to player</param>
    public void TakeDamage(int amount)
    {
        if (amount <= 0) return;
        health = Mathf.Max(0, health - amount);
        HealthChanged?.Invoke(health, maxHealth);
        // TODO: if (health == 0) HandleDeath();
    }
    /// <summary>
    /// heals the player
    /// </summary>
    /// <param name="amount">the ammount to heal player by</param>
    public void Heal(int amount)
    {
        if (amount <= 0) return;
        health = Mathf.Min(maxHealth, health + amount);
        HealthChanged?.Invoke(health, maxHealth);
    }

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        t = GetComponent<Transform>();
        playerSpeed = startSpeed;

        HealthChanged?.Invoke(health, maxHealth);

        ba = basicAttackObj.GetComponent<BasicAttack>();
    }

    // Update is called once per frame
    void Update()
    {
        t.position += (Vector3)move.ReadValue<Vector2>() * Time.deltaTime * playerSpeed;
        t.position = new Vector3(Mathf.Clamp(t.position.x, minX, maxX), Mathf.Clamp(t.position.y, minY, maxY));

        if (basicAtkAction.triggered && basicAtkAction.ReadValue<float>() > 0)
        {
            ba.Attack();
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Enemy")
        {
            this.TakeDamage(other.gameObject.GetComponent<Enemy>().atk);
        }
    }
}
