using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : AnimatedEntity
{
    public static int maxEnemies = 5;
    public static int enemyCount = 0;
    public int hp = 1;
    private IEnumerator spawnRoutine;
    // Start is called before the first frame update
    void Start()
    {
        AnimationSetup();
        spawnRoutine = Spawn();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyCount < maxEnemies)
        {
            StartCoroutine(spawnRoutine);
        }
        else
        {
            StopCoroutine(spawnRoutine);
        }
        if (hp <= 0)
        {
            Destroy(this);
        }
        this.transform.position += Vector3.Normalize(new Vector3(-this.transform.position.x, -this.transform.position.y)) * Time.deltaTime; 
    }

    public IEnumerator Spawn()
    {
        Vector3[] positions = { new Vector3(Random.Range(0, 2), Random.Range(0f, 1f)), new Vector3(Random.Range(0f, 1f), Random.Range(0,2))};
        Instantiate(new Enemy(), positions[Random.Range(0,2)], Quaternion.identity); 
        yield return new WaitForSeconds(2);
    }
}