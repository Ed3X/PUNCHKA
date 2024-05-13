using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.InputSystem;

public class PlayerHealthSystem : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public Color originalColor;
    public Color damageColor;
    public float damageFlashTime = 0.2f;
    public HealthBar healthBar;
    public bool isInvincible = false;
    public LayerMask WhatIsHealing;
    public Transform GroundChecker;
    public float groundSphereRadius = 0.5f;
    public DrugBar drugBar;
    public Transform Respawn;
    public Hurt_Layout hurt_Layout;
    public AudioClip deathSong;
    public GameObject deathImage;
    public Image deathFadeImage; // Nuevo objeto para el fade out
    public float fadeDuration = 3.0f; // Duración del fade out ajustada a 3 segundos

    private Material myMaterial;
    private float flashTimer;
    private AudioSource audioSource;
    private PlayerInput playerInput;

    private void Start()
    {
        currentHealth = maxHealth;
        myMaterial = GetComponent<Renderer>().material;
        originalColor = myMaterial.color;

        playerInput = GetComponent<PlayerInput>();

        healthBar.SetMaxHealth(maxHealth);

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        deathImage.SetActive(false);
        deathFadeImage.gameObject.SetActive(false); // Desactivar la imagen de fade out al inicio
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            PlayerTakesDamage(100);
        }
        if (flashTimer > 0)
        {
            flashTimer -= Time.deltaTime;
            myMaterial.color = damageColor;
        }
        else
        {
            myMaterial.color = originalColor;
        }

        if (IsHealing())
        {
            drugBar.drogaActual = 100f;
            currentHealth = maxHealth;
        }
    }

    private bool IsHealing()
    {
        return Physics.CheckSphere(
            GroundChecker.position, groundSphereRadius, WhatIsHealing);
    }

    public void PlayerTakesDamage(int damage)
    {
        if (!isInvincible)
        {
            currentHealth -= damage;
            hurt_Layout.ShowAndHideHurtUI();
        }

        if (currentHealth <= 0)
        {
            Die();
        }
        flashTimer = damageFlashTime;
        if (!isInvincible) // Check invulnerability after damage
        {
            StartCoroutine(InvulnerabilityTimer());
        }
        healthBar.SetHealth(currentHealth);
    }

    IEnumerator InvulnerabilityTimer()
    {
        isInvincible = true;
        yield return new WaitForSeconds(3.0f);
        isInvincible = false;
        Debug.Log("isInvincible");
    }

    private void Die()
    {
        deathImage.SetActive(true);
        playerInput.enabled = false;

        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        // Activar la imagen de fade out
        deathFadeImage.gameObject.SetActive(true);

        // Obtener el color inicial de la imagen
        Color color = deathFadeImage.color;

        // Iterar durante la duración del fade out
        float timer = 0;
        while (timer < fadeDuration)
        {
            // Calcular el nuevo valor de opacidad
            float alpha = Mathf.Lerp(0, 1, timer / fadeDuration);

            // Actualizar el color de la imagen con la nueva opacidad
            deathFadeImage.color = new Color(color.r, color.g, color.b, alpha);

            // Avanzar el temporizador
            timer += Time.deltaTime;

            // Esperar un frame
            yield return null;
        }

        // Una vez completado el fade out, reiniciar la escena o cargar una nueva escena
        SceneManager.LoadScene("Testing Zone");
    }
}
