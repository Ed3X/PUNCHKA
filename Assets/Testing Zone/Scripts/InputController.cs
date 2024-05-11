using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    PlayerInput playerInput;
    InputAction moveAction;
    InputAction dashAction;
    InputAction pegarAction;

    PlayerCombat playerCombat;
    PlayerHealthSystem playerHealth;

    [SerializeField] private float speed = 5f;

        // Método público para obtener el valor de speed
        public float GetSpeed()
        {
            return speed;
        }

        // Método público para establecer el valor de speed
        public void SetSpeed(float newSpeed)
        {
            speed = newSpeed;
        }

    [SerializeField] float gravity = 9.81f; // Gravedad en m/s^2
    [SerializeField] float forceMultiplier = 0.1f; // Multiplicador de fuerza
    [SerializeField] float dashDistance = 5f; // Distancia del dash

     [SerializeField] private float dashCooldown = 1f; // Tiempo de espera para volver a dashear

    // Método público para obtener el valor de dashCooldown
    public float GetDashCooldown()
    {
        return dashCooldown;
    }

    // Método público para establecer el valor de dashCooldown
    public void SetDashCooldown(float newDashCooldown)
    {
        dashCooldown = newDashCooldown;
    }


    [SerializeField] float dashDuration = 0.2f; // Espera del dash en segundos
    [SerializeField] float rotationSpeed = 10f;
    [SerializeField] float lateralSpeed = 0.1f;

    CharacterController characterController;
    Camera mainCamera;

    private Animator anim;
    Vector3 velocity; // Vector de velocidad para la gravedad
    bool isDashing = false; // Bandera para controlar si se est� realizando un dash
    bool isPegando = false;
    bool isMoving = false;
    internal static object instance;

    //private bool isMousePressed = false;
    //private bool canRotate = true; // Indica si el personaje puede rotar hacia la direcci�n del rat�n

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions.FindAction("Move");
        dashAction = playerInput.actions.FindAction("Dash");
        pegarAction = playerInput.actions.FindAction("Pegar");
        characterController = GetComponent<CharacterController>();
        mainCamera = Camera.main; // Obtenemos la c�mara principal
        moveAction.Enable();
        dashAction.Enable();
        anim = GetComponent<Animator>();

        playerCombat = GetComponent<PlayerCombat>();
        playerHealth = GetComponent<PlayerHealthSystem>();
    }

    //IEnumerator CooldownRotation()
    //{
    //canRotate = false; // El personaje no puede rotar
    //yield return new WaitForSeconds(3f); // Espera 3 segundos
    //canRotate = true; // El personaje puede rotar nuevamente
    //}
    void Update()
    {
        // Verifica si el jugador est� atacando
        if (!playerCombat.isAttacking) // Si no est� atacando, permite el movimiento
        {
            MovePlayer(); // Mueve al personaje en un eje fijo
        }

        // Verifica si se ha presionado el bot�n de dash y realiza el dash si es as�
        if (dashAction.triggered && !isDashing && !playerCombat.isAttacking)
        {
            StartCoroutine(Dash());
        }

        if (pegarAction.triggered && !isPegando && !playerCombat.isAttacking)
        {
            // Llamar a la funci�n de ataque del PlayerCombat
            playerCombat.Attack();
        }
    }


    void MovePlayer()
    {
        // Verifica si el jugador est� atacando
        if (playerCombat.isAttacking)
        {
            // Si el jugador est� atacando, no permite el movimiento
            return;
        }
        Vector2 inputDirection = moveAction.ReadValue<Vector2>();
        Vector3 moveDirection = new Vector3(inputDirection.x, 0, inputDirection.y);

        // Si hay entrada de movimiento
        if (moveDirection.magnitude > 0.1f)
        {
            // Actualiza la variable isMoving
            isMoving = true;

            // Calcula la rotaci�n hacia la direcci�n del movimiento
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            //Interpola suavemente la rotaci�n actual hacia la rotaci�n objetivo
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);



            // Normaliza el vector de movimiento para evitar la velocidad adicional
            moveDirection.Normalize();

            // Mueve al personaje en la direcci�n de entrada basada en las teclas de direcci�n o el joystick
            characterController.Move(moveDirection * speed * Time.deltaTime);
        }
        else
        {
            // Actualiza la variable isMoving
            isMoving = false;
        }

        ApplyGravity();

        // Actualiza las animaciones
        anim.SetFloat("VelX", moveDirection.x);
        anim.SetFloat("VelY", moveDirection.z);
    }


    IEnumerator Dash()
    {
        if (isDashing)
        {
            yield break; // Si ya se est� realizando un dash, salir del m�todo
        }

        isDashing = true; // Indica que se est� realizando un dash

        playerHealth.isInvincible = true;

        // Activa la animaci�n de dash
        anim.SetBool("Esquivar", true);

        // Almacena la posici�n inicial del personaje antes del dash
        Vector3 startPos = transform.position;
        Vector3 dashDirection = transform.forward; // Direcci�n del dash es la direcci�n en la que el personaje est� mirando

        // Interpola suavemente la posici�n del personaje desde la posici�n inicial hasta la posici�n final durante el dash
        float elapsedTime = 0f;
        while (elapsedTime < dashDuration)
        {
            // Normaliza la direcci�n del dash para mantener la misma velocidad en cualquier direcci�n
            Vector3 finalDirection = (dashDirection * dashDistance);

            // Normaliza la velocidad lateral para evitar movimientos diagonales m�s r�pidos que en l�nea recta
            finalDirection = finalDirection.normalized * lateralSpeed;

            transform.position += finalDirection * Time.deltaTime;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Desactiva la animaci�n de dash al finalizar
        anim.SetBool("Esquivar", false);

        playerHealth.isInvincible = false;

        // Espera el tiempo de cooldown antes de permitir otro dash
        yield return new WaitForSeconds(dashCooldown);

        // Restablece la bandera de dash al finalizar el cooldown
        isDashing = false;

    }

    //public void OnDashAnimationEnd()
    //{
    //    // Reactivar las acciones de movimiento al finalizar el dash
    //    moveAction.Enable();
    //}





    void ApplyGravity()
    {
        if (!characterController.isGrounded)
        {
            velocity.y -= gravity * Time.deltaTime;
            characterController.Move(velocity * Time.deltaTime);
        }
        else
        {
            velocity.y = 0f;
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody rigidbody = hit.collider.attachedRigidbody;
        if (rigidbody != null && !rigidbody.isKinematic)
        {
            Vector3 force = hit.moveDirection * speed * forceMultiplier;
            rigidbody.AddForce(force, ForceMode.Acceleration);
        }
    }

    void OnDisable()
    {
        moveAction.Disable();
        dashAction.Disable();
    }

    void OnDestroy()
    {
        moveAction.Disable();
        dashAction.Disable();
    }
}
