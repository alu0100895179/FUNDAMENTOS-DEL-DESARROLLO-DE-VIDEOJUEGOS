using System; // Necesario para 'Action'
using UnityEngine;

public class eventCollsionController : MonoBehaviour
{
    // Evento estático: cualquiera puede suscribirse sin necesidad de una referencia a este script.
    public static event Action OnPowerUpCollected;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Si chocamos con un objeto con la etiqueta "PowerUpShoot"
        if (collision.gameObject.CompareTag("PowerUpShoot"))
        {
            Debug.Log("Detector de colisión: PowerUpShoot");

            // Lanzamos el evento para que los oyentes reaccionen.
            OnPowerUpCollected?.Invoke();

            // Destruimos el power-up.
            Destroy(collision.gameObject);
        }
    }
}
