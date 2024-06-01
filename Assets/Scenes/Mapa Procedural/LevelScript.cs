using SVS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelScript : MonoBehaviour
{
    public int currentLevel = 1;
    private int totalEnemies = 5;

    public Visualizer visualizer;

    public int EnemiesToBeSpawned()
    {
        return totalEnemies + (currentLevel * 2);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            LevelPassed();
        }
    }
    public void LevelPassed()
    {
            visualizer.CreateTown();
    }
}
