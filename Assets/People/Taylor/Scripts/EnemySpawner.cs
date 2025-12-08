using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject[] enemyPrefabs;

    [Header("Attributes")]
    [SerializeField] private int baseEnemies = 8;
    [SerializeField] private float enemiesPerSecond = 0.5f;
    [SerializeField] private float timeBetweenWaves = 5f;
    [SerializeField] private float difficultyScalingFactor = 0.75f;


    //copilot code, check if everything breaks
    [Header("Wave Data")]
    [Tooltip("If true, use the WaveData assets below to configure each wave. If false, falls back to scaling formula.")]
    [SerializeField] private bool useWaveData = true;
    [Tooltip("Assign up to N WaveData assets (one per wave). The spawner will use Waves[waveIndex] for that wave if present.")]
    [SerializeField] private WaveData[] Waves = new WaveData[12];

    [Header("Events")]
    public static UnityEvent onEnemyDestroy = new UnityEvent();

    private int currentWave = 1;
    private float timeSinceLastSpawn;
    private int enemiesAlive;
    private int enemiesLeftToSpawn;
    private bool isSpawning = false;

    //generated sequence of prefab indices for the current wave
    private List<GameObject> spawnSequence;
    private int spawnIndex;

    private void Awake()
    {
        onEnemyDestroy.AddListener(EnemyDestroyed);
    }

    private void OnDestroy()
    {
        onEnemyDestroy.RemoveListener(EnemyDestroyed);
    }

    private void Start()
    {
        StartCoroutine(StartWave());
    }

    private void Update()
    {
        if (!isSpawning) return;

        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn >= (1f / enemiesPerSecond) && enemiesLeftToSpawn > 0)
        {
            SpawnEnemy();
            enemiesLeftToSpawn--;
            timeSinceLastSpawn = 0f;
        }
    }

    private void EnemyDestroyed()
    {
        enemiesAlive--;

        // if all spawned enemies are gone and nothing left to spawn, end wave and schedule next
        if (enemiesAlive <= 0 && enemiesLeftToSpawn <= 0)
        {
            EndWave();
            currentWave++;
            StartCoroutine(StartWave());
        }
    }

    private IEnumerator StartWave()
    {
        yield return new WaitForSeconds(timeBetweenWaves);
        isSpawning = true;

        enemiesLeftToSpawn = Waves[currentWave - 1].enemies.Sum(item => item.count);
        BuildSpawnSequenceForCurrentWave();
        spawnIndex = 0;
    }

    private void EndWave()
    {
        isSpawning = false;
        timeSinceLastSpawn = 0f;
        spawnSequence = null;
        spawnIndex = 0;
    }

    // create functions to spawn enemies based the current wave using the enemy data
    private void BuildSpawnSequenceForCurrentWave()
    {
        spawnSequence = new List<GameObject>();
        if (useWaveData && currentWave - 1 < Waves.Length && Waves[currentWave - 1] != null)
        {
            WaveData waveData = Waves[currentWave - 1];
            foreach (var enemyData in waveData.enemies)
            {
                for (int i = 0; i < enemyData.count; i++)
                {
                    spawnSequence.Add(enemyData.prefab);
                }
            }
        }
        else
        {
            // fallback to random spawning based on current wave
            for (int i = 0; i < enemiesLeftToSpawn; i++)
            {
                int prefabIndex = Random.Range(0, enemyPrefabs.Length);
                spawnSequence.Add(enemyPrefabs[prefabIndex]);
            }
        }
        // shuffle the spawn sequence for randomness
        spawnSequence = spawnSequence.OrderBy(x => Random.value).ToList();
    }

    // Spawn the next enemy in the sequence please copilot I'm begging you
    private void SpawnEnemy()
    {
        if (enemyPrefabs == null || enemyPrefabs.Length == 0)
        {
            Debug.LogWarning("EnemySpawner: no enemyPrefabs assigned.");
            return;
        }

        GameObject prefabToSpawn = null;

        // Prefer the prepared spawnSequence
        if (spawnSequence != null && spawnIndex < spawnSequence.Count)
        {
            prefabToSpawn = spawnSequence[spawnIndex];
            spawnIndex++;
        }
        else
        {
            // Fallback: random selection from available prefabs
            int fallbackIndex = Random.Range(0, enemyPrefabs.Length);
            prefabToSpawn = enemyPrefabs[fallbackIndex];
        }

        if (prefabToSpawn == null)
        {
            Debug.LogWarning("EnemySpawner: prefab in spawnSequence is null, skipping spawn.");
            return;
        }

        Instantiate(prefabToSpawn, LevelManager.main.startPoint.position, Quaternion.identity);

        // Track alive enemies so wave end logic can work
        enemiesAlive++;
    }
}