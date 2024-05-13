// El script actualizado con la referencia a la c�mara.
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.InputSystem;
using Cinemachine;
using UnityEngine.InputSystem.XInput;

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
    //public Image deathFadeImage;
    //public float fadeDuration = 3.0f;

    private Material myMaterial;
    private float flashTimer;
    private AudioSource audioSource;
    private PlayerInput playerInput;
    private PlayerCombat playerCombat;
    private WeaponSwitcher weaponSwitcher;
    private PlayerHealthSystem healthSystem;
    private CharacterController charController;
    private InputController inputController;

    public CinemachineVirtualCamera camara; // Ahora es un CinemachineVirtualCamera

    private Animator anim;

    private void Start()
    {
        ChangeOrthoSize(5f, 0.5f);
        currentHealth = maxHealth;
        myMaterial = GetComponent<Renderer>().material;
        originalColor = myMaterial.color;

        playerInput = GetComponent<PlayerInput>();
        inputController = GetComponent<InputController>();
        playerCombat = GetComponent<PlayerCombat>();
        weaponSwitcher = GetComponent<WeaponSwitcher>();
        healthSystem = GetComponent<PlayerHealthSystem>();
        charController = GetComponent<CharacterController>();

        anim = GetComponent<Animator>();

        healthBar.SetMaxHealth(maxHealth);

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        deathImage.SetActive(false);
        //deathFadeImage.gameObject.SetActive(false);
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
        anim.SetTrigger("Muerto");
        deathImage.SetActive(true);
        playerInput.enabled = false;
        inputController.enabled = false;
        playerCombat.enabled = false;
        weaponSwitcher.enabled = false;
        healthSystem.enabled = false;
        charController.enabled = false;

        ChangeOrthoSize(3f, 5f);

        healthSystem.enabled = false;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("Testing Zone");
        }
    }

    void ChangeOrthoSize(float targetSize, float duration)
    {
        if (camara != null) // Asegur�ndose de que la referencia a la c�mara no sea nula
        {
            // Obtener el tama�o actual de la lente ortogr�fica
            float currentSize = camara.m_Lens.OrthographicSize;

            // Iniciar una rutina para cambiar suavemente el tama�o
            StartCoroutine(SmoothOrthoSizeChange(currentSize, targetSize, duration));
        }
    }

    IEnumerator SmoothOrthoSizeChange(float startSize, float targetSize, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // Interpolar entre el tama�o inicial y el tama�o objetivo a lo largo del tiempo
            float newSize = Mathf.Lerp(startSize, targetSize, elapsedTime / duration);

            // Aplicar el nuevo tama�o a la lente ortogr�fica
            camara.m_Lens.OrthographicSize = newSize;

            // Incrementar el tiempo transcurrido
            elapsedTime += Time.deltaTime;

            // Esperar un frame
            yield return null;
        }

        // Asegurarse de que el tama�o objetivo sea alcanzado exactamente al finalizar la interpolaci�n
        camara.m_Lens.OrthographicSize = targetSize;
    }

}
