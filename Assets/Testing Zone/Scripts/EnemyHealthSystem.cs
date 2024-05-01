using UnityEngine;
using System;
using UnityEngine.AI;

public class EnemyHealthSystem : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public bool IsDead { get; private set; }

    private Animator animator;
    public GameObject Sangre;

    // Evento para notificar cuando un enemigo es destruido
    public event Action OnEnemyDestroyed;

    void Start()
    {
        currentHealth = maxHealth;
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

    //private void DestroyRecursive(GameObject obj)
    //{
    //    foreach (Transform child in obj.transform)
    //    {
    //        DestroyRecursive(child.gameObject);
    //    }
    //    // Programar la destrucción del objeto después de 2 segundos
    //    Destroy(obj, 2f);
    //}

}
