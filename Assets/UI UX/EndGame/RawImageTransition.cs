using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RawImageTransition : MonoBehaviour
{
    public RawImage rawImage;
    public float fadeDuration = 2.0f; // Duración de la transición de fade

    void Start()
    {
        // Supongamos que rawImage es tu RawImage que quieres hacer fade
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        // Establecer la opacidad inicial a 0
        rawImage.color = new Color(rawImage.color.r, rawImage.color.g, rawImage.color.b, 0);

        // Iterar durante la duración del fade in
        float timer = 0;
        while (timer < fadeDuration)
        {
            // Calcular el nuevo valor de opacidad
            float alpha = Mathf.Lerp(0, 1, timer / fadeDuration);

            // Actualizar la opacidad de la imagen
            rawImage.color = new Color(rawImage.color.r, rawImage.color.g, rawImage.color.b, alpha);

            // Avanzar el temporizador
            timer += Time.deltaTime;

            // Esperar un frame
            yield return null;
        }

        // Establecer la opacidad final a 1 (asegurarse de que sea exactamente 1)
        rawImage.color = new Color(rawImage.color.r, rawImage.color.g, rawImage.color.b, 1);
    }
}
