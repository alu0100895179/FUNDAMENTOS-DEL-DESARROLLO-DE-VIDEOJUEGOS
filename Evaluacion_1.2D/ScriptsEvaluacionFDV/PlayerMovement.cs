using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 20f;
    
    private float h = 0f;
    private bool isGrounded = false;
    private bool isAttacking = false;

    // Componentes
    private SpriteRenderer sprite;
    private Rigidbody2D rb2D;
    private Animator animator;
    private Vector3 originalScale;

    private int groundLayer; // ID de la capa "Terrain"
    
    void Start()
    {
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        rb2D = GetComponent<Rigidbody2D>();
        originalScale = transform.localScale;
        rb2D.constraints = RigidbodyConstraints2D.FreezeRotation;

        groundLayer = LayerMask.NameToLayer("Terrain");
    }

    void Update()
    {
        animator.SetBool("IsAttacking", false);

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
        }
        
        // --- 3. LEER INPUT DE ATAQUE ---
        if (Keyboard.current.xKey.wasPressedThisFrame)
        {
            isAttacking = true;
            Debug.Log("Atacando");
            animator.SetBool("IsAttacking", true);
        }

        // --- 4. VOLTEAR EL SPRITE ---
        if (h > 0.01f)
            transform.localScale = originalScale;
        else if (h < -0.01f)
            transform.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);

        // --- 5. ACTUALIZAR ANIMATOR ---
        animator.SetFloat("Speed", h * h * speed);

    }

    void FixedUpdate()
    {
        // --- APLICAR MOVIMIENTO HORIZONTAL ---
        if (h != 0f)
        {
            rb2D.linearVelocity = new Vector2(h * speed, rb2D.linearVelocity.y);
        }
        else
        {
            // Frena al jugador si está en el suelo (sin 'parent' ya)
            rb2D.linearVelocity = new Vector2(0, rb2D.linearVelocity.y);
        }
    }

    // --- CONTROL DE COLISIÓN (SUELO Y ATAQUE) ---
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Comprueba si es suelo
        if (collision.gameObject.layer == groundLayer)
        {
            isGrounded = true;
            animator.SetBool("Grounded", isGrounded);
        }

        // Comprueba si ataca a un bandido
        if (isAttacking && collision.gameObject.CompareTag("Bandit"))
        {
            Bandit bandit = collision.gameObject.GetComponent<Bandit>();
            if (bandit != null)
                bandit.Die();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Comprueba si deja el suelo
        if (collision.gameObject.layer == groundLayer)
        {
            isGrounded = false;
            animator.SetBool("Grounded", isGrounded);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // Mantiene 'isGrounded' mientras esté en el suelo
        if (collision.gameObject.layer == groundLayer)
            isGrounded = true;

        // Comprueba si ataca a un bandido mientras está en contacto
        if (isAttacking && collision.gameObject.CompareTag("Bandit"))
        {
            Bandit bandit = collision.gameObject.GetComponent<Bandit>();
            if (bandit != null)
                bandit.Die();
        }
    }
    
    public void AttackFinished()
    {
        isAttacking = false;
        animator.SetBool("IsAttacking", false);
    }
}