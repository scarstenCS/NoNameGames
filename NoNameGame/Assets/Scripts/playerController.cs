using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    private Transform t;
    public int health = 20;
    public float startSpeed = 1;

    // Start is called before the first frame update
    void Start()
    {
        t = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnMove(InputValue movementValue)
    {
        t.position += new Vector2(movementValue.x, movementValue.y);
    }
}
