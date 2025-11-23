using UnityEngine;

// Efecto Parallax con Scroll Infinito (Cámara Móvil)
public class ParallaxController2 : MonoBehaviour
{
    [Tooltip("La cámara principal que sigue al jugador")]
    [SerializeField] private Camera mainCamera;

    [Tooltip("Los dos fondos que se alternarán")]
    [SerializeField] private Transform background_A;
    [SerializeField] private Transform background_B;

    [Header("Configuración Parallax")]
    [Tooltip("0 = Se mueve igual que la cámara (estático respecto a ella), 1 = Se queda quieto en el mundo")]
    [SerializeField] [Range(0f, 1f)] private float parallaxFactor = 0.5f;

    // Variables para los fondos
    private Transform leftBackground;   // El fondo que esté a la izquierda
    private Transform rightBackground;  // El fondo que esté a la derecha
    private float spriteWidth;          // El ancho del sprite del fondo

    // Variables para la cámara y posición anterior
    private float camX;
    private float lastCamX;             // Para calcular el delta de movimiento

    void Start()
    {
        // Validar la cámara
        if (mainCamera == null)
        { 
            Debug.LogError("ParallaxController: No se ha asignado una 'mainCamera'.");
            this.enabled = false;
            return; 
        }
        
        // Inicializamos la posición anterior de la cámara
        lastCamX = mainCamera.transform.position.x;

        // 'bounds.size.x' es el ancho del fondo
        spriteWidth = background_A.GetComponent<SpriteRenderer>().bounds.size.x;

        // Posicionamos B a la derecha de A
        background_B.position = new Vector3(background_A.position.x + spriteWidth, background_A.position.y, background_A.position.z);
    }

    // Usamos LateUpdate para ejecutar DESPUÉS de que la cámara se mueva
    void LateUpdate()
    {
        if (mainCamera == null) return;

        camX = mainCamera.transform.position.x;

        /* --- LÓGICA DE MOVIMIENTO PARALLAX --- */
        // 1. Calculamos cuánto se ha movido la cámara en este frame
        float deltaX = camX - lastCamX;
        
        // 2. Calculamos cuánto debe moverse el fondo (en dirección opuesta o más lento)
        // Si parallaxFactor es 1, el fondo no se mueve (se queda en el mundo).
        // Si es 0, se mueve con la cámara (parece pegado a la pantalla).
        // Queremos moverlo un porcentaje del movimiento de la cámara.
        float parallaxSpeed = deltaX * parallaxFactor;

        // 3. Movemos ambos fondos
        background_A.Translate(new Vector3(parallaxSpeed, 0, 0));
        background_B.Translate(new Vector3(parallaxSpeed, 0, 0));

        // Actualizamos la última posición de la cámara para el siguiente frame
        lastCamX = camX;


        /* --- LÓGICA DE SALTO DE FONDO (Teleport Infinito) --- */
        
        // Identificar qué fondo es 'left' y cuál 'right'
        if (background_A.position.x < background_B.position.x)
        {
            leftBackground = background_A;
            rightBackground = background_B;
        }
        else
        {
            leftBackground = background_B;
            rightBackground = background_A;
        }

        /* --- Moverse a la DERECHA --- */
        // Si el centro de la cámara pasa el centro del fondo 'right'
        if (camX > rightBackground.position.x)
        {
            // 'left' salta a la derecha de 'right'
            leftBackground.position = new Vector3(rightBackground.position.x + spriteWidth, leftBackground.position.y, leftBackground.position.z);
        }
        
        /* --- Moverse a la IZQUIERDA --- */
        // Si el centro de la cámara pasa el centro del fondo 'left'
        else if (camX < leftBackground.position.x)
        {
            // 'right' salta a la izquierda de 'left'
            rightBackground.position = new Vector3(leftBackground.position.x - spriteWidth, rightBackground.position.y, rightBackground.position.z);
        }
    }
}