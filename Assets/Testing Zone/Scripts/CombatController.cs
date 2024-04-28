using UnityEngine;

public class CombatController : MonoBehaviour
{
    public float meleeRange = 2f; // Alcance del golpe cuerpo a cuerpo
    public LayerMask targetLayer; // Capa de los objetivos contra los que se puede golpear

    private Animator animator;
    private Camera mainCamera;

    void Start()
    {
        animator = GetComponent<Animator>();
        mainCamera = Camera.main;
    }

    void Update()
    {
        // Detectar la dirección hacia donde apunta el ratón
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 targetDirection = hit.point - transform.position;
            targetDirection.y = 0f; // Ignorar la componente Y para que el personaje no mire hacia arriba o abajo

            // Girar el personaje hacia la dirección del ratón
            transform.rotation = Quaternion.LookRotation(targetDirection);
        }

        // Realizar un ataque cuerpo a cuerpo cuando se presione el botón de ataque
        if (Input.GetButtonDown("Fire1")) // Cambiar "Fire1" según el input que uses para el ataque
        {
            MeleeAttack();
        }
    }

    void MeleeAttack()
    {
        // Detectar los objetivos dentro del alcance del golpe
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, meleeRange, targetLayer);

        // Realizar el ataque en todos los objetivos dentro del alcance
        foreach (Collider collider in hitColliders)
        {
            EnemyHealthSystem enemyHealth = collider.GetComponent<EnemyHealthSystem>();
            if (enemyHealth != null)
            {
                // Aplicar daño al enemigo
                enemyHealth.TakeDamage(20); // Ajusta "damageAmount" según la cantidad de daño que quieras que haga el ataque
            }
        }

        // Activar la animación de ataque en el Animator
        animator.SetTrigger("Attack");
    }
}
