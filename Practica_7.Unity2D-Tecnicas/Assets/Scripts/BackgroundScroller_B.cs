using UnityEngine;

// Cámara Móvil y fondos estáticos (con "salto")
public class BackgroundScroller_B : MonoBehaviour
{
    [Tooltip("La cámara principal que sigue al jugador")]
    [SerializeField] private Camera mainCamera; // Arrastraremos la Main Camera aquí

    [Tooltip("Los dos fondos que se alternarán")]
    [SerializeField] private Transform background_A;
    [SerializeField] private Transform background_B;

    // Variables para los fondos
    private Transform leftBackground;   // El fondo que esté a la izquierda
    private Transform rightBackground;  // El fondo que esté a la derecha
    private float spriteWidth;          // El ancho del sprite del fondo

    // Variables para la cámara 
    private float camX;

    void Start()
    {
        // Validar la cámara
        if (mainCamera == null)
        { 
            Debug.LogError("BackgroundScroller: No se ha asignado una 'mainCamera'.");
            this.enabled = false; // Desactiva este script si no hay cámara
            return; 
        }
        
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

        /* --- LÓGICA DE SALTO DE FONDO (Teleport) --- */

        /* --- Moverse a la DERECHA --- */
        // Si el centro de la cámara pasa el centro del fondo 'right'
        if (camX > rightBackground.position.x)
        {
            // 'left' salta a la derecha de 'right'
            leftBackground.position = new Vector3(rightBackground.position.x + spriteWidth, leftBackground.position.y, leftBackground.position.z);
            Debug.Log($"Movido {leftBackground.name} a la derecha de {rightBackground.name}");
        }
        
        /* --- Moverse a la IZQUIERDA --- */
        // Si el centro de la cámara pasa el centro del fondo 'left'
        else if (camX < leftBackground.position.x)
        {
            // 'right' salta a la izquierda de 'left'
            rightBackground.position = new Vector3(leftBackground.position.x - spriteWidth, rightBackground.position.y, rightBackground.position.z);
            Debug.Log($"Movido {rightBackground.name} a la izquierda de {leftBackground.name}");
        }
    }
}