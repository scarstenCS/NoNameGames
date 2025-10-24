using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
    public struct Wave
    {
        public int numRunner;
        public int numTurret;
        public bool boss;
        public bool upgradeInWave;
    }
public class WaveManager : MonoBehaviour
{
    static private WaveManager _instance;
    static public WaveManager Instance;
    [SerializeField] private DialogueTrigger dialogueTrigger; 
    public List<Wave> waves;
    private int _waveCount;
    public GameObject enemyPrefab;
    public GameObject turretPrefab;
    public GameObject tBulletPrefab;
    public Camera mainCamera;
    public GameObject player;
    private static WaitForSeconds wait;
    public static float spawnrate;
    public static int maxEnemies;
    public static int enemiesLeft;
    public static int runnerCount;
    public static int turretCount;
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
        _waveCount = 0;
        mainCamera = Camera.main;
        spawnrate = 1.5f;
        maxEnemies = waves[_waveCount].numRunner + waves[_waveCount].numTurret;
        enemiesLeft = maxEnemies;
        runnerCount = 0;
        turretCount = 0;
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
        float rand = Random.Range(0, 2);
        if ((rand == 0 && runnerCount < waves[_waveCount].numRunner) || turretCount == waves[_waveCount].numTurret)
        {
            SpawnEnemy();
        }
        else
        {
            SpawnTurret();
        }
    }
    public void SpawnEnemy()
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
        runnerCount++;
    }
    public void SpawnTurret()
    {
        GameObject e = Instantiate(turretPrefab, new Vector3(Random.Range(GameManager.minX, GameManager.maxX), Random.Range(GameManager.minY, GameManager.maxY)), Quaternion.identity);
        e.GetComponent<TurretEnemy>().player = player;
        e.GetComponent<TurretEnemy>().bulletPrefab = tBulletPrefab;
        turretCount++;
    }
    public IEnumerator Phase()
    {
        while (_waveCount < waves.Count && !GameManager.isPaused)
        {
            while (runnerCount + turretCount < maxEnemies)
            {
                yield return new WaitForSeconds(spawnrate);
                Spawn();
            }
            // wait 5 secs once all enemies dead
            yield return new WaitUntil(() => enemiesLeft == 0);
            // if (_waveDoneText) _waveDoneText.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            if (_waveDoneText) _waveDoneText.SetActive(false);
            dialogueTrigger.OnWaveEnd(_waveCount);
            yield return new WaitUntil(dialogueTrigger.manager.isDialogueFinished);
            if (_waveDoneText) _waveDoneText.SetActive(false);

            _waveCount++;

            if (_waveCount < waves.Count)
            {
                if (waves[_waveCount-1].upgradeInWave)
                {
                    UpgradeManager.Instance.ShowUpgradeWindow();
                    yield return new WaitUntil(UpgradeManager.isWindowClosed);
                }
                // reset vars for new wave
                maxEnemies = waves[_waveCount].numRunner + waves[_waveCount].numTurret;
                runnerCount = 0;
                turretCount = 0;
                enemiesLeft = maxEnemies;
                Player p = player.GetComponent<Player>();
                p.Heal(p.MaxHealth);
            }
        }
        // game done
        GameManager.Instance.GoToMainMenu();
    }
}
