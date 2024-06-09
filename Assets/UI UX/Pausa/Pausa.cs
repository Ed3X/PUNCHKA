using UnityEngine;

public class rPausa : MonoBehaviour
{
    public GameObject pausaPanel;
    public AudioClip pausa;
    public AudioSource audiosource;
    public AudioSource otherAudioSource;

    private bool juegoEnPausa = false;
    private float volumenNormal; // Guarda el volumen normal del otro AudioSource

    void Start()
    {
        Time.timeScale = 1f; // Reanudar el tiempo del juego
        // Al iniciar el juego, ocultamos el panel de pausa
        pausaPanel.SetActive(false);

        // Asegurarse de que el AudioSource esté configurado correctamente
        audiosource = GetComponent<AudioSource>();
        if (audiosource == null)
        {
            audiosource = gameObject.AddComponent<AudioSource>();
        }

        // Configurar el AudioSource para ignorar la pausa del juego
        audiosource.ignoreListenerPause = true;

        // Guardar el volumen normal del otro AudioSource
        volumenNormal = otherAudioSource.volume;

        // Agregar el efecto de reverb al otro AudioSource
        otherAudioSource.spatialize = true;
        otherAudioSource.spatialBlend = 1.0f; // Asegura que el efecto de reverb se aplique completamente
        otherAudioSource.reverbZoneMix = 0.8f; // Ajusta el nivel de mezcla de reverb
    }

    void Update()
    {
        // Si se presiona la tecla "esc"
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Si el juego está en pausa, lo reanuda y oculta el panel de pausa
            if (juegoEnPausa)
            {
                ReanudarJuego();
            }
            // Si el juego no está en pausa, lo pausa y muestra el panel de pausa
            else
            {
                PausarJuego();
            }
        }
    }

    void PausarJuego()
    {
        // Reproducir el sonido de pausa
        audiosource.PlayOneShot(pausa);

        // Reducir el volumen del otro AudioSource
        otherAudioSource.volume = volumenNormal * 0.2f;

        juegoEnPausa = true;
        pausaPanel.SetActive(true);
        Time.timeScale = 0f; // Pausar el tiempo del juego
    }

    void ReanudarJuego()
    {
        // Reproducir el sonido de reanudación
        audiosource.PlayOneShot(pausa);

        // Restaurar el volumen normal del otro AudioSource
        otherAudioSource.volume = volumenNormal;

        juegoEnPausa = false;
        pausaPanel.SetActive(false);
        Time.timeScale = 1f; // Reanudar el tiempo del juego
    }
}
