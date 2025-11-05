using UnityEngine;
using UnityEngine.InputSystem; // Nuevo sistema de entrada

public class PlayerMovement : MonoBehaviour
{
    // Variables ajustables en el Inspector y privadas gracias a la cláusula seralizada
    [Header("Velocidades")]
    [SerializeField] private float normalSpeed = 5f;
    [SerializeField] private float turboSpeed = 10f;

    // Esta será la velocidad que se usa en cada momento. Ya no necesita ser pública.
    private float speed;

    // El resto de tus variables se mantienen igual
    private float h = 0f;
    private float v = 0f;

    // Declarar la variable que representará al SpriteRenderer para controlar el 'flip'
    private SpriteRenderer sprite;

    // Variable para guardar la escala original para el flip del personaje y sus hijos
    private Vector3 originalScale;

    void Start()
    {
        // Al empezar, nos aseguramos de que la velocidad sea la normal
        speed = normalSpeed;

        // Obtener la referencia al SpriteRenderer una sola vez (eficiencia)
        sprite = GetComponent<SpriteRenderer>();

        // Al empezar, guardamos la escala original del personaje
        originalScale = transform.localScale;
    }

    void Update()
    {
        h = 0f;
        v = 0f;
        // 1. Leer entradas de teclado (nuevo Input System)
        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
            h = -1f;
        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
            h = 1f;
        if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed)
            v = 1f;
        if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed)
            v = -1f;

        /* 
        / 2. Calcular la traslación espacio = velocidad * tiempo.
        / La velocidad se ha declarado previamente, el tiempo lo da Time.deltaTime
         La traslación se puede hacer con el método Translate del Transform del objeto
        / Mover al personaje (espacio global)
        */
        // He añadido .normalized al cálculo del movimiento. Esto evita el movimiento sea más rápido en diagonal
        Vector3 movimiento = new Vector3(h, v, 0f).normalized * speed * Time.deltaTime;
        transform.Translate(movimiento, Space.World);

        /* Sistema anterior del giro del sprite (se cambia para poder girar los proyectiles)
        if (sprite != null)
        {
            if (h > 0.01f) sprite.flipX = false;
            else if (h < -0.01f) sprite.flipX = true;
        }
        */

        // Obtenemos todos los componentes SpriteRenderer en este objeto y en todos sus hijos para poderlos girar
        SpriteRenderer[] renderers = GetComponentsInChildren<SpriteRenderer>();

        if (h > 0.01f)
        {
            // Mirar a la derecha: restaura la escala original
            transform.localScale = originalScale;
        }
        else if (h < -0.01f)
        {
            // Mirar a la izquierda: invierte la escala en X
            transform.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);
        }
    }

    // --- FUNCIÓN PÚBLICA PARA LA UI ---
    // Esta función será llamada por el Toggle.
    // Recibe 'true' si el Toggle está activado, y 'false' si está desactivado
    public void SetTurboMode(bool isTurboOn)
    {
        if (isTurboOn)
        {
            speed = turboSpeed;
            Debug.Log("Modo Turbo ACTIVADO");
        }
        else
        {
            speed = normalSpeed;
            Debug.Log("Modo Turbo DESACTIVADO");
        }
    }
}