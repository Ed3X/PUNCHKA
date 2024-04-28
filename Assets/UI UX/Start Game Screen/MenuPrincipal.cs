using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // Lista de nombres de las escenas
    public string[] sceneNames;

    // Método que se llama cuando se hace click
    public void LoadSceneOnClick(int sceneIndex)
    {
        // Verificar si el índice está dentro del rango válido
        if (sceneIndex >= 0 && sceneIndex < sceneNames.Length)
        {
            // Obtener el nombre de la escena usando el índice
            string sceneToLoad = sceneNames[sceneIndex];

            // Cargar la escena por nombre
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogError("Índice de escena fuera de rango.");
        }
    }
}
