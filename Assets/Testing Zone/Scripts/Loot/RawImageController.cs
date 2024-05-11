using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PowerUpImage
{
    public RawImage image;
    public float displayDuration = 5f; // Duración predeterminada de visualización
    [HideInInspector] public float displayTimer = 0f; // Temporizador para esta imagen
}

public class RawImageController : MonoBehaviour
{
    public PowerUpImage[] powerUpImages;

    private void Update()
    {
        // Actualizar los temporizadores y desactivar las imágenes que hayan alcanzado su tiempo
        foreach (PowerUpImage powerUpImage in powerUpImages)
        {
            if (powerUpImage.image.gameObject.activeSelf)
            {
                powerUpImage.displayTimer -= Time.deltaTime;
                if (powerUpImage.displayTimer <= 0f)
                {
                    powerUpImage.image.gameObject.SetActive(false);
                }
            }
        }

        // Detectar la entrada de teclado para mostrar imágenes
        for (int i = 0; i < powerUpImages.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha3 + i) && powerUpImages[i].image != null)
            {
                DisplayRawImage(powerUpImages[i]);
            }
        }
    }

    private void DisplayRawImage(PowerUpImage powerUpImage)
    {
        if (powerUpImage.image != null)
        {
            powerUpImage.image.gameObject.SetActive(true);
            powerUpImage.displayTimer = powerUpImage.displayDuration;
        }
    }
}
