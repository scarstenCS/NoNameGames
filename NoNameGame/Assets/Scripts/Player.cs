using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Transform t;

    public int maxHealth = 20;
    private int health;
    public float startSpeed = 1;
    private float playerSpeed;

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

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        t = GetComponent<Transform>();
        playerSpeed = startSpeed;

        ba = basicAttackObj.GetComponent<BasicAttack>();
    }

    // Update is called once per frame
    void Update()
    {
        t.position += (Vector3)move.ReadValue<Vector2>() * Time.deltaTime * playerSpeed;

        if (basicAtkAction.triggered && basicAtkAction.ReadValue<float>()>0)
        {
            ba.Attack();
        }
    }
}
