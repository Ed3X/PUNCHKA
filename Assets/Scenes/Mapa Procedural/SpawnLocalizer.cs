using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnLocalizer : MonoBehaviour
{
    [SerializeField]
    private string targetLayerName = "Calle";
    [SerializeField]
    private GameObject spawnEnemyPrefab;
    [SerializeField]
    private GameObject spawnPlayerPrefab;
    [SerializeField]
    private GameObject spawnShopPrefab;

    private bool playerSpawnPlaced = false;
    private bool shopSpawnPlaced = false;

    private int spawnCount = 0;

    public LevelScript levelScript;

    public ObjectSpawner objectSpawner;

    private int enemyspawns;

    private void Start()
    {
        enemyspawns = levelScript.EnemiesToBeSpawned();
        Debug.Log(enemyspawns);
    }

    public void LocalizeSpawnablePositions()
    {
        spawnCount = 0;



        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if(obj.layer == LayerMask.NameToLayer(targetLayerName))
            {
                if(Random.value < 0.1f)
                {
                    if(!playerSpawnPlaced)
                    {
                        GameObject playerSpawnPrefab = Instantiate(spawnPlayerPrefab, obj.transform.position, obj.transform.rotation);
                        playerSpawnPlaced=true;
                        spawnCount++;
                    }
                    else if (!shopSpawnPlaced)
                    {
                        GameObject shopSpawnPrefab = Instantiate(spawnShopPrefab, obj.transform.position, obj.transform.rotation);
                        shopSpawnPlaced = true;
                        spawnCount++;
                    }
                    else 
                    {
                        GameObject enemySpawnPrefab = Instantiate(spawnEnemyPrefab, obj.transform.position, obj.transform.rotation);
                        spawnCount++;
                    }
                }
            }
        }

        Debug.Log(spawnCount);
        spawnShopPrefab.SetActive(false);
        spawnPlayerPrefab.SetActive(false);
        spawnEnemyPrefab.SetActive(false);

        objectSpawner.SpawnEntities();
    }
}
