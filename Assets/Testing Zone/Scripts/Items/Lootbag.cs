using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lootbag : MonoBehaviour
{
    public List<GameObject> lootPrefabList = new List<GameObject>();
    private List<float> dropProbabilities = new List<float>() { 0.4f, 0.03f, 0.03f, 0.03f, 0.03f };

    GameObject GetDroppedItem()
    {
        if (lootPrefabList.Count > 0)
        {
            float totalProbability = 0f;

            // Calcular la suma total de probabilidades
            foreach (float probability in dropProbabilities)
            {
                totalProbability += probability;
            }

            // Generar un número aleatorio entre 0 y la suma total de probabilidades
            float randomValue = Random.Range(0f, totalProbability);

            // Determinar el objeto basado en las probabilidades acumuladas
            for (int i = 0; i < lootPrefabList.Count; i++)
            {
                if (randomValue <= dropProbabilities[i])
                {
                    return lootPrefabList[i];
                }

                randomValue -= dropProbabilities[i];
            }

            // Si por alguna razón no se encuentra ningún objeto para instanciar, devuelve null
            return null;
        }
        else
        {
            Debug.LogWarning("No loot items configured.");
            return null;
        }
    }

    public void InstantiateLoot(Vector3 spawnPosition)
    {
        GameObject droppedItemPrefab = GetDroppedItem();

        if (droppedItemPrefab != null)
        {
            Instantiate(droppedItemPrefab, spawnPosition, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("No loot prefab to instantiate.");
        }
    }

}
