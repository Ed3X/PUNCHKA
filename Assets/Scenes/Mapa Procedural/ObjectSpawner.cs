using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject EnemyPrefab;
    [SerializeField]
    private GameObject PlayerPrefab;
    [SerializeField]
    private GameObject ShopPrefab;

    public int enemyspawns;

    public LevelScript levelScript;

    public void SpawnEntities()
    {
        enemyspawns = levelScript.EnemiesToBeSpawned();
        Debug.Log("Enemies to be spawned: " + enemyspawns);

        GameObject[] allObjects = GameObject.FindGameObjectsWithTag("Spawner");
        foreach (GameObject obj in allObjects)
        {
            Vector3 spawnPosition = obj.transform.position;
            Quaternion spawnRotation = Quaternion.identity;

            if (obj.name.Contains("EnemySpawnPrefab (Clone)"))
            {
                int enemyspawned = Random.Range(1,3);
                for(int i = 0; i < enemyspawned; i++)
                {
                    if(enemyspawns > 0)
                    {
                        GameObject enemyInstantiated = Instantiate(EnemyPrefab, spawnPosition, spawnRotation);
                        enemyspawns--;
                    }
                }                
            }
            else if (obj.name.Contains("PlayerSpawnPrefab (Clone)"))
            {
                GameObject playerInstantiated = Instantiate(PlayerPrefab, spawnPosition, spawnRotation);
            }
            else if (obj.name.Contains("ShopSpawnPrefab (Clone)"))
            {
                Debug.Log("Shop");
            }
        }
        if (enemyspawns > 0)
        {
            Debug.Log("Not enough spawnPoints");
        }
    }
}
