using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CatchLoot : MonoBehaviour
{
    public TextMeshProUGUI scoreText; // Referencia al TextMeshPro donde mostrar el puntaje
    private int score = 0; // Variable para almacenar el puntaje

    private void OnTriggerEnter(Collider other)
    {
        // Verificar si el objeto que ha colisionado tiene la etiqueta "loot"
        if (other.CompareTag("Loot"))
        {
            // Sumar 1 al puntaje
            score++;

            // Actualizar el texto del puntaje
            if (scoreText != null)
            {
                scoreText.text = "" + score.ToString();
            }

            // Hacer que el objeto "loot" desaparezca
            Destroy(other.gameObject);
        }
    }
}