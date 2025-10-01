using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    static private WaveManager _instance;
    static public WaveManager Instance;
    static private ArrayList waveTable = new ArrayList { 10, 10, 15, 15, 15, 1, 20, 20, 25, 25, 25, 1 };
    public static float spawnrate;
    private int waveCount = 0;
    public static int maxEnemies;
    public static int enemiesLeft;
    public static int enemyCount;
    // Start is called before the first frame update
    void Awake()
    {
        _instance = this;
    }
    void Start()
    {
        spawnrate = 2f;
        maxEnemies = (int)waveTable[waveCount];
        enemiesLeft = maxEnemies;
        enemyCount = 0;
        StartCoroutine(Spawner.Instance.Spawn());
    }

    // Update is called once per frame
    void Update()
    {
        if (enemiesLeft == 0)
        {
            Debug.Log("Wave Passed");
            waveCount++;
            maxEnemies = (int)waveTable[waveCount];
            enemyCount = 0;
            enemiesLeft = maxEnemies;
            StartCoroutine(Spawner.Instance.Spawn());
        }
    }
    
    public IEnumerator WavePassedTime()
    {
        yield return new WaitForSeconds(3f);
    }
}
