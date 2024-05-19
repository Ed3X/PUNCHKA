using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelScript : MonoBehaviour
{
    public int currentLevel = 1;
    private int totalEnemies = 5;

    public int EnemiesToBeSpawned()
    {
        return totalEnemies + (currentLevel * 2);
    }
}
