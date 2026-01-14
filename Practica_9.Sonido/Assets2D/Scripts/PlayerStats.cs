using UnityEngine;
using TMPro;                // Para poder enviar texto a la UI
using System.Collections;   // Para uso de corrutinas
using System.Text;
using UnityEngine.Rendering;          // Necesario para StringBuilder

public class PlayerStats : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI jumpPowerText;     // Aquí pinto el contador para la mejora de salto

    [Header("Health System")]
    [SerializeField] private int lives = 5;            // Vidas actuales
    [SerializeField] private GameObject[] hearts;      // Array para guardar los sprites de las vidas

    [Header("Jump Power")]
    [SerializeField] private int jumpPowerThreshold = 3;            // El objetivo para lograr el poder
    [SerializeField] private float baseJumpForce = 14f;             // Valor original de salto
    [SerializeField] private float upgradedJumpValue = 20f;         // El nuevo valor de salto

    [Header("Effects")]
    [SerializeField] private Color upgradeColor = Color.yellow;     // Color al mejorar 
    [SerializeField] private float upgradeColorDuration = 10.0f;    // Duración del efecto

    
    private int currentJumpPower = 0;                               // El contador de la mejora
    private PlayerMovement playerMovement;                          // Referencia al script de movimiento
    private SpriteRenderer playerSpriteRenderer;                    // Referencia al SpriteRenderer del jugador
    private Color originalPlayerColor;                              // Para guardar el color inicial
    private StringBuilder sb = new StringBuilder();                 // Cacheamos el "Builder"

    void Start()
    {
        // Obtenemos la referencia al otro script en el mismo objeto (nuestro jugador)
        playerMovement = GetComponent<PlayerMovement>();
        // Al empezar, nos aseguramos de que el PlayerMovement
        // tenga el salto base que define este script.
        if (playerMovement != null)
        {
            playerMovement.UpgradeJump(baseJumpForce);
        }

        UpdateUI();

        // Obtener el SpriteRenderer del jugador
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
        if (playerSpriteRenderer != null)
        {
            originalPlayerColor = playerSpriteRenderer.color;   // Guardar el color original
        }
    }

    // Función para quitar las vidas indicadas
    public void TakeDamage(int damage)
    {
        lives -= damage;

        // Nos aseguramos no bajar de 0 vidas
        if (lives < 0) lives = 0;
        UpdateLivesUI();    // Actualizamos los corazones visuales
        Debug.Log("Vidas: "+lives);
    }

    // Función para incrimentar vidas
    public void Heal(int amount)
    {
        lives += amount;
        
        // Evitar tener más vidas que la cantidad de corazones en la UI
        if (lives > hearts.Length) lives = hearts.Length;
        UpdateLivesUI();
        Debug.Log("Vidas: "+lives);
    }

    private void UpdateLivesUI()
    {
        // Recorremos todos los objetos de vidas en el array
        for (int i = 0; i < hearts.Length; i++)
        {
            // Si el índice es menor que las vidas actuales, el corazón se ve (true)
            // Si el índice es mayor o igual, el corazón se apaga (false)
            if (i < lives)
            {
                hearts[i].SetActive(true);
            }
            else
            {
                hearts[i].SetActive(false);
            }
        }
    }

    // Esta es la función pública que llamará el script de la poción
    public void AddJumpPower(int amount)
    {
        currentJumpPower += amount;
        Debug.Log("JumpPower sube a: " + currentJumpPower);
        // Actualizamos el texto cada vez que sumamos
        UpdateUI();

        // Comprobamos si hemos llegado al objetivo
        if (currentJumpPower >= jumpPowerThreshold)
        {
            // Aplicamos la mejora
            Debug.Log("Mejora de salto activada");
            playerMovement.UpgradeJump(upgradedJumpValue);

            // Aplicar efectos gráficos de la mejora de salto
            StartCoroutine(ApplyPowerUp());
        }
    }

    // Método para actualizar la UI optimizado para consumo de recursos
    private void UpdateUI()
    {
        if (jumpPowerText != null)
        {
            sb.Clear();                     // Limpiamos el builder anterior
            sb.Append(currentJumpPower);
            jumpPowerText.text = sb.ToString();
        }
    }

    // Corrutina para aplicar efectos y resetear el contador
    private IEnumerator ApplyPowerUp()
    {
        if (playerSpriteRenderer != null)
        {
            playerSpriteRenderer.color = upgradeColor;              // Cambiar a color de mejora
            currentJumpPower = 0;                                   // Restablecemos Jump Power
            UpdateUI();                                             // Actualizamos UI
            playerMovement.CollectPowerUp();                        // Enviar sonido de la mejora de salto
            yield return new WaitForSeconds(upgradeColorDuration);  // Esperar antes de reestablecer
            playerMovement.UpgradeJump(baseJumpForce);              // Restablecemos salto                          
            playerSpriteRenderer.color = originalPlayerColor;       // Volver al color original
             playerMovement.CollectPowerDown();                     // Enviar sonido de fin de la mejora de salto
        }
    }
}

