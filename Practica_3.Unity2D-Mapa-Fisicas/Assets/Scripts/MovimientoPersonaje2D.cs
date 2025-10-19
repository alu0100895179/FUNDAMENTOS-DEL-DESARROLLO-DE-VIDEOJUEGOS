using UnityEngine;
using UnityEngine.InputSystem; // Nuevo Input System

public class MovimientoPersonaje2D : MonoBehaviour
{
    public float speed = 5f;                  // unidades/segundo (avance local en Y)
    public float rotationSpeed = 180f;        // grados/segundo (rotación sobre Z)
    public float activationDistance = 5f;
    public string isWalkingParam = "isWalking";
    public string walkedFarParam = "walkedFar";
    public string facingParam = "isBack";

    private Vector3 prevPos;
    private float totalDistance = 0f;
    private bool walkedFar = false;

    public float idleThreshold = 3f;
    private float idleTimer = 0f;
    private bool idleFacingSet = false;

    private SpriteRenderer spriteRenderer;
    private Animator animator;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        prevPos = transform.position;
    }

    void Update()
    {
        // ----- 1) OBTENER ENTRADAS (rotación = eje X, avance = eje Y) -----
        float rotationInput = 0f; // izquierda/derecha -> rota sobre Z
        float forwardInput = 0f;  // adelante/atrás -> mueve en Y local

        bool anyArrowPressed = false;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.leftArrowKey.isPressed)
            {
                rotationInput = 1f; // girar a la izquierda (ccw)
                anyArrowPressed = true;
            }
            else if (Keyboard.current.rightArrowKey.isPressed)
            {
                rotationInput = -1f; // girar a la derecha (cw)
                anyArrowPressed = true;
            }

            if (Keyboard.current.upArrowKey.isPressed)
            {
                forwardInput = 1f; // avanzar (en la dirección que mira el objeto)
                anyArrowPressed = true;
                // opcional: marcar mirando hacia atrás si tu animador lo requiere
                animator.SetBool(isWalkingParam, true);
                animator.SetBool(facingParam, true); // ejemplo: mirar atrás
            }
            else if (Keyboard.current.downArrowKey.isPressed)
            {
                forwardInput = -1f; // retroceder
                anyArrowPressed = true;
                animator.SetBool(isWalkingParam, true);
                animator.SetBool(facingParam, false); // mirar frente
            }
        }

        // Si no se pulsa adelante/atrás, parar la animación de caminar (salvo que quieras otra lógica)
        if (forwardInput == 0f)
        {
            animator.SetBool(isWalkingParam, false);
        }

        // ----- 2) APLICAR ROTACIÓN (Z) -----
        float rotationAmount = rotationInput * rotationSpeed * Time.deltaTime;
        transform.Rotate(0f, 0f, rotationAmount);

        // ----- 3) APLICAR TRASLACIÓN EN ESPACIO LOCAL (avanzar en Y local) -----
        // Movemos en Y local para que la dirección dependa de la rotación previa
        Vector3 localMove = new Vector3(0f, forwardInput * speed * Time.deltaTime, 0f);
        transform.Translate(localMove, Space.Self);

        // ----- 4) MANTENER LÓGICA DE walkedFar / idle como tenías -----
        float distanceThisFrame = Vector3.Distance(prevPos, transform.position);
        totalDistance += distanceThisFrame;
        prevPos = transform.position;

        if (!walkedFar && totalDistance >= activationDistance)
        {
            walkedFar = true;
            animator.SetBool(walkedFarParam, true);
            totalDistance = 0f;
        }

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
                totalDistance = 0f;
                walkedFar = false;
            }
        }
        else
        {
            idleTimer = 0f;
            idleFacingSet = false;
        }

        // Nota: spriteRenderer.flipX no es necesario si tu personaje rota físicamente,
        // pero puedes seguir usándolo si tu animación lo requiere. Si rotas el objeto,
        // flipX normalmente **no** debe usarse para "mirar" (la rotación ya hace eso).
    }
}