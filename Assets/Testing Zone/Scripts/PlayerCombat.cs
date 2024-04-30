using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PlayerCombat : MonoBehaviour
{
    public int damage = 20; // Daño que inflige el jugador
    public LayerMask enemyLayer; // Capa de los enemigos
    public BoxCollider attackCollider; // Referencia al BoxCollider que representa el área de ataque
    public float autoTargetRange = 2f; // Rango de búsqueda de enemigos para autoenfoque
    public float speed = 20f; // Velocidad de movimiento del jugador para autoenfoque
    public float rotationSpeed = 20f;
    public float visionAngle = 120f;
    float attackSpeed = 2f;
    float attackRange = 2f;
    float maxAttackDistance = 1.5f; // Distancia máxima a la que puede acercarse el jugador al enemigo

    private Animator anim; // Referencia al Animator
    public bool isAttacking = false; // Flag para controlar si el jugador está atacando
    public float attackCooldown = 1f; // Tiempo de espera antes de otro ataque

    private void Start()
    {
        anim = GetComponent<Animator>(); // Obtener referencia al Animator
    }

    private void Update()
    {
        if (isAttacking)
        {
            // Calcular la dirección del ataque basado en la rotación actual del jugador
            Vector3 attackDirection = transform.forward;

            // Mueve al jugador solo en la dirección del ataque y a una velocidad reducida mientras ataca
            float moveSpeedWhileAttacking = 0.5f * attackSpeed; // Define la velocidad de movimiento mientras ataca (la mitad de la velocidad normal)

            // Realizar un Raycast en la dirección del movimiento para evitar atravesar obstáculos
            RaycastHit hit;
            if (Physics.Raycast(transform.position, attackDirection, out hit, attackRange))
            {
                // Si el Raycast golpea algo, ajustar la posición del jugador para evitar el obstáculo
                transform.position = hit.point - attackDirection * 0.1f; // Agregar un pequeño desplazamiento para evitar la colisión
            }
            else
            {
                // Si no hay obstáculo, mover al jugador en la dirección del ataque
                transform.position += attackDirection * moveSpeedWhileAttacking * Time.deltaTime;
            }
        }
    }


    // Método para detectar y atacar a los enemigos
    public void Attack()
    {
        // Si ya está atacando o está en cooldown, salir del método
        if (isAttacking)
            return;

        // Activar la animación de Ataque en el Animator
        anim.SetBool("Ataque", true);

        // Marcar que el jugador está atacando
        isAttacking = true;

        // Buscar el enemigo más cercano y mover al jugador hacia él
        AutoTargetEnemy();

        // Infligir daño al enemigo más cercano
        DealDamage();

        // Iniciar el cooldown del ataque
        StartCoroutine(AttackCooldown());
    }

    // Método para infligir daño al enemigo más cercano
    private void DealDamage()
    {
        // Obtener el primer collider en la capa de enemigos que se superpone con el área de ataque del jugador
        Collider enemyCollider = Physics.OverlapBox(attackCollider.bounds.center, attackCollider.bounds.extents, attackCollider.transform.rotation, enemyLayer).FirstOrDefault();

        // Verificar si se encontró un collider de enemigo
        if (enemyCollider != null)
        {
            // Obtener el componente de salud del enemigo
            EnemyHealthSystem enemyHealth = enemyCollider.GetComponent<EnemyHealthSystem>();

            // Infligir daño al enemigo si tiene un componente de salud
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
            }
        }
    }

    IEnumerator AttackCooldown()
    {
        // Esperar el tiempo de cooldown
        yield return new WaitForSeconds(attackCooldown);

        // Reiniciar la bandera de ataque y desactivar la animación
        isAttacking = false;
        anim.SetBool("Ataque", false);
    }

    // Método para buscar el enemigo más cercano dentro del rango de autoenfoque
    private void AutoTargetEnemy()
    {
        // Obtener todos los enemigos dentro del rango de autoenfoque
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, autoTargetRange, enemyLayer);

        if (hitColliders.Length > 0)
        {
            // Encontrar el enemigo más cercano dentro del cono de visión
            Transform nearestEnemy = FindNearestEnemyInCone(hitColliders);

            if (nearestEnemy != null)
            {
                // Calcular la dirección hacia el enemigo y normalizarla
                Vector3 directionToEnemy = (nearestEnemy.position - transform.position).normalized;
                directionToEnemy.y = 0f; // Limitar el movimiento al plano horizontal (ejes X y Z)

                // Si el jugador está atacando y está dentro del rango de ataque y no está demasiado cerca del enemigo
                if (isAttacking && Vector3.Distance(transform.position, nearestEnemy.position) <= attackRange && Vector3.Distance(transform.position, nearestEnemy.position) > maxAttackDistance)
                {
                    // Mover al jugador hacia el enemigo
                    Vector3 moveDirection = directionToEnemy * attackSpeed * Time.deltaTime;
                    transform.position += moveDirection;
                }

                // Calcular la rotación hacia el enemigo (solo en los ejes X y Z)
                Quaternion targetRotation = Quaternion.LookRotation(new Vector3(directionToEnemy.x, 0f, directionToEnemy.z), Vector3.up);

                // Rotar al jugador hacia el enemigo (solo en los ejes X y Z)
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

                // Configurar el IK de Upper Body para que mire hacia el enemigo
                // Obtener el componente Animator
                Animator animator = GetComponent<Animator>();

                // Calcular la posición objetivo del jugador justo delante del enemigo
                Vector3 targetPosition = nearestEnemy.position - (directionToEnemy * (attackRange)); // Agregar un pequeño offset para mantener la distancia
                targetPosition.y = transform.position.y; // Mantener la misma altura del jugador

                // Configurar la posición de mirada del Upper Body IK hacia el enemigo
                animator.SetLookAtPosition(targetPosition);
                animator.SetLookAtWeight(1.0f, 0.3f, 1.0f, 1.0f, 0.5f); // Ajusta los pesos según sea necesario
            }
        }
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
}
