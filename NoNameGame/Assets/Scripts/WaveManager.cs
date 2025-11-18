using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

[System.Serializable]
    public struct Wave
    {
        public int numRunner;
        public int numTurret;
        public int numBoss;
        public bool upgradeInWave;
        public float spawnrate;
        public int hpIncrease;
        public int dmgIncrease;
        public float skillLowerBound;
        public float skillUpperBound;
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
    public GameObject bossPrefab;
    public Camera mainCamera;
    public GameObject player;
    private static WaitForSeconds wait;
    public static int maxEnemies;
    public static int enemiesLeft;
    public static int runnerCount;
    public static int turretCount;
    public static int bossCount;
    static public GameObject _waveDoneText;
    [SerializeField] GameObject waveDoneText;
    bool bossRunnersSpawned = false;
    public int numBossRunners = 3;
    public float timeBeforeBossRunnersSpawn = 5f;
    private BossEnemy boss;
    public int BaseEnemySpawn = 4;
    
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
        maxEnemies = waves[_waveCount].numRunner + waves[_waveCount].numTurret + waves[_waveCount].numBoss;
        enemiesLeft = maxEnemies;
        runnerCount = 0;
        turretCount = 0;
        bossCount = 0;
        StartCoroutine(Phase());
    }

    void OnEnable()
    {
        if (_waveDoneText) _waveDoneText.SetActive(false);
    }

    void Update()
    {
        if (bossCount == 1 && enemiesLeft == 1 && !bossRunnersSpawned)
        {
            StartCoroutine(BossSpawnRunners());     
            bossRunnersSpawned = true;
        }
    }
    public void Spawn()
    {
        float rand = Random.Range(0, 2);
        if (waves[_waveCount].numBoss > bossCount)
        {

            SpawnBoss();
        }
        
        else if ((rand == 0 && runnerCount < waves[_waveCount].numRunner) || turretCount == waves[_waveCount].numTurret)
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
        Enemy enemy = e.GetComponent<Enemy>();
        enemy.player = player;
        Wave currWave = waves[_waveCount];
        enemy.hp += currWave.hpIncrease;
        enemy.atk += currWave.dmgIncrease;
        enemy.followSkill = Random.Range(currWave.skillLowerBound, currWave.skillUpperBound);
        runnerCount++;
    }
    public void SpawnTurret()
    {
        GameObject e = Instantiate(turretPrefab, new Vector3(Random.Range(GameManager.minX, GameManager.maxX), Random.Range(GameManager.minY, GameManager.maxY)), Quaternion.identity);
        TurretEnemy t = e.GetComponent<TurretEnemy>();
        t.player = player;
        t.bulletPrefab = tBulletPrefab;
        Wave currWave = waves[_waveCount];
        t.hp += currWave.hpIncrease;
        t.atk += currWave.dmgIncrease;
        t.skill = Random.Range(currWave.skillLowerBound, currWave.skillUpperBound);
        turretCount++;
    }
    public void SpawnBoss()
    {
        UnityEngine.Debug.Log("Spawning Boss");
        Vector3 spawnLocation = bossSpawnLocation();

        GameObject b = Instantiate(bossPrefab, spawnLocation, Quaternion.identity);

        boss = b.GetComponent<BossEnemy>();
        boss.player = player;
        Wave currWave = waves[_waveCount];
        bossCount++;
    }
    Vector3 bossSpawnLocation()
    {
        var p = GameObject.FindGameObjectWithTag("Player");
        Transform playerPos = null;
        if (p != null) playerPos = p.transform;

        float minDistance = 4.5f;
        float maxDistance = 6.5f;

        Vector3 spawnLocation;
        do
        {
            float randX = Random.Range(GameManager.minX, GameManager.maxX);
            float randY = Random.Range(GameManager.minY, GameManager.maxY);
            spawnLocation = new Vector3(randX, randY, 0f);
        }
        while (Vector3.Distance(spawnLocation, playerPos.position) < minDistance || Vector3.Distance(spawnLocation, playerPos.position) > maxDistance);

        return spawnLocation;
    }
    public IEnumerator Phase()
    {
        while (_waveCount < waves.Count && !GameManager.isPaused)
        {
            while (runnerCount + turretCount + bossCount < maxEnemies)
            {
                yield return new WaitForSeconds(waves[_waveCount].spawnrate);
                int spawnAmount = Mathf.FloorToInt((BaseEnemySpawn+_waveCount)/3f);
                while(spawnAmount > (maxEnemies - (runnerCount + turretCount + bossCount)))
                {
                    spawnAmount--;
                }
                for(int i = 0; i < spawnAmount; i++)
                {
                    Spawn();
                }
                //Spawn();
            }
            // wait 5 secs once all enemies dead
            yield return new WaitUntil(() => enemiesLeft == 0);
            // if (_waveDoneText) _waveDoneText.SetActive(true);
            yield return new WaitForSeconds(0.5f);


            AudioManager.Instance.stopDrums();
            if (_waveCount >1)
            {
                if (_waveDoneText) _waveDoneText.SetActive(true);
                AudioManager.SfxWaveComplete();
                yield return new WaitForSeconds(2);
                if (_waveDoneText) _waveDoneText.SetActive(false);
            }

            // Show upgrades if applicable
            if (waves[_waveCount].upgradeInWave)
            {
                UpgradeManager.Instance.ShowUpgradeWindow();
                yield return new WaitUntil(UpgradeManager.isWindowClosed);
            }

            // show dialogue if applicable
            if (boss != null && boss.hp <= 0) yield break;
            dialogueTrigger.OnWaveEnd(_waveCount);
            yield return new WaitUntil(dialogueTrigger.manager.isDialogueFinished);

            _waveCount++;

            if (_waveCount < waves.Count)
            {
                // reset vars for new wave
                maxEnemies = waves[_waveCount].numRunner + waves[_waveCount].numTurret + waves[_waveCount].numBoss;
                runnerCount = 0;
                turretCount = 0;
                bossCount = 0;
                enemiesLeft = maxEnemies;
                Player p = player.GetComponent<Player>();
                p.Heal(p.MaxHealth);

                //!!
                AudioManager.Instance.startDrums();
            }
        }
        // game done
        GameManager.Instance.GoToMainMenu();
    }
    public IEnumerator BossSpawnRunners()
    {
        
        BossEnemy boss = FindObjectOfType<BossEnemy>();

        yield return new WaitForSeconds(timeBeforeBossRunnersSpawn);
        UnityEngine.Debug.Log("Boss hp: " + boss.hp);
        if(boss.hp <= 0) yield break;

        int difficulty = boss.difficulty;

        maxEnemies += numBossRunners*difficulty;
        enemiesLeft += numBossRunners*difficulty;
        Wave currWave = waves[_waveCount];
        currWave.numRunner += numBossRunners*difficulty;
        waves[_waveCount] = currWave;
        for (int i = 0; i < numBossRunners*difficulty; i++)
        {
            SpawnEnemy();
        }
        bossRunnersSpawned = false;
    }
}
