using UnityEngine;
using UnityEngine.InputSystem; // New Input System

public class MovimientoDoppler : MonoBehaviour
{
    public float speed = 10f;

    void Update()
    {
        // Verificamos si existe un teclado conectado y si la tecla M está presionada
        if (Keyboard.current != null && Keyboard.current.mKey.isPressed)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
        else if (Keyboard.current != null && Keyboard.current.nKey.isPressed)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime * -1);
        }
    }
}
