using UnityEngine;
using UnityEngine.InputSystem; // Nuevo Input System

public class MovimientoPersonaje2D : MonoBehaviour
{
    public float speed = 5f;                  // velocidad con la que se moverá en el transform
    public float activationDistance = 5f;     // distancia para activar walkedFar
    public string isWalkingParam = "isWalking"; // nombres de los parámetros en la máquina de estados
    public string walkedFarParam = "walkedFar";
    public string facingParam = "isBack";

    private float horizontal = 0f;              // para solo movernos en el eje X
    private Vector3 prevPos;                    // para saber cuanto vamos recorriendo
    private float totalDistance = 0f;
    private bool walkedFar = false;

    // --- Variables para el temporizador de inactividad ---
    private float idleTimer = 0f;
    private float idleThreshold = 3f;
    private bool idleFacingSet = false;
    // ----------------------------------------------------------

    private SpriteRenderer spriteRenderer;
    private Animator animator;

    // Función de incio
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        prevPos = transform.position;
    }

    // Actualizar en cada frame
    void Update()
    {
        // Por defecto mira al frente y sin movimiento
        horizontal = 0;

        bool anyArrowPressed = false;

        // Comprobación de errores (sin teclado)
        if (Keyboard.current != null)
        {
            if (Keyboard.current.leftArrowKey.isPressed ){            // Movimiento a la izquierda
                horizontal = -1f;
                spriteRenderer.flipX = false;
                animator.SetBool(isWalkingParam, true);
                anyArrowPressed = true;
            }
            else if (Keyboard.current.rightArrowKey.isPressed){       // Movimiento a la derecha
                horizontal = 1f;
                spriteRenderer.flipX = true;
                animator.SetBool(isWalkingParam, true);
                anyArrowPressed = true;
            }
            else if(Keyboard.current.upArrowKey.isPressed){
                horizontal = 0;                                      // Mirando hacia atrás
                animator.SetBool(isWalkingParam, false);
                animator.SetBool(facingParam, true);
                anyArrowPressed = true;
            }
            else if(Keyboard.current.downArrowKey.isPressed){
                horizontal = 0;                                      // Mirando hacia el frente
                animator.SetBool(isWalkingParam, false);
                animator.SetBool(facingParam, false);
                anyArrowPressed = true;
            }
        }
        // Mover en X (movimiento horizontal)
        transform.Translate(Vector2.right * horizontal * speed * Time.deltaTime);

        // Acumular distancia recorrida
        float distanceThisFrame = Vector3.Distance(prevPos, transform.position);
        totalDistance += distanceThisFrame;
        prevPos = transform.position;

        // Gestión de walkedFar
        if (!walkedFar && totalDistance >= activationDistance)
        {
           walkedFar = true;
           animator.SetBool(walkedFarParam, true);
           totalDistance = 0;
        }

        // Temporizador de inactividad: si no nos movemos y no pulsamos flechas durante 3s lo consideraremos inactividad y mirará al frente
        bool isMoving = distanceThisFrame > 0.01f;
        if (!isMoving && !anyArrowPressed)
        {
            idleTimer += Time.deltaTime;
            if (idleTimer >= idleThreshold && !idleFacingSet)
            {
                animator.SetBool(walkedFarParam, false);
                animator.SetBool(isWalkingParam, false);
                animator.SetBool(facingParam, false);
                idleFacingSet = true;
                totalDistance = 0;
                walkedFar = false;
            }
        }
        else
        {
            idleTimer = 0f;
            idleFacingSet = false;
        }
    }
}
