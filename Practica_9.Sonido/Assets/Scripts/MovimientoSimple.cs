using UnityEngine;
using UnityEngine.InputSystem;

public class MovimientoSimple: MonoBehaviour
{
    public float velocidad = 10f;

    void Update()
    {
        if (Keyboard.current == null)
            return;

        float x = 0;
        float z = 0;

        if (Keyboard.current.leftArrowKey.isPressed)  x = -1;
        if (Keyboard.current.rightArrowKey.isPressed) x =  1;
        
        if (Keyboard.current.downArrowKey.isPressed)  z = -1;
        if (Keyboard.current.upArrowKey.isPressed)    z =  1;

        Vector3 movimiento = new Vector3(x, 0, z);
        transform.Translate(movimiento * velocidad * Time.deltaTime);
    }
}