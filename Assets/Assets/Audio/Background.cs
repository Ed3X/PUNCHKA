using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip[] audioClips; // Lista de clips de audio a reproducir
    private AudioSource audioSource; // Referencia al componente AudioSource
    private AudioClip lastPlayedClip; // Referencia al último clip reproducido
    private bool isMusicPlaying = false; // Estado de la música

    void Start()
    {
        // Obtener el componente AudioSource del GameObject actual o añadir uno nuevo si no existe
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Inicialmente la música está desactivada
        StopMusic();
    }

    void Update()
    {
        // Verificar si se presiona la tecla 'C' para activar/desactivar la música
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (isMusicPlaying)
            {
                StopMusic(); // Si la música está reproduciéndose, detenerla
            }
            else
            {
                PlayRandomAudioClip(); // Si la música no está reproduciéndose, empezar a reproducirla
            }
        }
    }

    void PlayRandomAudioClip()
    {
        // Comprobar si hay clips de audio en la lista
        if (audioClips.Length == 0)
        {
            Debug.LogWarning("No hay clips de audio para reproducir.");
            return;
        }

        // Seleccionar un clip aleatorio que no sea el último reproducido
        AudioClip randomClip = GetRandomClipDifferentFromLast();

        // Reproducir el clip de audio seleccionado
        audioSource.clip = randomClip;
        audioSource.Play();

        // Programar la reproducción del siguiente clip
        Invoke("PlayRandomAudioClip", randomClip.length);

        isMusicPlaying = true; // Marcar que la música está reproduciéndose
    }

    void StopMusic()
    {
        audioSource.Stop(); // Detener la reproducción actual
        CancelInvoke("PlayRandomAudioClip"); // Cancelar la reproducción programada

        isMusicPlaying = false; // Marcar que la música está detenida
    }

    AudioClip GetRandomClipDifferentFromLast()
    {
        AudioClip randomClip;

        // Seleccionar un clip aleatorio que no sea el último reproducido
        do
        {
            randomClip = audioClips[Random.Range(0, audioClips.Length)];
        }
        while (randomClip == lastPlayedClip && audioClips.Length > 1); // Asegurarse de que el clip no sea el último reproducido

        // Actualizar la referencia al último clip reproducido
        lastPlayedClip = randomClip;

        return randomClip;
    }
}
