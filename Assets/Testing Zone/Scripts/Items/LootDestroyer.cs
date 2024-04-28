using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootDestroyer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Verificar si el objeto colisionado tiene la etiqueta "loot" y es un objeto "is trigger"
        if (other.CompareTag("Loot"))
        {
            Destroy(other.gameObject); // Destruir el objeto colisionado
        }
    }
}
