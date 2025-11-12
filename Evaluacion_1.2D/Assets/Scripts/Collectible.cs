using UnityEngine;

public class Collectible : MonoBehaviour
{
 
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Comprobamos si el objeto que ha entrado es el "Player"
        if (other.CompareTag("Player"))
        {
            // Intentamos obtener el script de Stats del jugador
            PlayerStats stats = other.GetComponent<PlayerStats>();
            
            if (stats != null)
            {
                // Le decimos al script del jugador que sume 1 al score
                stats.AddScore(1);
            }

            // Destruimos la poción (el objeto al que este script está asociado)
            Destroy(gameObject);
        }
    }
}
