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

    public int TotalEnemyKills = 0;

    public Visualizer visualizer;
    public SpawnLocalizer spawnLocalizer;
    public CatchLoot catchLoot;

    public GameObject Player;

    public TMP_Text CurrentLevelTextbox;
    public TMP_Text EnemiesToKillTextbox;

    public TMP_Text levelsClearedTextbox;
    public TMP_Text TotalEnemiesKilledTextbox;
    public TMP_Text TimeAliveTextbox;
    public TMP_Text DientesRecolectadosTextbox;
    public TMP_Text PuntuacionFinalTextbox;

    float elapsedTime = 0f;

    private void Start()
    {
        LevelPassed();
    }
    public void Update()
    {
        elapsedTime = Time.time;

        if(Input.GetKeyDown(KeyCode.R))
        {
            LevelPassed();
        }
    }

    public int EnemiesToBeSpawned()
    {
        return totalEnemies + (currentLevel * 2)+1;
    }

    public void LevelPassed()
    {
        if(Player != null)
        {
            Vector3 position = new Vector3(0f, 0f, 0f);
            Player.transform.position = position;
        }

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
        TotalEnemyKills++;
        if(currentEnemies == EnemiesToBeSpawned() - 1)
        {
            LevelPassed();
        }
        Debug.Log("Current Enemies: " + currentEnemies);        
    }
    public void PlayerDies()
    {
        int levelsCleared = currentLevel-1;
        Debug.Log("" + levelsCleared);
        int levelsClearedPoints = levelsCleared * 3000;

        levelsClearedTextbox.text = $"{ levelsCleared}";

        int enemiesKilled = TotalEnemyKills;
        Debug.Log("" + enemiesKilled);
        int EnemiesKilledPoints = TotalEnemyKills * 250;

        TotalEnemiesKilledTextbox.text = $"{enemiesKilled}";

        int ToothCount = catchLoot.TotalDientes();
        Debug.Log("" + ToothCount);
        int ToothCountPoints = ToothCount * 75;
        DientesRecolectadosTextbox.text = $"{ToothCount}";

        int TimeAlive = Mathf.FloorToInt(elapsedTime);
        Debug.Log("" +  TimeAlive);
        int TimeAlivePoints = TimeAlive * 5;

        TimeAliveTextbox.text = $"{TimeAlive}";

        int computoTotalPuntos = levelsClearedPoints + EnemiesKilledPoints + TimeAlivePoints + ToothCountPoints;
        PuntuacionFinalTextbox.text = $"{computoTotalPuntos}";

        //CurrentLevelTextbox.text = $"Level: {currentLevel}";
        //EnemiesToKillTextbox.text = $"{EnemiesToBeSpawned() - 1}";

        //levelsClearedTextbox;
        //TotalEnemiesKilledTextbox;
        //TimeAliveTextbox;
        //DientesRecolectadosTextbox;
        //PuntuacionFinalTextbox;
    }
}
