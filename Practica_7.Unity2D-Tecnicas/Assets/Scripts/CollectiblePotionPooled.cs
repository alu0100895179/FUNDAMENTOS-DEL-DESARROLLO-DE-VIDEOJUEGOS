using UnityEngine;

public class CollectiblePotionpooled : MonoBehaviour
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
                // Le decimos al script del jugador que sume 1
                stats.AddJumpPower(1);
            }

            // NO usamos Destroy(gameObject);
            // Usamos SetActive(false). Esto "devuelve" el objeto al pool (lo apaga).
            gameObject.SetActive(false);
        }
    }
}