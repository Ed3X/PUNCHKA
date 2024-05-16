using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public SpawnLocalizer spawnLocalizer;
    public void SpawnEnemies()
    {
        spawnLocalizer.LocalizeSpawnablePositions();
    }
}
