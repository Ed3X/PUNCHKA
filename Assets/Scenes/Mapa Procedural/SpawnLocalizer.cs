using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnLocalizer : MonoBehaviour
{
    [SerializeField]
    private string targetLayerName = "Calle";
    [SerializeField]
    public GameObject spawnEnemyPrefab;
    [SerializeField]
    public GameObject spawnPlayerPrefab;

    public LevelScript levelScript;

    private bool playerSpawnPlaced = false;    

    private void Start()
    {
        int spawnCount = levelScript.EnemiesToBeSpawned();
        LocalizeSpawnablePositions(spawnCount);
    }

    public void LocalizeSpawnablePositions(int spawnCount)
    {
        Debug.Log(spawnCount);
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();

        while (spawnCount > 0)
        {
            foreach (GameObject obj in allObjects)
            {
                if(spawnCount == 0)
                {
                    return;
                }
                if (obj.layer == LayerMask.NameToLayer(targetLayerName))
                {
                    if (Random.value < 0.2f)
                    {
                        if (!playerSpawnPlaced)
                        {
                            //GameObject playerSpawnPrefab = Instantiate(spawnPlayerPrefab, obj.transform.position, obj.transform.rotation);
                            //playerSpawnPlaced = true;
                            playerSpawnPlaced=true;
                            spawnPlayerPrefab.transform.position = obj.transform.position;
                            Debug.Log("Spawns the player at: " + obj.transform.position + " with rotation: " + obj.transform.rotation);
                            spawnCount--;
                        }
                        else
                        {
                            GameObject enemySpawnPrefab = Instantiate(spawnEnemyPrefab, obj.transform.position, obj.transform.rotation);                            
                            Debug.Log("Spawns the enemy" + obj.transform.position + " with rotation: " + obj.transform.rotation);
                            spawnCount--;
                        }
                    }
                }
            }
        }
        Debug.Log("Gets here");
        spawnEnemyPrefab.SetActive(false);
    }
}
