using UnityEngine;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI ScoreText;

    private int score = 0;
    private int currentTorchValue = 1; // Multiplicador de antorchas

    void Start()
    {
        UpdateUI();
    }

    // Llamado por Collectible.cs (Antorcha). Recibe 'baseAmount' (que es 1)
    public void AddScore(int baseAmount)
    {
        score += (baseAmount * currentTorchValue); // Suma 1 * el multiplicador
        Debug.Log("Score sube a: " + score);
        UpdateUI();
    }

    // Llamado por BanditPlatform.cs
    public void IncreaseTorchValue()
    {
        currentTorchValue++;
        Debug.Log("Valor de antorcha sube a: " + currentTorchValue);
    }
    
    private void UpdateUI()
    {
        if (ScoreText != null)
            ScoreText.text = "" + score; // Actualiza el texto
    }
}
