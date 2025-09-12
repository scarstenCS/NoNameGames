using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public static int maxEnemies = 5;
    public static int enemyCount = 0;
    public int hp = 1;
    public float speed = 3;
    public Spawner spawner;
    // private IEnumerator spawnRoutine;
    // Start is called before the first frame update
    void Start()
    {
        // AnimationSetup();
        // spawnRoutine = Spawn();
        // spawner.Spawn();
    }

    // Update is called once per frame
    void Update()
    {
        // Instantiate(new Enemy(), new Vector3(Random.Range(0, 2), Random.Range(0f, 1f)), Quaternion.identity);
        // if (enemyCount < maxEnemies)
        // {
        //     StartCoroutine(spawnRoutine);
        // }
        // else
        // {
        //     StopCoroutine(spawnRoutine);
        // }
        // if (hp <= 0)
        // {
        //     Destroy(this);
        // }
        transform.position += new Vector3(-transform.position.x, -transform.position.y) * Time.deltaTime; 
    }

    // public IEnumerator Spawn()
    // {
    //     Vector3[] positions = { new Vector3(Random.Range(0, 2), Random.Range(0f, 1f)), new Vector3(Random.Range(0f, 1f), Random.Range(0,2))};
    //     Instantiate(new Enemy(), positions[Random.Range(0,2)], Quaternion.identity); 
    //     yield return new WaitForSeconds(2);
    // }
}