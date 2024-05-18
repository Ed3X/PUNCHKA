using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public SpawnLocalizer spawnLocalizer;

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
        Debug.Log("Hola");

        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
        List <GameObject> clones = new List <GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.name.Contains("(Clone)"))
            {
                clones.Add(obj);
            }
        }

        foreach(GameObject obj in clones)
        {
            Destroy(obj);
        }
        spawnShopPrefab.SetActive(true);
        spawnPlayerPrefab.SetActive(true);
        spawnEnemyPrefab.SetActive(true);
    }
}
