using UnityEngine;

// Movimiento estilo Endless Runner: automático constante aplicando la fórmula de capas
public class AutoParallaxScroller : MonoBehaviour
{
    [Tooltip("Velocidad base del scroll")]
    [SerializeField] private float scrollSpeed = 0.5f;

    [Tooltip("Dirección del movimiento (Vector2.right mueve la textura a la derecha, lo que hace que el fondo parezca ir a la izquierda)")]
    [SerializeField] private Vector2 direction = Vector2.right;

    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        // Iteramos por todos los materiales (capas) asignados al Quad
        for (int i = 0; i < rend.materials.Length; i++)
        {
            Material m = rend.materials[i];

            // Calculamos el desplazamiento base (Speed * Time.deltaTime)
            float speedOffset = scrollSpeed * Time.deltaTime;

            // Aplicamos la fórmula: (Magnitud * Dirección) / (Índice + 1.0)
            // Más cercano a 0 es que está más cercano al jugador y por lo tanto lo moveremos más rápido
            Vector2 movementStep = (speedOffset * direction) / (i + 1.0f);

            // Sumamos este paso al offset actual de la textura ("_MainTex" es la propiedad estándar)
            m.mainTextureOffset += movementStep;
        }
    }
}
