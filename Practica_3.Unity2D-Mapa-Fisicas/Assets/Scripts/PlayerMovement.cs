using UnityEngine;
using UnityEngine.InputSystem; // Nuevo sistema de entrada

public class PlayerMovement : MonoBehaviour
{
    // Declarar la velocidad de tipo float
    // Variables ajustables en el Inspector y privadas gracias a la cláusula seralizada
    [SerializeField] private float speed = 5f;  // Velocidad ajustable desde el Inspector
    [SerializeField] private float h = 0f;      // Movimiento horizontal (eje x)
    [SerializeField] private float v = 0f;      // Movimiento vertica (eje y)

    // Declarar la variable que representará al SpriteRenderer para controlar el 'flip'
    private SpriteRenderer sprite;

    void Start()
    {
        // Obtener la referencia al SpriteRenderer una sola vez (eficiencia)
        sprite = GetComponent<SpriteRenderer>();
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
        Vector3 movimiento = new Vector3(h, v, 0f) * speed * Time.deltaTime;
        transform.Translate(movimiento, Space.World);

        // Flip del sprite al girar
        if (sprite != null)
        {
            if (h > 0.01f) sprite.flipX = false;
            else if (h < -0.01f) sprite.flipX = true;
        }
    }
}