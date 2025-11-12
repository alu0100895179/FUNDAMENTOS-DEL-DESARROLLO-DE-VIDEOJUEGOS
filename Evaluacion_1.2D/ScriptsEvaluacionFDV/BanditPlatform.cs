using UnityEngine;

// Script para las 3 plataformas que desactivan bandidos
[RequireComponent(typeof(Collider2D))]
public class BanditPlatform : MonoBehaviour
{
    [SerializeField] private GameObject banditToDeactivate;
    private bool hasBeenActivated = false; // Flag para activar solo una vez

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Salir si ya se activó o no es el "Player"
        if (hasBeenActivated || !collision.gameObject.CompareTag("Player"))
            return;

        // Comprobar si el jugador aterriza (choque vertical)
        // Para descargar contacto por los otros lados de la plataforma
        if (collision.contacts[0].normal.y < -0.5f)
        {
            hasBeenActivated = true; 

            // 1. Desactivar bandido
            if (banditToDeactivate != null)
                banditToDeactivate.SetActive(false);

            // 2. Avisar a PlayerStats que aumente el bonus
            PlayerStats stats = collision.gameObject.GetComponent<PlayerStats>();
            if (stats != null)
                stats.IncreaseTorchValue();

            // 3. Feedback visual (opcional)
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            if (sr != null)
                sr.color = Color.gray;
        }
    }
}