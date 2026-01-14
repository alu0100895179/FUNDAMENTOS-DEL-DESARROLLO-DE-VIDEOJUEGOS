using UnityEngine;
using UnityEngine.InputSystem;

// Me aseguro de que el objeto tenga sí o sí estos componente
[RequireComponent(typeof(Rigidbody2D), typeof(AudioSource), typeof(PlayerStats))]
public class PlayerMovement : MonoBehaviour
{
    // Asignar mis sonidos desde el Inspector
    [Header("Audio Setup")]
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip landSound;
    [SerializeField] private AudioClip walkSound;
    [SerializeField] private AudioClip pickupSound;
    [SerializeField] private AudioClip powerUpSound;
    [SerializeField] private AudioClip powerDownSound;
    [SerializeField] private AudioClip damageSound;
    [SerializeField] private AudioClip healSound;

    [Header("Movement Settings")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 10f;
    
    // Para ajustar con cuanta frecuencia se quiere se considere un paso
    [SerializeField] private float stepDistance = 0.9f; 
    private float distanceMoved = 0f;
    private Vector3 lastPosition;

    // Variables de control para saltos y movimiento
    private float h = 0f;
    private bool isGrounded = false;
    
    // Variable para el sonido de aterrizaje.
    // Guardo la velocidad vertical del frame anterior al aterrizaje 
    // ya que en el momento del choque la velocidad se vuelve 0 instantáneamente
    private float previousVelocityY; 

    // Referencias los componentes necesario
    private SpriteRenderer sprite;
    private Rigidbody2D rb2D;
    private Animator animator;
    private AudioSource audioSource;
    private PlayerStats playerStats;    // Para vidas y pociones 
    private Vector3 originalScale;

    // Capas para identificar suelo y plataformas
    private int groundLayer;
    private int invisiblePlatformLayer;

    void Start()
    {
        // ASigno las referencias necesarias al arrancar
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        rb2D = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();

        // También necesito hablar con el script de estadísticas (vidas, pociones)
        playerStats = GetComponent<PlayerStats>();

        // Necesito guardar la escala ya que las plataformas tienen otra
        originalScale = transform.localScale;
        
        // Congelo la rotación para que el personaje no gire 
        rb2D.constraints = RigidbodyConstraints2D.FreezeRotation;

        // Configuro los IDs de las capas para no usar strings en el Update (optimización)
        groundLayer = LayerMask.NameToLayer("Terrain");
        invisiblePlatformLayer = LayerMask.NameToLayer("PlatInv");

        lastPosition = transform.position;
    }

    // Ejecutar cada Frame - renderizado visual - Time.deltaTime
    void Update()
    {
        // Movimiento horizontal
        h = 0f;
        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) h = -1f;
        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) h = 1f;

        // Gestión del Salto
        if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
        {
            PerformJump();
        }

        // ------------------------------------------------------------------------------------------------------
        // Implementé esto para testear rápido el audio de daño y curación     
        // Tecla M: Me curo
        if (Keyboard.current.mKey.wasPressedThisFrame)
        {
            if (playerStats != null)
            {
                playerStats.Heal(1);            // EL contador de vidas va al otro script
                PlayImportantSound(healSound);  // El feedback sonoro
            }
        }

        // Tecla N: Me hago daño
        if (Keyboard.current.nKey.wasPressedThisFrame)
        {
            if (playerStats != null)
            {
                playerStats.TakeDamage(1);
                PlayImportantSound(damageSound);
            }
        }
        // ------------------------------------------------------------------------------------------------------

        // Calculo si toca reproducir sonido de paso
        HandleFootsteps();

