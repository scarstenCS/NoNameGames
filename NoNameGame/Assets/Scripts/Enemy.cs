using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int hp = 1;

    public string attackTag = "PlayerAttack";

    public GameObject player;
    private Transform playerPos;
    public float speed = 3;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
        playerPos = player.GetComponent<Transform>();
    }

    void Awake()
    {
        float multiplier = Random.Range(0f, 1f);
        speed += multiplier * 2;
        GetComponent<Renderer>().material.color = new Color(multiplier, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (hp <= 0)
        {
            Destroy(gameObject);
            Spawner.enemyCount--;

        }
        transform.position += Vector3.Normalize(playerPos.position - transform.position) * Time.deltaTime * speed;
    }

    private void OnTriggerEnter2D(Collider2D other) {

        if (other.tag == attackTag && BasicAttack.atkStage != 0)
        {
            BasicAttack.atkStage = 2;
            hp--;
        }
    }
}