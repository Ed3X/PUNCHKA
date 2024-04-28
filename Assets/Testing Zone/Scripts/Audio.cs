using UnityEngine;

public class SoundOnTrigger : MonoBehaviour
{
    public AudioClip soundEffect;
    public AudioClip screamer;

    private void OnTriggerEnter(Collider other)
    {
        // Verifica si el objeto en colisión tiene el tag o el nombre adecuado
        if (other.gameObject.CompareTag("ObjetoAudio"))
        {
            // Reproduce el sonido cuando hay una colisión
            AudioSource.PlayClipAtPoint(soundEffect, transform.position);
        }
        // Verifica si el objeto en colisión tiene el tag o el nombre adecuado
        if (other.gameObject.CompareTag("Screamer"))
        {
            // Reproduce el sonido cuando hay una colisión
            AudioSource.PlayClipAtPoint(screamer, transform.position);
        }
    }
}
