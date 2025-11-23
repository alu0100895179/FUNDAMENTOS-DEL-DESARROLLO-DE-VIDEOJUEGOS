using UnityEngine;

// Mueve el fondo cambiando el Offset de la textura, no el Transform
public class TextureScroller : MonoBehaviour
{
    [SerializeField] private float scrollSpeed = 0.5f;

    private Renderer rend;

    void Start()
    {
        // Obtenemos la referencia al Renderer
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        // 'Time.time' es el tiempo total desde que empezó el juego.
        // Multiplicado por la velocidad, nos da un 'offset' que siempre aumenta.
        float offset = Time.time * scrollSpeed;

        // El 'Wrap Mode: Repeat' se encarga de que la textura se repita
        // cuando el offset supera 1.
        // Solo movemos el eje X, el Y (vertical) se queda en 0.
        rend.material.mainTextureOffset = new Vector2(offset, 0);
    }
}