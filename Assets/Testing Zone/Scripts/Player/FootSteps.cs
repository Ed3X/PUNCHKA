using UnityEngine;

public class FootSteps : MonoBehaviour
{
    [SerializeField] private AudioClip[] dirtFootstepClips; // Array de sonidos de pasos en tierra
    [SerializeField] private AudioClip[] asphaltFootstepClips; // Array de sonidos de pasos en asfalto
    [SerializeField] private AudioSource audioSource; // El componente AudioSource
    [SerializeField] private float stepInterval = 0.5f; // Intervalo de tiempo entre pasos
    [SerializeField] private LayerMask surfaceLayerMask; // Máscara de capa para las superficies

    private Vector3 lastPosition;
    private float stepTimer;

    private void Awake()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        // Almacenar la posición inicial
        lastPosition = transform.position;
    }

    private void Update()
    {
        // Calcular la distancia desde la última posición
        float distance = Vector3.Distance(transform.position, lastPosition);

        // Verificar si el objeto se ha movido lo suficiente para activar el sonido de los pasos
        if (distance > 0.01f)
        {
            // Incrementar el temporizador de pasos
            stepTimer += Time.deltaTime;

            // Si el temporizador alcanza el intervalo de pasos, reproducir el sonido
            if (stepTimer >= stepInterval)
            {
                Debug.Log("Step timer reached. Playing footstep sound.");
                PlayFootstepSound();
                stepTimer = 0f;
            }
        }
        else
        {
            Debug.Log("Object is not moving.");
            stepTimer = 0f; // Reiniciar el temporizador si el objeto no se está moviendo
        }

        // Actualizar la última posición
        lastPosition = transform.position;
    }

    private void PlayFootstepSound()
    {
        if (dirtFootstepClips.Length == 0 && asphaltFootstepClips.Length == 0)
        {
            Debug.LogWarning("Footstep clips array is empty.");
            return;
        }

        // Raycast hacia abajo para detectar la superficie
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1f, surfaceLayerMask))
        {
            Debug.Log("Raycast hit something. Checking surface type.");
            AudioClip[] footstepClips = null;

            // Detectar el tipo de superficie por tag
            switch (hit.collider.tag)
            {
                case "Tierra":
                    Debug.Log("Hit dirt surface.");
                    footstepClips = dirtFootstepClips;
                    break;
                case "Asfalto":
                    Debug.Log("Hit asphalt surface.");
                    footstepClips = asphaltFootstepClips;
                    break;
                default:
                    Debug.LogWarning("Unknown surface type. Using asphalt footstep clips by default.");
                    footstepClips = asphaltFootstepClips; // Valor predeterminado
                    break;
            }

            if (footstepClips.Length > 0)
            {
                // Selecciona un sonido aleatorio de los pasos
                AudioClip clip = footstepClips[Random.Range(0, footstepClips.Length)];
                audioSource.PlayOneShot(clip);
                Debug.Log("Playing footstep sound.");
            }
            else
            {
                Debug.LogWarning("No footstep clips found for the surface.");
            }
        }
        else
        {
            Debug.LogWarning("Raycast did not hit anything.");
        }
    }
}
