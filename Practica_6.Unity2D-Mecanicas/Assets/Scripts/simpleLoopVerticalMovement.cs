using UnityEngine;
using System.Collections; // Requerido para Corrutinas

public class simpleLoopVerticalMovement : MonoBehaviour
{
    [Tooltip("Distancia que se moverá a la derecha desde el punto inicial")]
    public float movementDistance = 7f;

    [Tooltip("Velocidad de movimiento")]
    public float speed = 1f;

    [Tooltip("Segundos a esperar en cada extremo")]
    public float waitTime = 1.5f;

    private Vector3 startPosition;
    private Vector3 newPosition;
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
        newPosition = startPosition + new Vector3(0, movementDistance, 0);

        // 2. Iniciamos el bucle
        movementCoroutine = StartCoroutine(MovementLoop());
    }

    private IEnumerator MovementLoop()
    {
        // Bucle infinito
        while (true)
        {
            // --- 1. MOVER HACIA ARRIBA ---
            currentSpeed = speed; // El personaje se está moviendo
            
            while (transform.position != newPosition)
            {
                transform.position = Vector3.MoveTowards(transform.position, newPosition, speed * Time.deltaTime);
                yield return null; 
            }

            // --- 2. ESPERAR ---
            currentSpeed = 0f; // El personaje se detiene
            yield return new WaitForSeconds(waitTime);

            // --- 3. MOVER HACIA ABAJO (Volver al inicio) ---
            // Activa animación de correr y voltea el sprite
            currentSpeed = speed; // El personaje se está moviendo
            
            while (transform.position != startPosition)
            {
                transform.position = Vector3.MoveTowards(transform.position, startPosition, speed * Time.deltaTime);
                yield return null;
            }

            // --- 4. ESPERAR ---
            currentSpeed = 0f; // El personaje se detiene
            yield return new WaitForSeconds(waitTime);
            // El 'while(true)' hace que vuelva al paso 1
        }
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
