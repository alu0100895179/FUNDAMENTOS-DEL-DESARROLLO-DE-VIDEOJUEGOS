using UnityEngine;
using UnityEngine.InputSystem; // Nuevo sistema de entrada

// Se necesita un Rigidbody2D en el objeto: movimiento físico.
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    // Variables ajustables en el Inspector
    [Header("Movimiento")]
    [SerializeField] private float speed = 5f;          // Velocidad de movimiento
    
    private float jumpForce;                            // Fuerza de salto
    private float h = 0f;                               // Para el input horizontal
    private bool isGrounded = false;                    // Flag para controlar si estamos en el suelo

    // Componentes
    private SpriteRenderer sprite;
    private Rigidbody2D rb2D;
    private Animator animator;
    private Vector3 originalScale;

    // Acceso a las capas que necesitamos
    private int groundLayer;            // Variable para 'cachear' el ID de la capa
    private int invisiblePlatformLayer; // ID de la capa "PlatInv"
    
    void Start()
    {
        // Obtener referencia al Animator y SpriteRenderer
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();

        rb2D = GetComponent<Rigidbody2D>();     // Obtenemos el Rigidbody
        originalScale = transform.localScale;   // Escala original para cuando interactuemos con plataforma (escala diferente)

        // Congelar la rotación Z para que el personaje no se caiga
        rb2D.constraints = RigidbodyConstraints2D.FreezeRotation;

        // Guardamos los ID de la capas "Terrain" y "PlatInv"
        groundLayer = LayerMask.NameToLayer("Terrain");
        invisiblePlatformLayer = LayerMask.NameToLayer("PlatInv");
    }

    // En Update se leen Inputs
    void Update()
    {
        // --- 1. LEER INPUT HORIZONTAL ---
        h = 0f;
        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
            h = -1f;

        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
            h = 1f;
            
        // --- 2. LEER INPUT DE SALTO ---
        if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
        {
            rb2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            Debug.Log("Saltando");
            animator.SetBool("RealJump",true);
        }

        // --- 3. VOLTEAR EL SPRITE ---
        if (h > 0.01f)
        {
            // Mirar a la derecha
            transform.localScale = originalScale;
        }
        else if (h < -0.01f)
        {
            // Mirar a la izquierda
            transform.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);
        }

        // Actualizamos el Animator usando traspasando Speed real (multiplicamos 2 veces por h para asegurar que sea positiva)
        animator.SetFloat("Speed", h * h * speed);
        // Pasamos la velocidad vertical (Y) al Animator.
        // Positivo = subiendo (saltando), Negativo = bajando (cayendo).
        animator.SetFloat("VelocityY", rb2D.linearVelocity.y);
        if(rb2D.linearVelocity.y<0)
            animator.SetBool("RealJump",false);
    }

    // FixedUpdate, para aplicar físicas
    void FixedUpdate()
    {
        // --- 4. APLICAR MOVIMIENTO HORIZONTAL ---
        if (h != 0f) // Si el jugador está pulsando A o D
        {
            // El control lo tiene el jugador
            // Calculamos la velocidad horizontal con h (izqd o dcha) y la velocidad que hayamos decidido
            // Indicamos que la velocidad en "Y" debe ser la misma que estaba gestionando
            rb2D.linearVelocity = new Vector2(h * speed, rb2D.linearVelocity.y);
        }
        else
        {
            // Si el jugador está quieto (h == 0). Frenamos al jugador:
            // si está en suelo ( NO está encima de una plataforma)
            if (transform.parent == null)
            {
                rb2D.linearVelocity = new Vector2(0, rb2D.linearVelocity.y);
            }
            // Si tiene un 'parent' (está en la plataforma)
            // No hacemos nada, dejamos que la plataforma mueva el Rigidbody libremente
        }
    }

    // Esta función la llamará 'PlayerStats' cuando se cumpla la condición
    public void UpgradeJump(float newJumpForce)
    {
        jumpForce = newJumpForce;
    }


    // --- CONTROL DE COLISIÓN (SUELO Y PLATAFORMAS MÓVILES / INVISIBLES) ---
    // Modificado teniendo en cuenta LAYERS
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Comprobamos si la capa del objeto es la capa "Ground"
        if (collision.gameObject.layer == groundLayer)
        {
            isGrounded = true;
            animator.SetBool("Grounded",isGrounded);

            // Lógica de plataformas móviles (comparando por Tag)
            if (collision.gameObject.CompareTag("MovingPlatform"))
            {
                transform.SetParent(collision.transform);

                // Compensamos nuestra escala original
                Vector3 platformScale = collision.transform.localScale;
                originalScale.x /= platformScale.x;
                originalScale.y /= platformScale.y;
                originalScale.z /= platformScale.z;

                Debug.Log("Anclado a la plataforma");
            }
        } else
        // Lógica de plataformas invisibles (comparando por Tag)
        if (collision.gameObject.layer == invisiblePlatformLayer)
        {
            // Habilito el salto
            isGrounded = true;
            animator.SetBool("Grounded",isGrounded);

            // Obtengo las referencias a su sprite y en caso de ser no visible, revierto este estado
            SpriteRenderer platformRenderer = collision.gameObject.GetComponent<SpriteRenderer>();
            if (platformRenderer != null && !platformRenderer.enabled)
            {
                // La hacemos visible
                platformRenderer.enabled = true;
                Debug.Log("Plataforma invisible revelada");
            }
        } 
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Comprobamos si la capa del objeto es la capa "Terrain" o "PlatInv"
        if ((collision.gameObject.layer == groundLayer) || (collision.gameObject.layer == invisiblePlatformLayer))
        {
            isGrounded = false;
            animator.SetBool("Grounded",isGrounded);

            // Adicionalmente, comprobamos si era una plataforma móvil
            if (collision.gameObject.CompareTag("MovingPlatform"))
            {
                // Restauramos la escala original ANTES de soltarnos
                Vector3 platformScale = collision.transform.localScale;
                originalScale.x *= platformScale.x;
                originalScale.y *= platformScale.y;
                originalScale.z *= platformScale.z;

                // Comprobación de seguridad
                if (transform.parent == collision.transform)
                {
                    transform.SetParent(null);
                }
                Debug.Log("Liberado de la plataforma");
            }
            
        }
    }
}