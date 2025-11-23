using UnityEngine;
using System.Collections; // Requerido para Corrutinas

public class simpleLoopLateralMovement : MonoBehaviour
{
    [Tooltip("Distancia que se moverá a la derecha desde el punto inicial")]
    public float movementDistance = 5f;

    [Tooltip("Velocidad de movimiento")]
    public float speed = 2f;

    [Tooltip("Segundos a esperar en cada extremo")]
    public float waitTime = 2.5f;

    private Vector3 startPosition;
    private Vector3 rightPosition;
    private Coroutine movementCoroutine;

    // --- NUEVAS VARIABLES PARA ANIMATOR Y VOLTEO ---
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private float currentSpeed; // Para calcular la velocidad actual
    // ----------------------------------------------

    void Start()
    {
        // Obtener referencia al Animator y SpriteRenderer
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // 1. Guardamos las posiciones
        startPosition = transform.position;
        rightPosition = startPosition + new Vector3(movementDistance, 0, 0);

        // 2. Iniciamos el bucle
        movementCoroutine = StartCoroutine(MovementLoop());
    }

    private IEnumerator MovementLoop()
    {
        // Bucle infinito
        while (true)
        {
            // --- 1. MOVER A LA DERECHA ---
            // Activa animación de correr y voltea el sprite
            currentSpeed = speed; // El personaje se está moviendo
            UpdateAnimationAndFlip(1); // 1 = se mueve a la derecha
            
            while (transform.position != rightPosition)
            {
                transform.position = Vector3.MoveTowards(transform.position, rightPosition, speed * Time.deltaTime);
                yield return null; 
            }

            // --- 2. ESPERAR ---
            currentSpeed = 0f; // El personaje se detiene
            UpdateAnimationAndFlip(0); // 0 = se detiene
            yield return new WaitForSeconds(waitTime);

            // --- 3. MOVER A LA IZQUIERDA (Volver al inicio) ---
            // Activa animación de correr y voltea el sprite
            currentSpeed = speed; // El personaje se está moviendo
            UpdateAnimationAndFlip(-1); // -1 = se mueve a la izquierda
            
            while (transform.position != startPosition)
            {
                transform.position = Vector3.MoveTowards(transform.position, startPosition, speed * Time.deltaTime);
                yield return null;
            }

            // --- 4. ESPERAR ---
            currentSpeed = 0f; // El personaje se detiene
            UpdateAnimationAndFlip(0); // 0 = se detiene
            yield return new WaitForSeconds(waitTime);
            
            // El 'while(true)' hace que vuelva al paso 1
        }
    }

    // --- NUEVO MÉTODO PARA ACTUALIZAR ANIMACIÓN Y VOLTEO ---
    private void UpdateAnimationAndFlip(int direction) // direction: 1=derecha, -1=izquierda, 0=parado
    {
        // Actualiza el parámetro 'Speed' en el Animator
        // comento esta línea porque solo será necesario cuando el animator lo requiere
        // animator.SetFloat("Speed", currentSpeed);

        // Voltea el sprite si es necesario
        if (direction == 1) // Moviéndose a la derecha
        {
            spriteRenderer.flipX = false; // El caballero mira a la derecha (por defecto)
        }
        else if (direction == -1) // Moviéndose a la izquierda
        {
            spriteRenderer.flipX = true; // Voltea el sprite para que mire a la izquierda
        }
        // Si direction es 0 (parado), no cambiamos el volteo, se queda en la última dirección
    }
    // --------------------------------------------------------

    void OnDisable()
    {
        if (movementCoroutine != null)
        {
            StopCoroutine(movementCoroutine);
        }
    }
}