using UnityEngine;
using UnityEngine.Events; // Para usar UnityEvent


// --- eventCollsionController.cs ---
// Añadiré este script al Jugador para definir el evento
public class eventCollsionController : MonoBehaviour
{
    [Header("Evento de Unity")]
    [Tooltip("Este evento se lanza al colisionar con el Tag correcto.")]
    public UnityEvent OnShootEventTriggered; // Evento público para el Inspector

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Comprobamos si chocamos con el Tag "ShootTagEvent"
        if (collision.gameObject.CompareTag("ShootTagEvent"))
        {
            Debug.Log("Detector de colisión: ShootTagEvent");

            // Lanzamos el evento para que los oyentes (configurados en el Inspector) reaccionen.
            // La '?' comprueba si OnShootEventTriggered no es nulo (si alguien se ha suscrito)
            OnShootEventTriggered?.Invoke();

            // Destruimos el power-up.
            Destroy(collision.gameObject);
        }
    }
}
