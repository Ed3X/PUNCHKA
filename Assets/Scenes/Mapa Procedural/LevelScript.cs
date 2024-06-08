using SVS;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelScript : MonoBehaviour
{
    public int currentLevel = 1;
    private int totalEnemies = 5;

    public int currentEnemies = 1;

    public Visualizer visualizer;
    public SpawnLocalizer spawnLocalizer;

    public TMP_Text CurrentLevelTextbox;
    public TMP_Text EnemiesToKillTextbox;

    private void Start()
    {
        LevelPassed();
    }

    public int EnemiesToBeSpawned()
    {
        return totalEnemies + (currentLevel * 2)+1;
    }

    public void LevelPassed()
    {
        currentLevel++;
        currentEnemies = 1;
        CurrentLevel();
        EnemiesToKill();
        visualizer.CreateTown();
        DestroyAllEnemies();
        spawnLocalizer.LocalizeSpawnablePositions(EnemiesToBeSpawned());

        CurrentLevelTextbox.text = $"Level: {currentLevel}";
        EnemiesToKillTextbox.text = $"{EnemiesToBeSpawned()-1}";
    }

    private void DestroyAllEnemies()
    {
        string targetName = "ENEMY Variant (1)(Clone)";

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
            currentEnemies++;
            if(currentEnemies == EnemiesToBeSpawned() - 1)
            {
                LevelPassed();
            }
            Debug.Log("Current Enemies: " + currentEnemies);        
    }
}
