using UnityEngine;
using System;
using UnityEngine.AI;
using NUnit.Framework;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;

public class EnemyHealthSystem : MonoBehaviour
{
    public int maxHealth = 50;
    public int currentHealth;

    [SerializeField] private List<AudioClip> puño;
    private AudioSource audioSource;
    public bool IsDead { get; private set; }

    private Animator animator;
    public GameObject Sangre;

    public GameObject EnemyIndicator;
    public Sprite DeadEnemySprite;

    public CinemachineVirtualCamera camara;
    private float tambaleo1 = 0.25f;
    private float tambaleo2 = 90f;

    // Evento para notificar cuando un enemigo es destruido
    public event Action OnEnemyDestroyed;

    public LevelScript levelScript;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        GetComponent<EnemyNavMesh>().enabled = true;
        GetComponent<NavMeshAgent>().enabled = true;
        audioSource = GetComponent<AudioSource>();
        camara = GetComponent<CinemachineVirtualCamera>();

        BoxCollider boxCollider = GetComponent<BoxCollider>();
        if (boxCollider != null)
        {
            boxCollider.enabled = true;
        }

        CapsuleCollider capsuleCollider = GetComponent<CapsuleCollider>();
        if (capsuleCollider != null)
        {
            capsuleCollider.enabled = true;
        }

        // Obtener referencia al EnemySpawner usando FindObjectOfType
        EnemySpawner spawner = FindFirstObjectByType<EnemySpawner>();
        if (spawner != null)
        {
            OnEnemyDestroyed += spawner.EnemyDestroyed;
        }
    }

    IEnumerator Tambaleo()
    {
        var noise = camara.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        if (noise != null)
        {
            noise.m_FrequencyGain = tambaleo2;
            yield return new WaitForSeconds(0.5f);
            noise.m_FrequencyGain = tambaleo1;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        animator.SetTrigger("Stuned");
        Debug.Log("Infligiendo daño al enemigo.");

        // Generar un índice aleatorio para seleccionar un clip de audio
        int randomIndex = UnityEngine.Random.Range(0, puño.Count);
        audioSource.PlayOneShot(puño[randomIndex]);

        StartCoroutine(Tambaleo());

        if (currentHealth <= 0)
        {
            Debug.Log("Enemy died.");
            EnemyIndicator.GetComponent<SpriteRenderer>().sprite = DeadEnemySprite;
            levelScript.EnemyKillCounter();
            Dead();
        }
        else
        {
            // Verificar si el Prefab de Sangre está asignado
            if (Sangre != null)
            {
                // Calcular la rotación inversa del enemigo
                Quaternion reverseRotation = transform.rotation * Quaternion.Euler(0, 180, 0);

                // Instanciar una copia del Prefab de Sangre con la posición y rotación inversa del enemigo
                GameObject sangreInstance = Instantiate(Sangre, Sangre.transform.position, Sangre.transform.rotation);

                // Desvincular la sangre del padre actual
                sangreInstance.transform.parent = null;

                // Activar la sangre
                sangreInstance.SetActive(true);
            }
            else
            {
                Debug.LogWarning("Sangre Prefab not assigned!");
            }
        }
    }

    private void Dead()
    {
        IsDead = true;
        // Llamar al evento OnEnemyDestroyed
        OnEnemyDestroyed?.Invoke();
        // Activar la animación de muerte en el Animator
        if (animator != null)
        {
            animator.SetBool("Die", true);
            animator.SetBool("IsChasing", false);
            animator.SetBool("IsPlayerClose", false);
            animator.SetBool("IsPatrolling", false);
            animator.SetBool("IsAttacking", false);

            Lootbag lootbagComponent = GetComponent<Lootbag>();
            if (lootbagComponent != null)
            {
                lootbagComponent.InstantiateLoot(transform.position);
            }
            else
            {
                Debug.LogError("No Lootbag component found on this GameObject.");
            }

            BoxCollider boxCollider = GetComponent<BoxCollider>();
            if (boxCollider != null)
            {
                boxCollider.enabled = false;
            }

            CapsuleCollider capsuleCollider = GetComponent<CapsuleCollider>();
            if (capsuleCollider != null)
            {
                capsuleCollider.enabled = false;
            }

            GetComponent<EnemyNavMesh>().enabled = false;
            GetComponent<NavMeshAgent>().enabled = false;

            // Calcular la rotación inversa del enemigo
            Quaternion reverseRotation = transform.rotation * Quaternion.Euler(0, 180, 0);

            // Instanciar una copia del Prefab de Sangre con la posición y rotación inversa del enemigo
            GameObject sangreInstance = Instantiate(Sangre, Sangre.transform.position, Sangre.transform.rotation);

            // Desvincular la sangre del padre actual
            sangreInstance.transform.parent = null;

            // Activar la sangre
            sangreInstance.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Animator not found!");
        }
    }
}
