using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    static private WaveManager _instance;
    static private ArrayList waveTable = new ArrayList {10, 10, 15, 15, 15, 1, 20, 20, 25, 25, 25, 1};
    public static float spawnrate;
    private int waveCount;
    public static int maxEnemies;
    public static int enemyCount;
    // Start is called before the first frame update
    void Awake()
    {
        _instance = this;
    }
    void Start()
    {
        spawnrate = 2f;
        maxEnemies = (int)waveTable[0];
        enemyCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
