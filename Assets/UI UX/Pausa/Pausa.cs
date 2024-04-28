using UnityEngine;


public class rPausa : MonoBehaviour
{
    public GameObject pausaPanel;


    private bool juegoEnPausa = false;


    void Start()
    {
        // Al iniciar el juego, ocultamos el panel de pausa
        pausaPanel.SetActive(false);
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
        juegoEnPausa = true;
        pausaPanel.SetActive(true);
        Time.timeScale = 0f; // Pausar el tiempo del juego
    }


    void ReanudarJuego()
    {
        juegoEnPausa = false;
        pausaPanel.SetActive(false);
        Time.timeScale = 1f; // Reanudar el tiempo del juego
    }
}


