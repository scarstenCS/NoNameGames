using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Camera mainCamera;
    public GameObject player;
    private int maxEnemies = 5;
    private static int enemyCount = 0;
    private WaitForSeconds wait;
    public void Start()
    {
        mainCamera = Camera.main;
        wait = new WaitForSeconds(2f);
        StartCoroutine(Spawn());
    }

    public void Update()
    {
    
    }

    public IEnumerator Spawn()
    {
        while (enemyCount <= maxEnemies && !GameManager.isPaused)
        {
            if (enemyCount < maxEnemies)
            {
                Vector3[] positions = { new Vector3(Random.Range(0, 2), Random.Range(0f, 1f)), new Vector3(Random.Range(0f, 1f), Random.Range(0, 2)) };
                GameObject e = Instantiate(enemyPrefab, mainCamera.ViewportToWorldPoint(positions[Random.Range(0, 2)]), Quaternion.identity);
                enemyCount++;
                e.GetComponent<Enemy>().player = player;
            }
            yield return wait;
        }
    }
}
