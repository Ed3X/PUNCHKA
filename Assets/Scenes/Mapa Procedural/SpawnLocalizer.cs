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

    public void LocalizeSpawnablePositions()
    {
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
                    }
                    else if (!shopSpawnPlaced)
                    {
                        GameObject shopSpawnPrefab = Instantiate(spawnShopPrefab, obj.transform.position, obj.transform.rotation);
                        shopSpawnPlaced = true;
                    }
                    else
                    {
                        GameObject enemySpawnPrefab = Instantiate(spawnEnemyPrefab, obj.transform.position, obj.transform.rotation);
                    }
                }
            }
        }
        spawnShopPrefab.SetActive(false);
        spawnPlayerPrefab.SetActive(false);
        spawnEnemyPrefab.SetActive(false);
    }
}
