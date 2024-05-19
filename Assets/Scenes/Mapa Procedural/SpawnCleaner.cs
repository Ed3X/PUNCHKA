using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCleaner : MonoBehaviour
{
    public SpawnLocalizer spawnLocalizer;

    [SerializeField]
    private GameObject spawnEnemyPrefab;
    [SerializeField]
    private GameObject spawnPlayerPrefab;
    [SerializeField]
    private GameObject spawnShopPrefab;

    public void CleanSpawns()
    {
        GameObject[] allOjects = GameObject.FindObjectsOfType<GameObject>();

        foreach(GameObject obj in allOjects)
        {
            if(obj.tag == "Spawner" && obj.name.Contains(("Clone")))
            {
                Destroy(obj);
            }
        }
    }
}


