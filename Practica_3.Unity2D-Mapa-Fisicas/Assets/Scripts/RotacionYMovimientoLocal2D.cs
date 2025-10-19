using UnityEngine;
using UnityEngine.InputSystem;

public class RotacionYMovimientoLocal2D : MonoBehaviour
{
    
    // Velocidad de traslación hacia adelante (unidades/segundo)
    // Variables ajustables en el Inspector y privadas gracias a la cláusula seralizada
    [SerializeField] private float speed = 5f;

    // Velocidad de rotación (grados por segundo)
    [SerializeField] private float rotationSpeed = 180f;

    void Update()
    {
        // 1. OBTENER LAS ENTRADAS
        // Input para la rotación (izquierda/derecha) con el "eje Horizontal"
        float rotationInput = 0f;
        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
        {
            rotationInput = 1f; // girar a la izquierda (contrario a las agujas del reloj)
        }
        else if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
        {
            rotationInput = -1f; // girar a la derecha (agujas del reloj)
        }

        // Input para el movimiento hacia adelante/atrás con el "eje Vertical"
        float forwardInput = 0f;
        if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed)
        {
            forwardInput = 1f; // avanzar hacia adelante (en la dirección que mira el objeto)
        }
        else if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed)
        {
            forwardInput = -1f; // retroceder
        }

        // 2. APLICAR ROTACIÓN (Giro del Objeto sobre el Eje Z)
        // Multiplicamos por Time.deltaTime para suavizar el giro y que sea por grados/segundo
        float rotationAmount = rotationInput * rotationSpeed * Time.deltaTime;
        // Rotamos en Z porque estamos en 2D (transform.Rotate usa grados)
        transform.Rotate(0f, 0f, rotationAmount);

        // 3. APLICAR TRASLACIÓN EN EL ESPACIO LOCAL (Avanzar), una vez que hemos rotado.
        // Creamos un vector que solo tiene movimiento en el EJE Y (local), porque el objeto ya está rotado.
        Vector3 localMovement = new Vector3(0f, forwardInput * speed * Time.deltaTime, 0f);
        // Space.Self para mover en la dirección que mira el objeto (su espacio local)
        transform.Translate(localMovement, Space.Self);
    }
}
