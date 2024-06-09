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

    private bool isFootstepPlaying = false; // Variable para controlar si el sonido de pasos está siendo reproducido

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
    bool isDashing = false; // Bandera para controlar si se está realizando un dash
    bool isPegando = false;
    public bool isMoving = false;
    internal static object instance;

    [SerializeField] private AudioClip asphaltFootstepClip; // Clip de sonido de pasos en asfalto
    [SerializeField] private AudioSource audioSource; // El componente AudioSource

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions.FindAction("Move");
        dashAction = playerInput.actions.FindAction("Dash");
        pegarAction = playerInput.actions.FindAction("Pegar");
        characterController = GetComponent<CharacterController>();
        mainCamera = Camera.main; // Obtenemos la cámara principal
        moveAction.Enable();
        dashAction.Enable();
        anim = GetComponent<Animator>();

        playerCombat = GetComponent<PlayerCombat>();
        playerHealth = GetComponent<PlayerHealthSystem>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // Verifica si el jugador está atacando
        if (!playerCombat.isAttacking) // Si no está atacando, permite el movimiento
        {
            MovePlayer(); // Mueve al personaje en un eje fijo
        }

        // Verifica si se ha presionado el botón de dash y realiza el dash si es así
        if (dashAction.triggered && !isDashing && !playerCombat.isAttacking)
        {
            StartCoroutine(Dash());
        }

        if (pegarAction.triggered && !isPegando && !playerCombat.isAttacking)
        {
            // Llamar a la función de ataque del PlayerCombat
            playerCombat.Attack();
        }
        PlayFootstepSound();
    }

    void MovePlayer()
    {
        // Verifica si el jugador está atacando
        if (playerCombat.isAttacking)
        {
            // Si el jugador está atacando, no permite el movimiento
            return;
        }
        Vector2 inputDirection = moveAction.ReadValue<Vector2>();
        Vector3 moveDirection = new Vector3(inputDirection.x, 0, inputDirection.y);

        // Si hay entrada de movimiento
        if (moveDirection.magnitude > 0.1f)
        {
            // Actualiza la variable isMoving
            isMoving = true;

            // Calcula la rotación hacia la dirección del movimiento
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            // Interpola suavemente la rotación actual hacia la rotación objetivo
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

            // Normaliza el vector de movimiento para evitar la velocidad adicional
            moveDirection.Normalize();

            // Mueve al personaje en la dirección de entrada basada en las teclas de dirección o el joystick
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
            yield break; // Si ya se está realizando un dash, salir del método
        }

        isDashing = true; // Indica que se está realizando un dash

        playerHealth.isInvincible = true;

        // Activa la animación de dash
        anim.SetBool("Esquivar", true);

        // Almacena la posición inicial del personaje antes del dash
        Vector3 startPos = transform.position;
        Vector3 dashDirection = transform.forward; // Dirección del dash es la dirección en la que el personaje está mirando

        // Interpola suavemente la posición del personaje desde la posición inicial hasta la posición final durante el dash
        float elapsedTime = 0f;
        while (elapsedTime < dashDuration)
        {
            // Normaliza la dirección del dash para mantener la misma velocidad en cualquier dirección
            Vector3 finalDirection = (dashDirection * dashDistance);

            // Normaliza la velocidad lateral para evitar movimientos diagonales más rápidos que en línea recta
            finalDirection = finalDirection.normalized * lateralSpeed;

            transform.position += finalDirection * Time.deltaTime;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Desactiva la animación de dash al finalizar
        anim.SetBool("Esquivar", false);

        playerHealth.isInvincible = false;

        // Espera el tiempo de cooldown antes de permitir otro dash
        yield return new WaitForSeconds(dashCooldown);

        // Restablece la bandera de dash al finalizar el cooldown
        isDashing = false;

    }

    private void PlayFootstepSound()
    {
        // Verifica si el jugador se está moviendo
        if (isMoving)
        {
            // Verifica si ya se está reproduciendo un sonido de pasos
            if (!isFootstepPlaying)
            {
                // Reproduce el sonido de pasos de asfalto
                audioSource.clip = asphaltFootstepClip;
                audioSource.loop = true; // Para que el sonido se repita mientras se mueve
                audioSource.Play();
                isFootstepPlaying = true;
            }
        }
        else
        {
            // Si el jugador no se está moviendo, detén la reproducción del audio de pasos
            if (isFootstepPlaying)
            {
                audioSource.Stop();
                isFootstepPlaying = false;
            }
        }
    }

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
