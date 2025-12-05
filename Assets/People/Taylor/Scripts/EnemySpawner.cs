using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject[] enemyPrefabs;

    [Header("Attributes")]
    [SerializeField] private int baseEnemies = 8;
    [SerializeField] private float enemiesPerSecond = 0.5f;
    [SerializeField] private float timeBetweenWaves = 5f;
    [SerializeField] private float difficultyScalingFactor = 0.75f;

    [Header("Events")]
    public static UnityEvent onEnemyDestroy = new UnityEvent();

    [Header("Wave Configuration")]
    [SerializeField] private WaveData[] wavesConfigList;

    private int currentWave = 1;
    private float timeSinceLastSpawn;
    private int enemiesAlive;
    private int enemiesLeftToSpawn;
    private bool isSpawning = false;
    private int enemyIndex = 0; 

    private void Awake()
    {
        onEnemyDestroy.AddListener(EnemyDestroyed);
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
    }

    private void EndWave()
    {
        enemyIndex = 0;
        isSpawning = false;
        timeSinceLastSpawn = 0f;
    }

    private void SpawnEnemy()
    {
        //int index = Random.Range(0, enemyPrefabs.Length);
        Debug.Log(enemyIndex);
        GameObject prefabToSpawn = enemyPrefabs[enemyIndex];
        enemyIndex++;
        Instantiate(prefabToSpawn, LevelManager.main.startPoint.position, Quaternion.identity);
        if (enemyPrefabs.Length <= enemyIndex)
        {
            EndWave();
        }
    }

    private int EnemiesPerWave()
    {
        return Mathf.RoundToInt(baseEnemies * Mathf.Pow(currentWave, difficultyScalingFactor));
    }
}