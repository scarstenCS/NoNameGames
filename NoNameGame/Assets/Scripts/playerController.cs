using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerController : MonoBehaviour
{
    private Transform t;
    public int health = 20;
    public float startSpeed = 1;

    public PlayerControls controls;

    private InputAction move; 

    private void Awake() {
        controls = new PlayerControls();
    }
    private void OnEnable()
    {
        move = controls.Player.Move;
        move.Enable();
    }

    private void OnDisable()
    {
        move.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        t = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        t.position += (Vector3) move.ReadValue<Vector2>();
    }
    // private void OnMove(Input movementValue)
    // {
    //     t.position += new Vector2(movementValue.x, movementValue.y);
    // }
}
