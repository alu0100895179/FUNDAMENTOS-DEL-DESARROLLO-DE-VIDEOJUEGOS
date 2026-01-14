using UnityEngine;

public class CollectiblePotionPooled : MonoBehaviour
{
 
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Comprobamos si el objeto que ha entrado es el "Player"
        if (other.CompareTag("Player"))
        {
            // Busco los scripts necesarios dentro del jugador para comunicarme con él
            PlayerStats stats = other.GetComponent<PlayerStats>();
            PlayerMovement player = other.GetComponent<PlayerMovement>();
            
            if (stats != null && player != null)
            {
                stats.AddJumpPower(1); // Le decimos al script del jugador que sume 1 al contador de salto
                player.CollectItem(); // Reproducir el sonido de recolección
            }

            // Mejora de rendimiento:
            // No uso Destroy(gameObject);
            // Usamos SetActive(false). Esto "devuelve" el objeto al pool (lo apaga).
            gameObject.SetActive(false);
        }
    }
}