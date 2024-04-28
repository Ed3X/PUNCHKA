using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;
    public float spawnInterval = 3f;
    public int maxEnemies = 3;
    private int currentEnemies = 0;
    private bool isSpawning = false;

    void Start()
    {
        StartSpawning();
    }

    void StartSpawning()
    {
        if (!isSpawning)
        {
            isSpawning = true;
            InvokeRepeating("TrySpawnEnemy", 0f, spawnInterval);
        }
    }

    void StopSpawning()
    {
        if (isSpawning)
        {
            isSpawning = false;
            CancelInvoke("TrySpawnEnemy");
        }
    }

    void TrySpawnEnemy()
    {
        if (currentEnemies < maxEnemies)
        {
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        EnemyHealthSystem healthSystem = enemy.GetComponent<EnemyHealthSystem>();
        if (healthSystem != null)
        {
            healthSystem.OnEnemyDestroyed += EnemyDestroyed;
        }

        currentEnemies++;
    }

    public void EnemyDestroyed()
    {
        currentEnemies--;

        // Si no se está spawneando y no hay enemigos activos, iniciar el spawneo.
        if (!isSpawning && currentEnemies <= 0)
        {
            StartSpawning();
        }
    }
}
