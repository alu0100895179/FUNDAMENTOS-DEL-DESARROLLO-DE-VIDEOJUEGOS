using UnityEngine;

// Cámara Fija y desplazamiento del fondo
public class BackgroundScroller_A : MonoBehaviour
{
    [SerializeField] private float scrollSpeed = 2f;
    [SerializeField] private Transform currentBackground;   // Arrastraremos aquí Background_A
    [SerializeField] private Transform auxiliaryBackground; // Arrastraremos aquí Background_B

    private float spriteWidth;
    private Vector3 currentStartPosition;                   // La "posxini"

    void Start()
    {
        // Guardamos la posición inicial del primer fondo
        currentStartPosition = currentBackground.position;

        // Obtenemos el ancho
        spriteWidth = currentBackground.GetComponent<SpriteRenderer>().bounds.size.x;

        // Posicionamos el fondo auxiliar
        auxiliaryBackground.position = new Vector3(currentBackground.position.x + spriteWidth, currentBackground.position.y, currentBackground.position.z);
    }

    void Update()
    {
        /* Para dar sensación de desplazameinto, movemos ambos fondos a la izquierda*/
        Vector3 moveDelta = Vector2.left * scrollSpeed * Time.deltaTime;
        currentBackground.Translate(moveDelta, Space.World);
        auxiliaryBackground.Translate(moveDelta, Space.World);

        // Comprobamos si el fondo actual se ha movido la distancia de su ancho
        // Decido usar 'spriteWidth' completo
        if (currentBackground.position.x < currentStartPosition.x - spriteWidth)
        {
            // Teletransportamos el fondo actual a la derecha del auxiliar
            currentBackground.position = new Vector3(auxiliaryBackground.position.x + spriteWidth, currentBackground.position.y, currentBackground.position.z);

            // Intercambiamos los roles (auxiliar pasa aactual y viceversa)
            Transform temp = currentBackground;
            currentBackground = auxiliaryBackground;
            auxiliaryBackground = temp;

            // Actualizamos la nueva "posición inicial" para la siguiente comprobación
            currentStartPosition = currentBackground.position;
        }
    }
}