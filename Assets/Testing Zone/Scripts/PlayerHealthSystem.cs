using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.InputSystem;
using Cinemachine;

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

    private Material myMaterial;
    private float flashTimer;
    private AudioSource audioSource;
    private PlayerInput playerInput;
    private PlayerCombat playerCombat;
    private WeaponSwitcher weaponSwitcher;
    private CharacterController charController;
    private InputController inputController;

    public CinemachineVirtualCamera camara;

    private Animator anim;

    private void Start()
    {
        ChangeOrthoSize(0.5f, 0.05f);
        currentHealth = maxHealth;
        myMaterial = GetComponent<Renderer>().material;
        originalColor = myMaterial.color;

        playerInput = GetComponent<PlayerInput>();
        inputController = GetComponent<InputController>();
        playerCombat = GetComponent<PlayerCombat>();
        weaponSwitcher = GetComponent<WeaponSwitcher>();
        charController = GetComponent<CharacterController>();

        anim = GetComponent<Animator>();

        healthBar.SetMaxHealth(maxHealth);

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        deathImage.SetActive(false);
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
        charController.enabled = false;

        ChangeOrthoSize(0.3f, 0.5f);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("Testing Zone");
        }
    }

    void ChangeOrthoSize(float targetSize, float duration)
    {
        if (camara != null)
        {
            float currentSize = camara.m_Lens.OrthographicSize;
            StartCoroutine(SmoothOrthoSizeChange(currentSize, targetSize, duration));
        }
    }

    IEnumerator SmoothOrthoSizeChange(float startSize, float targetSize, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float newSize = Mathf.Lerp(startSize, targetSize, elapsedTime / duration);
            camara.m_Lens.OrthographicSize = newSize;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        camara.m_Lens.OrthographicSize = targetSize;
    }

    public void HealToMax()
    {
        currentHealth = maxHealth;
        healthBar.SetHealth(currentHealth);
    }
}
