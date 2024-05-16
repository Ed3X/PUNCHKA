using UnityEngine;

public class CombatController : MonoBehaviour
{
    public float meleeRange = 2f; // Alcance del golpe cuerpo a cuerpo
    public LayerMask targetLayer; // Capa de los objetivos contra los que se puede golpear

    public float increasedDamage = 30f; // Daño aumentado durante el power-up

    private Animator animator;
    private Camera mainCamera;
    private bool isPowerUpActive = false; // Indica si el power up de daño está activo

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

        // Realizar un ataque cuerpo a cuerpo cuando se presione el botón de ataque y el power-up esté activo
        if (Input.GetButtonDown("Fire1") && isPowerUpActive) // Cambiar "Fire1" según el input que uses para el ataque
        {
            MeleeAttackWithIncreasedDamage();
        }
    }

void MeleeAttackWithIncreasedDamage()
{
    // Detectar los objetivos dentro del alcance del golpe
    Collider[] hitColliders = Physics.OverlapSphere(transform.position, meleeRange, targetLayer);

    // Realizar el ataque en todos los objetivos dentro del alcance con daño aumentado
    foreach (Collider collider in hitColliders)
    {
        EnemyHealthSystem enemyHealth = collider.GetComponent<EnemyHealthSystem>();
        if (enemyHealth != null)
        {
            // Convertir el daño aumentado a un entero antes de pasarlo
            int damage = Mathf.RoundToInt(increasedDamage);
            // Aplicar daño aumentado al enemigo
            enemyHealth.TakeDamage(damage); // Utiliza el daño aumentado durante el power-up
        }
    }

    // Activar la animación de ataque en el Animator
    animator.SetTrigger("Attack");
}

    // Método para activar/desactivar el power-up de daño
    public void SetPowerUpActive(bool isActive)
    {
        isPowerUpActive = isActive;
    }
}
