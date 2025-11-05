using UnityEngine;

public class TimeScaleScript : MonoBehaviour
{
    [Tooltip("El Time.timeScale a aplicar al colisionar (ej: 0.5 para lento, 2.0 para rápido)")]
    public float timeScaleTarget = 0.5f;

    [Tooltip("Tag del objeto que puede activar este script")]
    public string targetTag = "Player";

    // Comprobación de la colisión
    private void OnCollisionEnter2D(Collision2D other)
    {
        // Comprobamos si el GameObject que ha chocado tiene el tag del jugador
        if (other.gameObject.CompareTag(targetTag))
        {
            // Aplicamos la  nueva velocidad en caso afirmativo
            Time.timeScale = timeScaleTarget;
        }
    }
}
