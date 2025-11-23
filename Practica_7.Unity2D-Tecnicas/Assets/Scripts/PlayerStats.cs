using UnityEngine;
using TMPro;                // Para poder enviar texto a la UI
using System.Collections;   // Para uso de corrutinas
using System.Text;          // Necesario para StringBuilder

public class PlayerStats : MonoBehaviour
{
    [Header("Jump Power")]
    [SerializeField] private int jumpPowerThreshold = 3;            // El objetivo para lograr el poder
    [SerializeField] private float baseJumpForce = 14f;             // Valor original de salto
    [SerializeField] private float upgradedJumpValue = 20f;         // El nuevo valor de salto

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI jumpPowerText;

    private int currentJumpPower = 0;                               // El contador
    private PlayerMovement playerMovement;                          // Referencia al script de movimiento

    [Header("Effects")]
    [SerializeField] private Color upgradeColor = Color.yellow;     // Color al mejorar
    [SerializeField] private float upgradeColorDuration = 10.0f;    // Duración del efecto

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
            yield return new WaitForSeconds(upgradeColorDuration);  // Esperar
            playerMovement.UpgradeJump(baseJumpForce);              // Restablecemos salto                          
            playerSpriteRenderer.color = originalPlayerColor;       // Volver al color original
        }
    }
}

