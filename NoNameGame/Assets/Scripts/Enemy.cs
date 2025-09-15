using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int hp = 1;

    public GameObject player;
    private Transform playerPos;
    public float speed = 3;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
        playerPos = player.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hp <= 0)
        {
            Destroy(this);
        }
        transform.position += Vector3.Normalize(playerPos.position - transform.position) * Time.deltaTime * speed;
        Debug.Log(playerPos.position);
    }
}