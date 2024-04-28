using UnityEngine;
using System;
using UnityEngine.AI;

public class EnemyHealthSystem : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public Color originalColor;
    public Color damageColor;
    public float damageFlashTime = 0.2f;
    public bool IsDead { get; private set; }

    private Material myMaterial;
    private Animator animator;

    // Evento para notificar cuando un enemigo es destruido
    public event Action OnEnemyDestroyed;

    void Start()
    {
        currentHealth = maxHealth;
        //myMaterial = GetComponent<Renderer>().material;
        //originalColor = myMaterial.color;

        animator = GetComponent<Animator>();
        GetComponent<EnemyNavMesh>().enabled = true;
        GetComponent<NavMeshAgent>().enabled = true;

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
        else
        {
            Debug.LogError("EnemySpawner not found!");
        }
    }

    private void Update()
    {
        animator = GetComponent<Animator>();
        if (Input.GetKeyDown(KeyCode.I))
        {
            Dead();
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Infligiendo daño al enemigo.");
        if (currentHealth <= 0)
        {
            Debug.Log("matando.");
            Dead();
        }
    }

    private void Dead()
    {
        IsDead = true;
        Debug.Log("Enemy died.");

        // Llamar al evento OnEnemyDestroyed
        OnEnemyDestroyed?.Invoke();

        //GetComponent<ChaseBehaviour>().enabled = false;

        // Activar la animación de muerte en el Animator
        animator.SetBool("Die", true);
        animator.SetBool("IsChasing", false);
        animator.SetBool("IsPlayerClose", false);
        animator.SetBool("IsPatrolling", false);
        animator.SetBool("IsAttacking", false);

        // Destruir recursivamente el GameObject y todos sus hijos
        // Esto se ha comentado porque parece que ya no quieres destruir el objeto
        //DestroyRecursive(gameObject);

        // Código original para instanciar Lootbag
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
        // Desactivar cualquier componente de movimiento o comportamiento
        // por ejemplo, si tienes un componente de persecución o ataque, desactívalos aquí
        GetComponent<EnemyNavMesh>().enabled = false;
        GetComponent<NavMeshAgent>().enabled = false;

    }

    //private void DestroyRecursive(GameObject obj)
    //{
    //    foreach (Transform child in obj.transform)
    //    {
    //        DestroyRecursive(child.gameObject);
    //    }
    //    Destroy(obj);
}


