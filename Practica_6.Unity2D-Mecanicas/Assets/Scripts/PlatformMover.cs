using System.Collections;
using UnityEngine;

public class PlatformMover : MonoBehaviour
{
    // Variables de clase (asumidas del script original)
    [SerializeField] private float moveDistance = 3f;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float waitTime = 0.5f;

    private Rigidbody2D rb;
    private Vector2 startPosition;
    private Vector2 endPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Las plataformas móviles deben ser Kinematic
        // para que se muevan por script y no por físicas externas
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate; // Para movimiento más suave

        // Guardamos las posiciones de inicio y fin
        startPosition = rb.position;
        endPosition = new Vector2(startPosition.x + moveDistance, startPosition.y);

        // Iniciamos la Corutina que gestionará el bucle
        StartCoroutine(MoveLoop());
    }

    // Corrutina principal que orquesta el ciclo.
    private IEnumerator MoveLoop()
    {
        while (true) // Bucle infinito
        {
            // --- 1. Mover a la posición final (derecha) ---
            // Llamamos a la nueva función y esperamos a que termine
            yield return StartCoroutine(MoveToTarget(endPosition));
            
            // --- 2. Esperar en el destino ---
            yield return new WaitForSeconds(waitTime);

            // --- 3. Mover a la posición inicial (izquierda) ---
            // Reutilizamos la misma función, pero con el otro destino
            yield return StartCoroutine(MoveToTarget(startPosition));

            // --- 4. Esperar en el inicio ---
            yield return new WaitForSeconds(waitTime);
        }
    }

    // --- ¡NUEVA FUNCIÓN DE MOVIMIENTO REUTILIZABLE! ---
    // Esta corrutina se encarga de mover el rb a un punto
    // y se termina cuando llega.
    private IEnumerator MoveToTarget(Vector2 targetPosition)
    {
        // Mientras la distancia al objetivo sea mayor a un margen pequeño...
        while (Vector2.Distance(rb.position, targetPosition) > 0.01f)
        {
            // Calculamos la nueva posición para este frame
            Vector2 newPos = Vector2.MoveTowards(
                rb.position, 
                targetPosition, 
                moveSpeed * Time.fixedDeltaTime // Usar fixedDeltaTime por ser física (Kinematic)
            );
            
            // Aplicamos el movimiento al Rigidbody
            rb.MovePosition(newPos);
            
            // Esperamos al siguiente ciclo de físicas
            yield return new WaitForFixedUpdate();
        }

        // Al salir del bucle, nos aseguramos de que quede exactamente
        // en la posición final, para evitar errores de redondeo.
        rb.MovePosition(targetPosition);
    }
}