using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    static private WaveManager _instance;
    static public WaveManager Instance;
    [SerializeField] private DialogueTrigger dialogueTrigger; // assign in Inspector
    static private ArrayList waveTable = new ArrayList { 10, 15, 20, 25, 30, 1 };
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
        
        List<Vector3> positions = new List<Vector3>();
        if (player.transform.position.x > GameManager.minX + 1 && player.transform.position.x < GameManager.maxX - 1)
        {
            positions.Add(new Vector3(GameManager.minX - 1, Random.Range((float)GameManager.minY, (float)GameManager.maxY)));
            positions.Add(new Vector3(GameManager.maxX + 1, Random.Range((float)GameManager.minY, (float)GameManager.maxY)));
        }
        if (player.transform.position.y > GameManager.minY + 1 && player.transform.position.y < GameManager.maxY - 1)
        {
            positions.Add(new Vector3(Random.Range((float)GameManager.minX, (float)GameManager.maxX), GameManager.minY - 1));
            positions.Add(new Vector3(Random.Range((float)GameManager.minX, (float)GameManager.maxX), GameManager.maxY + 1));
        }
        // Vector3[] positions = { new Vector3(Random.Range(0, 2), Random.Range(0f, 1f)), new Vector3(Random.Range(0f, 1f), Random.Range(0, 2)) };
        // GameObject e = Instantiate(enemyPrefab, mainCamera.ViewportToWorldPoint(positions[Random.Range(0, positions.Count)]), Quaternion.identity);
        // ...after building 'positions' and BEFORE the Instantiate line:
        if (positions.Count == 0)
        {
            const float margin = .5f;
            const float cornerW = 1f;
            const float cornerH = 1f;

            float minX = GameManager.minX, maxX = GameManager.maxX;
            float minY = GameManager.minY, maxY = GameManager.maxY;
            float px = player.transform.position.x, py = player.transform.position.y;

            bool nearLeft   = px <= minX + margin;
            bool nearRight  = px >= maxX - margin;
            bool nearBottom = py <= minY + margin;
            bool nearTop    = py >= maxY - margin;

            Vector3 fallback;

            if (nearRight && nearBottom)fallback = new Vector3(Random.Range(minX - cornerW, minX - margin),
                Random.Range(maxY + margin, maxY + cornerH), 0f);
            else if (nearLeft && nearBottom) fallback = new Vector3(Random.Range(maxX + margin, maxX + cornerW),
                Random.Range(maxY + margin, maxY + cornerH), 0f);
            else if (nearRight && nearTop) fallback = new Vector3(Random.Range(minX - cornerW, minX - margin),
                Random.Range(minY - cornerH, minY - margin), 0f);
            else fallback = new Vector3(Random.Range(maxX + margin, maxX + cornerW),
                Random.Range(minY - cornerH, minY - margin), 0f);

            positions.Add(fallback);
        }
        GameObject e = Instantiate(enemyPrefab, positions[Random.Range(0, positions.Count)], Quaternion.identity);
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
            yield return new WaitForSeconds(0.5f);
            if (_waveDoneText) _waveDoneText.SetActive(false);
            dialogueTrigger.OnWaveEnd(waveCount);
            yield return new WaitUntil(dialogueTrigger.manager.isDialogueFinished);
            if (_waveDoneText) _waveDoneText.SetActive(false);

            UpgradeManager.Instance.ShowUpgradeWindow();

            yield return new WaitUntil(UpgradeManager.isWindowClosed);
            // reset vars for new wave
            waveCount++;
            maxEnemies = (int)waveTable[waveCount];
            enemyCount = 0;
            enemiesLeft = maxEnemies;
            Player p = player.GetComponent<Player>();
            p.Heal(p.MaxHealth);
        }
    }
}
