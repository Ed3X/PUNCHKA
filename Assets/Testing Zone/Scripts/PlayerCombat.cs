using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PlayerCombat : MonoBehaviour
{
    public int damage = 20; // Daño que inflige el jugador
    public LayerMask enemyLayer; // Capa de los enemigos
    public BoxCollider manoIz; // Referencia al BoxCollider que representa el área de ataque
    public BoxCollider ManoDe; // Referencia al BoxCollider que representa el área de ataque
    public float autoTargetRange = 2f; // Rango de búsqueda de enemigos para autoenfoque
    public float speed = 20f; // Velocidad de movimiento del jugador para autoenfoque
    public float rotationSpeed = 20f;
    public float visionAngle = 120f;
    float attackSpeed = 3f;
    float attackRange = 2f;
    float maxAttackDistance = 1f; // Distancia máxima a la que puede acercarse el jugador al enemigo

    private Animator anim; // Referencia al Animator
    public bool isAttacking = false; // Flag para controlar si el jugador está atacando
    public float attackCooldown = 1f; // Tiempo de espera antes de otro ataque

    // Lista para mantener un registro de los enemigos golpeados durante el ataque actual
    List<EnemyHealthSystem> enemiesHitThisAttack = new List<EnemyHealthSystem>();

    private void Start()
    {
        anim = GetComponent<Animator>(); // Obtener referencia al Animator
    }

    private void Update()
    {
        if (isAttacking)
        {
            // Obtener todos los enemigos dentro del rango de autoenfoque
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, autoTargetRange, enemyLayer);

            // Encontrar el enemigo más cercano dentro del cono de visión
            Transform nearestEnemy = FindNearestEnemyInCone(hitColliders);

            //if (isAttacking)
            //{

            //    // Calcular la dirección del ataque basado en la rotación actual del jugador
            //    Vector3 attackDirection = transform.forward;

            //    // Mueve al jugador solo en la dirección del ataque y a una velocidad reducida mientras ataca
            //    float moveSpeedWhileAttacking = attackSpeed; // Define la velocidad de movimiento mientras ataca (la mitad de la velocidad normal)

            //    // Realizar un Raycast en la dirección del movimiento para evitar atravesar obstáculos
            //    RaycastHit hit;
            //    if (Physics.Raycast(transform.position, attackDirection, out hit, attackRange))
            //    {
            //        // Si el Raycast golpea algo, ajustar la posición del jugador para evitar el obstáculo
            //        transform.position = hit.point - attackDirection * 0.1f; // Agregar un pequeño desplazamiento para evitar la colisión
            //    }
            //    else
            //    {
            //        // Si no hay obstáculo, mover al jugador en la dirección del ataque
            //        transform.position += attackDirection * moveSpeedWhileAttacking * Time.deltaTime;
            //    }
            //}
            if (nearestEnemy != null)
            {
                Attack();
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

                if (nearestEnemy != null && enemyLayer == (enemyLayer | (1 << nearestEnemy.gameObject.layer)))
                {
                    DealDamage(nearestEnemy.GetComponent<EnemyHealthSystem>());
                    return;
                }


            }
            isAttacking = false;
        }
    }

    // Método para detectar y atacar a los enemigos
    public void Attack()
    {
        // Si ya está atacando o está en cooldown, salir del método
        if (isAttacking)
            return;

        // Limpiar la lista de enemigos golpeados durante este ataque
        enemiesHitThisAttack.Clear();

        // Activar la animación de Ataque en el Animator
        anim.SetBool("Ataque", true);

        // Marcar que el jugador está atacando
        isAttacking = true;

        // Iniciar el cooldown del ataque
        StartCoroutine(AttackCooldown());
    }

    IEnumerator AttackCooldown()
    {
        // Esperar el tiempo de cooldown
        yield return new WaitForSeconds(attackCooldown);

        // Reiniciar la bandera de ataque y desactivar la animación
        isAttacking = false;
        anim.SetBool("Ataque", false);
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
        // Verificar si se encontró un enemigo y si no ha sido golpeado ya durante este ataque
        if (enemyHealth != null && !enemiesHitThisAttack.Contains(enemyHealth))
        {
            // Agregar el enemigo a la lista de enemigos golpeados durante este ataque
            enemiesHitThisAttack.Add(enemyHealth);

            // Infligir daño al enemigo
            enemyHealth.TakeDamage(damage);
        }
    }
}
