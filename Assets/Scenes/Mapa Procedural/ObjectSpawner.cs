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

    public void SpawnEntities()
    {
        GameObject[] allObjects = GameObject.FindGameObjectsWithTag("Spawner");
        foreach (GameObject obj in allObjects)
        {
            Debug.Log(obj.name);
        }
    }
}
