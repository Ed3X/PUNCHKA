using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public SpawnLocalizer spawnLocalizer;

    public SpawnCleaner spawnCleaner;

    [SerializeField]
    private GameObject spawnEnemyPrefab;
    [SerializeField]
    private GameObject spawnPlayerPrefab;
    [SerializeField]
    private GameObject spawnShopPrefab;

    public void SpawnEnemies()
    {
        spawnLocalizer.LocalizeSpawnablePositions();
    }
    public void DeleteOldSpawns()
    {
        spawnCleaner.CleanSpawns();

        spawnShopPrefab.SetActive(true);
        spawnPlayerPrefab.SetActive(true);
        spawnEnemyPrefab.SetActive(true);
    }
}
