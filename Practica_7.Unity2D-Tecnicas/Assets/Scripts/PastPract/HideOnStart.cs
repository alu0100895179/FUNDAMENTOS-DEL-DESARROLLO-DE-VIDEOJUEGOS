using UnityEngine;
public class HideOnStart : MonoBehaviour
{
    void Start()
    {
        // Oculta el renderer al iniciar el juego
        GetComponent<SpriteRenderer>().enabled = false;
    }
}
