using UnityEngine;
using System;

public class EnemyHealthSystem : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public bool IsDead { get; private set; }

    private Animator animator;
    private BloodSpawner bloodSpawner;

    // Evento para notificar cuando un enemigo es destruido
    public event Action OnEnemyDestroyed;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        bloodSpawner = GetComponent<BloodSpawner>();
    }

    void Update()
    {
        // Verificar el daño simulado con la tecla 'I'
        if (Input.GetKeyDown(KeyCode.I))
        {
            TakeDamage(20);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Infligiendo daño al enemigo.");
        if (currentHealth <= 0)
        {
            Debug.Log("Enemy died.");
            Dead();
        }
        else
        {
            if (bloodSpawner != null)
            {
                bloodSpawner.SpawnBloodParticles();
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
        }
        else
        {
            Debug.LogWarning("Animator not found!");
        }

        // Destruir el GameObject después de 1 segundo
        Destroy(gameObject, 1f);
    }
}
