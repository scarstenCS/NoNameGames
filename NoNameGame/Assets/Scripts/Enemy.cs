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
        if (enemyCount < maxEnemies) {
            StartCoroutine(spawnRoutine);
        } else {
            StopCoroutine(spawnRoutine);
        }
        if (hp <= 0) {
            Destroy(this);
        }
    }

    public IEnumerator Spawn()
    {
        Instantiate(new Enemy(), this.transform.position, Quaternion.identity); 
        yield return new WaitForSeconds(2);
    }
}