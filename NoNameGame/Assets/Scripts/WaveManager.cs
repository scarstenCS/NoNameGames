using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    static private WaveManager _instance;
    static public WaveManager Instance;
    static private ArrayList waveTable = new ArrayList { 10, 10, 15, 15, 15, 1, 20, 20, 25, 25, 25, 1 };
    public GameObject enemyPrefab;
    public Camera mainCamera;
    public GameObject player;
    private static WaitForSeconds wait;
    public static float spawnrate;
    private int waveCount = 0;
    public static int maxEnemies;
    public static int enemiesLeft;
    public static int enemyCount;
    static public GameObject _waveDoneText;
    [SerializeField] GameObject waveDoneText;
    // Start is called before the first frame update
    void Awake()
    {
        _instance = this;
    }
    void Start()
    {
        _waveDoneText = waveDoneText;
        mainCamera = Camera.main;
        spawnrate = 2f;
        maxEnemies = (int)waveTable[waveCount];
        enemiesLeft = maxEnemies;
        enemyCount = 0;
        StartCoroutine(Phase());
    }

    void OnEnable()
    {
        if (_waveDoneText) _waveDoneText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Spawn()
    {
        Vector3[] positions = { new Vector3(Random.Range(0, 2), Random.Range(0f, 1f)), new Vector3(Random.Range(0f, 1f), Random.Range(0, 2)) };
        GameObject e = Instantiate(enemyPrefab, mainCamera.ViewportToWorldPoint(positions[Random.Range(0, 2)]), Quaternion.identity);
        e.GetComponent<Enemy>().player = player;
        enemyCount++;
    }

    public IEnumerator Phase()
    {
        while (waveCount < waveTable.Count && !GameManager.isPaused)
        {
            while (enemyCount < maxEnemies)
            {
                yield return new WaitForSeconds(spawnrate);
                Spawn();
            }
            // wait 5 secs once all enemies dead
            yield return new WaitUntil(() => enemiesLeft == 0);
            Debug.Log("Wave Done");
            if (_waveDoneText) _waveDoneText.SetActive(true);
            yield return new WaitForSeconds(5f);
            if (_waveDoneText) _waveDoneText.SetActive(false);
            waveCount++;
            maxEnemies = (int)waveTable[waveCount];
            enemyCount = 0;
            enemiesLeft = maxEnemies;
        }
    }
}