        // 4. Actualizo los gráficos (voltear sprite y animaciones)
        if (h > 0.01f) transform.localScale = originalScale;
        else if (h < -0.01f) transform.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);

        animator.SetFloat("Speed", Mathf.Abs(h * speed));
        animator.SetFloat("VelocityY", rb2D.linearVelocity.y);
        
        // Si ya estoy cayendo, le aviso al animator que el salto "subida" terminó.
        if(rb2D.linearVelocity.y < 0) animator.SetBool("RealJump", false);
    }

    // Sincronizado con motor de físicas - Usa Time.fixedDeltaTime
    void FixedUpdate()
    {
        // Movimiento horizontal
        if (h != 0f)
        {
            // No dejamos 'Y' a 0 sino que mantenemos la velocidad que tenía
            rb2D.linearVelocity = new Vector2(h * speed, rb2D.linearVelocity.y);
        }
        else
        {
            // Freno en seco si no toco teclas (y no tengo padre, como plataformas móviles)
            // Mejor efecto en juegos 2D para evitar deslizamiento
            // Lo de comprobar el padre me dio muchos quebraderos de cabeza porque me frenaba al jugador en la plataforma
            if (transform.parent == null)
            {
                rb2D.linearVelocity = new Vector2(0, rb2D.linearVelocity.y);
            }
        }

        // Justo antes de terminar el frame físico, guardo a qué velocidad vertical iba.
        // Así, en el siguiente frame, si choco con el suelo, sabré si considerarlo "aterrizaje"
        previousVelocityY = rb2D.linearVelocity.y;
    }

    // Función de salto
    private void PerformJump()
    {
        rb2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        animator.SetBool("RealJump", true);
        
        // Lanzo el sonido de salto.
        audioSource.PlayOneShot(jumpSound, 1f);
        
        // Hasta que no toque el suelo, lo indicamos con booleano global y se transfiere a FSM
        isGrounded = false; 
        animator.SetBool("Grounded", false);
    }

    // Control de distancia de pasos y reproducción del sonido
    private void HandleFootsteps()
    {
        // Solo suenan pasos si estoy en el suelo
        if (isGrounded)
        {
            float moveDelta = Mathf.Abs(transform.position.x - lastPosition.x);
            distanceMoved += moveDelta;

            // Si he recorrido suficiente distancia desde el último paso
            if (distanceMoved >= stepDistance)
            {
                // Le doy un toque aleatorio al tono para que esa más natural
                audioSource.pitch = Random.Range(0.9f, 1.1f);
                
                // Los pasos los pongo un pelín más bajos (0.9)
                audioSource.PlayOneShot(walkSound, 0.9f);
                
                distanceMoved = 0f; // Reseteo contador para los pasos
            }
        }
        else
        {
            distanceMoved = 0f; 
        }
        lastPosition = transform.position;
    }

    // --- MÉTODOS PÚBLICOS  ---
    // He creado este método auxiliar para sonidos importantes
    public void PlayImportantSound(AudioClip clip)
    {
        if (clip != null) 
        {
            audioSource.pitch = 1f; // Quito la variación aleatoria de los pasos
            audioSource.PlayOneShot(clip, 1f);
        }
    }

    // Atajos para que los items llamen a una sola línea
    public void CollectItem() => PlayImportantSound(pickupSound);
    public void CollectPowerUp() => PlayImportantSound(powerUpSound);
    public void CollectPowerDown() => PlayImportantSound(powerDownSound);
    public void TakeDamage() => PlayImportantSound(damageSound);
    
    // Actualización de la fuerza de salto
    public void UpgradeJump(float newJumpForce) 
    { 
        jumpForce = newJumpForce; 
    }

    // --- GESTIÓN DE COLISIONES ---
    private void OnCollisionEnter2D(Collision2D collision)
    {
        bool isTerrain = collision.gameObject.layer == groundLayer;
        bool isInvisible = collision.gameObject.layer == invisiblePlatformLayer;

        if (isTerrain || isInvisible)
        {
            // Truco de la velocidad para aterrizajes
            // Compruebo 'previousVelocityY'
            // Si venía cayendo rápido (menos de -2f), considero que es un aterrizaje fuerte.
            if (!isGrounded && previousVelocityY < -2f)
            {
                Debug.Log("Previous Velocity: "+previousVelocityY);
                audioSource.pitch = 1f;
                audioSource.PlayOneShot(landSound, 1f);
            }

            // Hemos llegado al suelo
            isGrounded = true;
            animator.SetBool("Grounded", true);
            distanceMoved = 0f; // Reseteamos para los pasos 

            // Si caigo en una plataforma móvil, me hago hijo de ella para moverme a la vez.
            if (collision.gameObject.CompareTag("MovingPlatform"))
            {
                transform.SetParent(collision.transform);
                // Arreglo un bug visual ya que el padre tiene otra escala
                Vector3 pScale = collision.transform.localScale;
                if(pScale.x != 0) originalScale.x /= pScale.x; 
            }
            
            // Si toco una plataforma invisible, la hago aparecer.
            if (isInvisible)
            {
                SpriteRenderer pr = collision.gameObject.GetComponent<SpriteRenderer>();
                if (pr != null) pr.enabled = true;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == groundLayer || collision.gameObject.layer == invisiblePlatformLayer)
        {
            isGrounded = false;
            animator.SetBool("Grounded", false);

            // Si salto de la plataforma móvil, dejo de ser su hijo
            if (collision.gameObject.CompareTag("MovingPlatform"))
            {
                transform.SetParent(null);
            }
        }
    }
    
    private void OnCollisionStay2D(Collision2D collision)
    {
        // Por si acaso, confirmo que sigo en el suelo
        if (collision.gameObject.layer == groundLayer || collision.gameObject.layer == invisiblePlatformLayer)
        {
            isGrounded = true;
            animator.SetBool("Grounded", true);
        }
    }
}