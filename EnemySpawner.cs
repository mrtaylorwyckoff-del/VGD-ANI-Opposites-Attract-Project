csharp Assets\People\Taylor\Scripts\EnemySpawner.cs
using System.Collections;
using System.Collections.Generic;
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

    // generated sequence of prefab indices for the current wave
    private List<int> spawnSequence;
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
            enemiesAlive++;
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

        enemiesLeftToSpawn = EnemiesPerWave();
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

    private void SpawnEnemy()
    {
        if (enemyPrefabs == null || enemyPrefabs.Length == 0)
        {
            Debug.LogWarning("EnemySpawner: no prefabs assigned.");
            return;
        }

        GameObject prefabToSpawn = enemyPrefabs[0];

        if (spawnSequence != null && spawnIndex < spawnSequence.Count)
        {
            int prefabIndex = spawnSequence[spawnIndex];
            prefabIndex = Mathf.Clamp(prefabIndex, 0, enemyPrefabs.Length - 1);
            prefabToSpawn = enemyPrefabs[prefabIndex];
            spawnIndex++;
        }
        else
        {
            // fallback: round-robin if no sequence
            int rr = (spawnIndex + currentWave) % enemyPrefabs.Length;
            prefabToSpawn = enemyPrefabs[ rr ];
            spawnIndex++;
        }

        Instantiate(prefabToSpawn, LevelManager.main.startPoint.position, Quaternion.identity);
    }

    //COPILOT CODE PROCEED WITH CAUTION

    private int EnemiesPerWave()
    {
        if (useWaveData)
        {
            int waveIndex = currentWave - 1;
            if (Waves != null && waveIndex >= 0 && waveIndex < Waves.Length && Waves[waveIndex] != null)
            {
                WaveData wd = Waves[waveIndex];

                // If explicit prefabCounts are present and sum > 0, use that sum
                if (wd.prefabCounts != null && wd.prefabCounts.Count > 0)
                {
                    int sum = 0;
                    for (int i = 0; i < wd.prefabCounts.Count; i++) sum += Mathf.Max(0, wd.prefabCounts[i]);
                    if (sum > 0) return sum;
                }

                // otherwise use totalEnemyCount
                return Mathf.Max(1, wd.totalEnemyCount);
            }
        }

        // fallback to original scaling behavior
        return Mathf.RoundToInt(baseEnemies * Mathf.Pow(currentWave, difficultyScalingFactor));
    }

    private void BuildSpawnSequenceForCurrentWave()
    {
        spawnSequence = new List<int>();

        if (enemyPrefabs == null || enemyPrefabs.Length == 0) return;

        if (useWaveData)
        {
            int waveIndex = currentWave - 1;
            if (Waves != null && waveIndex >= 0 && waveIndex < Waves.Length && Waves[waveIndex] != null)
            {
                WaveData cfg = Waves[waveIndex];

                // If explicit prefabCounts provided and matches prefab count, honor them (block-order)
                if (cfg.prefabCounts != null && cfg.prefabCounts.Count == enemyPrefabs.Length)
                {
                    int sum = 0;
                    for (int i = 0; i < cfg.prefabCounts.Count; i++) sum += Mathf.Max(0, cfg.prefabCounts[i]);
                    if (sum > 0)
                    {
                        for (int i = 0; i < cfg.prefabCounts.Count; i++)
                        {
                            int count = Mathf.Max(0, cfg.prefabCounts[i]);
                            for (int c = 0; c < count; c++)
                                spawnSequence.Add(i);
                        }
                        return;
                    }
                }

                // otherwise distribute totalEnemyCount automatically
                int total = Mathf.Max(1, cfg.totalEnemyCount);
                DistributeAcrossPrefabs(total, spawnSequence);
                return;
            }
        }

        // default behavior when not using wave data: use scaled count and distribute
        int defaultTotal = Mathf.RoundToInt(baseEnemies * Mathf.Pow(currentWave, difficultyScalingFactor));
        DistributeAcrossPrefabs(defaultTotal, spawnSequence);
    }

    // Allocate total enemies across a number of distinct prefabs (grows with wave)
    private void DistributeAcrossPrefabs(int total, List<int> outSequence)
    {
        int distinctPrefabs = Mathf.Clamp(1 + (currentWave - 1) / 2, 1, enemyPrefabs.Length);
        int baseCount = total / distinctPrefabs;
        int remainder = total % distinctPrefabs;

        for (int prefabIndex = 0; prefabIndex < distinctPrefabs; prefabIndex++)
        {
            int count = baseCount + (prefabIndex < remainder ? 1 : 0);
            for (int c = 0; c < count; c++)
                outSequence.Add(prefabIndex);
        }
    }
}