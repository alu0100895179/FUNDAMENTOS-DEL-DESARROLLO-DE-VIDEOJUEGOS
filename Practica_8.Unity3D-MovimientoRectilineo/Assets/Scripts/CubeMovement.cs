using UnityEngine;
using UnityEngine.InputSystem;

public class CubeMovement : MonoBehaviour
{
    public float cubeSpeed = 5.0f;

    void Update()
    {
        
        // Inicializamos las variables de desplazamiento en 0
        float h = 0;
        float v = 0;

        // Verificamos si existe un teclado conectado
        if (Keyboard.current != null)
        {
            // Calculamos "Lateral" manualmente
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) 
                h += 1f;
            
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) 
                h -= 1f;


            // Calculamos "Forward" manualmente
            if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) 
                v += 1f;

            if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) 
                v -= 1f;

            // Movemos el cubo en sus ejes X y Z locales
            Vector3 movement = new Vector3(h, 0, v);
            this.transform.Translate(movement.normalized * cubeSpeed * Time.deltaTime);
        }
    }
}
