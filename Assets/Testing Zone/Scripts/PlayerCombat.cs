using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PlayerCombat : MonoBehaviour
{
    public int damage = 20; // Daño que inflige el jugador
    public LayerMask enemyLayer; // Capa de los enemigos
    public BoxCollider attackCollider; // Referencia al BoxCollider que representa el área de ataque
    public float autoTargetRange = 3f; // Rango de búsqueda de enemigos para autoenfoque
    public float speed = 20f; // Velocidad de movimiento del jugador para autoenfoque
    public float rotationSpeed = 20f;
    public float visionAngle = 120f;

    private Animator anim; // Referencia al Animator
    public bool isAttacking = false; // Flag para controlar si el jugador está atacando
    public float attackCooldown = 1f; // Tiempo de espera antes de otro ataque

    private void Start()
    {
        anim = GetComponent<Animator>(); // Obtener referencia al Animator
    }

    // Método para detectar y atacar a los enemigos
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

                // Calcular la rotación hacia el enemigo (solo en los ejes X y Z)
                Quaternion targetRotation = Quaternion.LookRotation(new Vector3(directionToEnemy.x, 0f, directionToEnemy.z), Vector3.up);

                // Rotar al jugador hacia el enemigo (solo en los ejes X y Z)
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

                // Verificar si el enemigo está dentro del cono de visión y del rango de alcance
                if (Vector3.Angle(transform.forward, directionToEnemy) <= visionAngle && Vector3.Distance(transform.position, nearestEnemy.position) <= autoTargetRange)
                {
                    // Calcular la posición objetivo del jugador justo delante del enemigo
                    Vector3 targetPosition = nearestEnemy.position - directionToEnemy * (attackCollider.size.magnitude / 2 + 0.3f); // Agregar un pequeño offset para mantener la distancia
                    targetPosition.y = transform.position.y; // Mantener la misma altura del jugador

                    // Mover al jugador hacia la posición objetivo (solo en los ejes X y Z)
                    transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
                }
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


    // Método para manejar la detección de colisiones con enemigos
    //private void OnTriggerEnter(Collider attackCollider)
    //{
    //    // Si el área de ataque del jugador entra en contacto con un enemigo y el jugador está atacando
    //    if (isAttacking && enemyLayer == (enemyLayer | (1 << attackCollider.gameObject.layer)))
    //    {
    //        // Obtener el componente de salud del enemigo
    //        EnemyHealthSystem enemyHealth = attackCollider.GetComponent<EnemyHealthSystem>();

    //        // Infligir daño al enemigo
    //        enemyHealth.TakeDamage(damage);
    //    }
    //}

    // Dibujar gizmos para visualizar el área de ataque en el editor
    //private void OnDrawGizmosSelected()
    //{
    //    if (attackCollider != null)
    //    {
    //        Gizmos.color = Color.red;
    //        Gizmos.DrawWireCube(attackCollider.bounds.center, attackCollider.bounds.size);
    //    }
    //}
}
