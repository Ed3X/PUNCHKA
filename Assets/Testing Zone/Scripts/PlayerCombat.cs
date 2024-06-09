using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerCombat : MonoBehaviour
{
    public int damage = 20; // Daño que inflige el jugador
    public LayerMask enemyLayer; // Capa de los enemigos
    public BoxCollider manoIz; // Referencia al BoxCollider que representa el área de ataque de la mano izquierda
    public BoxCollider manoDe; // Referencia al BoxCollider que representa el área de ataque de la mano derecha
    public float autoTargetRange = 2f; // Rango de búsqueda de enemigos para autoenfoque
    public float speed = 20f; // Velocidad de movimiento del jugador para autoenfoque
    public float rotationSpeed = 20f;
    public float visionAngle = 120f;
    float attackSpeed = 3f;
    float attackRange = 2f;
    float maxAttackDistance = 0.1f; // Distancia máxima a la que puede acercarse el jugador al enemigo

    private Animator anim; // Referencia al Animator
    public bool isAttacking = false; // Flag para controlar si el jugador está atacando
    public float attackCooldown = 0.3f; // Tiempo de espera antes de otro ataque

    private bool hasDealtDamage = false; // Bandera para controlar si se ha infligido daño
    private int baseDamage;

    private void Start()
    {
        anim = GetComponent<Animator>(); // Obtener referencia al Animator
        baseDamage = damage; // Guardar el daño base
    }

    private void Update()
    {
        if (isAttacking)
        {
            // Obtener todos los enemigos dentro del rango de autoenfoque
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, autoTargetRange, enemyLayer);

            // Encontrar el enemigo más cercano dentro del cono de visión
            Transform nearestEnemy = FindNearestEnemyInCone(hitColliders);
            if (nearestEnemy != null)
            {
                // Calcular la dirección hacia el enemigo y normalizarla
                Vector3 directionToEnemy = (nearestEnemy.position - transform.position);
                directionToEnemy.y = 0f; // Limitar el movimiento al plano horizontal (ejes X y Z)

                // Calcular la distancia al enemigo
                float distanceToEnemy = directionToEnemy.magnitude;

                // Si el jugador está demasiado cerca del enemigo, retrocede lentamente
                if (distanceToEnemy < maxAttackDistance)
                {
                    Vector3 moveDirection = -directionToEnemy.normalized;
                    transform.position += moveDirection * attackSpeed * Time.deltaTime;
                }
                else
                {
                    // Mover al jugador hacia el enemigo
                    Vector3 moveDirection = directionToEnemy.normalized;
                    transform.position += moveDirection * attackSpeed * Time.deltaTime;
                }

                // Calcular la rotación hacia el enemigo (solo en los ejes X y Z)
                Quaternion targetRotation = Quaternion.LookRotation(new Vector3(directionToEnemy.x, 0f, directionToEnemy.z), Vector3.up);

                // Rotar al jugador hacia el enemigo (solo en los ejes X y Z)
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

                // Infligir daño solo una vez por ataque
                if (!hasDealtDamage && nearestEnemy != null && enemyLayer == (enemyLayer | (1 << nearestEnemy.gameObject.layer)))
                {
                    DealDamage(nearestEnemy.GetComponent<EnemyHealthSystem>());
                    hasDealtDamage = true; // Marcar que se ha infligido daño
                }
            }
        }
    }

    public void Attack()
    {
        // Si ya está atacando o está en cooldown, salir del método
        if (isAttacking)
            return;

        // Marcar que el jugador está atacando
        isAttacking = true;
        hasDealtDamage = false; // Restablecer la bandera de daño

        // Activar la animación de Ataque en el Animator
        anim.SetTrigger("Ataque");

        // Iniciar el cooldown del ataque
        StartCoroutine(AttackCooldown());
    }

    IEnumerator AttackCooldown()
    {
        // Esperar el tiempo de cooldown
        yield return new WaitForSeconds(attackCooldown);

        // Reiniciar la bandera de ataque
        isAttacking = false;
    }

    // Método para encontrar el enemigo más cercano dentro del cono de visión
    private Transform FindNearestEnemyInCone(Collider[] colliders)
    {
        Transform nearestEnemy = null;
        float minDistance = float.MaxValue;

        foreach (Collider col in colliders)
        {
            // Calcular la dirección al enemigo
            Vector3 directionToEnemy = (col.transform.position - transform.position).normalized;

            // Verificar si el enemigo está dentro del cono de visión
            if (Vector3.Angle(transform.forward, directionToEnemy) <= visionAngle)
            {
                float distance = Vector3.Distance(transform.position, col.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestEnemy = col.transform;
                }
            }
        }

        return nearestEnemy;
    }

    // Método para infligir daño al enemigo más cercano
    private void DealDamage(EnemyHealthSystem enemyHealth)
    {
        // Verificar si se encontró un enemigo
        if (enemyHealth != null)
        {
            // Verificar si ambos colliders de las manos del jugador están colisionando con el enemigo al mismo tiempo
            if (manoIz.bounds.Intersects(enemyHealth.GetComponent<Collider>().bounds) || manoDe.bounds.Intersects(enemyHealth.GetComponent<Collider>().bounds))
            {
                // Infligir daño al enemigo
                enemyHealth.TakeDamage(damage);
            }
        }
    }

    // Método para establecer el daño
    public void SetDamage(int newDamage)
    {
        damage = newDamage;
    }

    // Método para restablecer el daño al valor base
    public void ResetDamage()
    {
        damage = baseDamage;
    }
}
