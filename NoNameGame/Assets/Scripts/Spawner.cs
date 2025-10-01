using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    static private Spawner _instance;
    static public Spawner Instance;
    public GameObject enemyPrefab;
    public Camera mainCamera;
    public GameObject player;
    private static WaitForSeconds wait;
    void Awake()
    {
        _instance = this;
        Instance = this;
    }
    public void Start()
    {
        mainCamera = Camera.main;
        wait = new WaitForSeconds(WaveManager.spawnrate);
    }

    public void Update()
    {
    
    }

    public IEnumerator Spawn()
    {
        while (WaveManager.enemyCount < WaveManager.maxEnemies && !GameManager.isPaused)
        {
            Vector3[] positions = { new Vector3(Random.Range(0, 2), Random.Range(0f, 1f)), new Vector3(Random.Range(0f, 1f), Random.Range(0, 2)) };
            GameObject e = Instantiate(enemyPrefab, mainCamera.ViewportToWorldPoint(positions[Random.Range(0, 2)]), Quaternion.identity);
            e.GetComponent<Enemy>().player = player;
            WaveManager.enemyCount++;
            yield return wait;
        }
        yield return wait;
    }
}
