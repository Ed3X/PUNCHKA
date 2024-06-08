using SVS;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelScript : MonoBehaviour
{
    public int currentLevel = 1;
    private int totalEnemies = 5;

    public int currentEnemies = 1;

    public Visualizer visualizer;
    public SpawnLocalizer spawnLocalizer;

    public int EnemiesToBeSpawned()
    {
        return totalEnemies + (currentLevel * 2)+1;
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
        currentLevel++;
        CurrentLevel();
        EnemiesToKill();
        visualizer.CreateTown();
        DestroyAllEnemies();
        spawnLocalizer.LocalizeSpawnablePositions(EnemiesToBeSpawned());
    }

    private void DestroyAllEnemies()
    {
        string targetName = "ENEMY Variant(Clone)";

        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
        List<GameObject> foundObjects = new List<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if(obj.name == targetName) 
            { 
                foundObjects.Add(obj);
            }
        }

        foreach (GameObject obj in foundObjects)
        {
            Destroy(obj);
        }
    }

    private void EnemiesToKill()
    {
        Debug.Log("Total enemies " + EnemiesToBeSpawned());
    }

    private void CurrentLevel()
    {
        Debug.Log("Level " + currentLevel);
    }
    public void EnemyKillCounter()
    {
        int TotalEnemies = EnemiesToBeSpawned();

        Debug.Log("Current Total Enemies to kill " + totalEnemies);
        
        if (currentEnemies < TotalEnemies)
        {            
            currentEnemies++;
            Debug.Log("Current Enemies: " + currentEnemies);
        }
        else
        {
            Debug.Log("Level passed");
            LevelPassed();
        }
    }
}
