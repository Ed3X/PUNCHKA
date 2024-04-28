using UnityEngine;
using System.Collections;

public class PlayerCombat : MonoBehaviour
{
    public int damage = 20; // Daño que inflige el jugador
    public LayerMask enemyLayer; // Capa de los enemigos
    public BoxCollider attackCollider; // Referencia al BoxCollider que representa el área de ataque

    private Animator anim; // Referencia al Animator
    private bool isAttacking = false; // Flag para controlar si el jugador está atacando
    public float attackCooldown = 0.5f; // Tiempo de espera antes de otro ataque

    private void Start()
    {
        anim = GetComponent<Animator>(); // Obtener referencia al Animator
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

    // Método para manejar la detección de colisiones con enemigos
    private void OnTriggerEnter(Collider attackCollider)
    {
        // Si el área de ataque del jugador entra en contacto con un enemigo y el jugador está atacando
        if (isAttacking && enemyLayer == (enemyLayer | (1 << attackCollider.gameObject.layer)))
        {
            // Obtener el componente de salud del enemigo
            EnemyHealthSystem enemyHealth = attackCollider.GetComponent<EnemyHealthSystem>();

            // Infligir daño al enemigo
            enemyHealth.TakeDamage(damage);
        }
    }

    // Dibujar gizmos para visualizar el área de ataque en el editor
    private void OnDrawGizmosSelected()
    {
        if (attackCollider != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(attackCollider.bounds.center, attackCollider.bounds.size);
        }
    }
}
