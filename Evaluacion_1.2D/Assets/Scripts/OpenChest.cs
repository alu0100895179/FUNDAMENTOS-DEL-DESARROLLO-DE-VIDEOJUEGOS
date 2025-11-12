using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections; // Necesario para Corrutinas

[RequireComponent(typeof(Animator))]
public class OpenChest : MonoBehaviour
{
    [SerializeField] private GameObject winTextObject;
    [SerializeField] private float restartDelay = 3f;

    private Animator anim;
    private bool hasBeenOpened = false; // Evita doble activación

    void Start()
    {
        anim = GetComponent<Animator>();

        if (winTextObject != null)
            winTextObject.SetActive(false); // Ocultar texto al inicio
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasBeenOpened)
        {
            hasBeenOpened = true;
            Debug.Log("¡Juego Terminado! Jugador alcanzó el cofre.");
            StartCoroutine(WinSequence()); // Iniciar secuencia de victoria
        }
    }

    // Corrutina para la secuencia de victoria
    private IEnumerator WinSequence()
    {
        // 1. Activar animación (Necesitas un bool "IsOpen" en el Animator)
        anim.SetBool("IsOpen", true); 

        // 2. Mostrar texto
        if (winTextObject != null)
            winTextObject.SetActive(true);

        // 3. Esperar
        yield return new WaitForSeconds(restartDelay);

        // 4. Reiniciar
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}