using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Camera mainCamera;
    public GameObject player;
    public static int maxEnemies = 5;
    public static int enemyCount = 0;
    public bool started = false;
    private IEnumerator wait;
    public void Start()
    {
        mainCamera = Camera.main;
        wait = new WaitForSecondsRealtime(2f);
        StartCoroutine(Spawn());

    }

    public void Update()
    {
        // if (!started)
        // {
        //     StartCoroutine(Spawn());
        //     started = true;
        // }
    }

    public IEnumerator Spawn()
    {
        while (enemyCount < maxEnemies)
        {
            Vector3[] positions = { new Vector3(Random.Range(0, 2), Random.Range(0f, 1f)), new Vector3(Random.Range(0f, 1f), Random.Range(0, 2)) };
            GameObject e = Instantiate(enemyPrefab, mainCamera.ViewportToWorldPoint(positions[Random.Range(0, 2)]), Quaternion.identity);
            enemyCount++;
            e.GetComponent<Enemy>().player = player;
            yield return wait;
        }
        
    }
}
