using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    static private WaveManager _instance;
    static private ArrayList<int> waveTable = [10, 10, 15, 15, 15, 1, 20, 20, 25, 25, 25, 1];
    public float spawnrate;
    private int waveCount;
    // Start is called before the first frame update
    void Awake()
    {
        _instance = this;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
