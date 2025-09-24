using UnityEngine;

public class ControladorMovimientoBasico : MonoBehaviour
{
    public float velocidad = 1f;
    Vector3 direccion = Vector3.zero;

    void Update()
    {
        // detecto si shift está presionado
        bool shift = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        // recalcular la nueva direccion
        direccion = Vector3.zero;

        if (Input.GetKey(KeyCode.UpArrow))
            direccion += shift ? Vector3.forward : Vector3.up;

        if (Input.GetKey(KeyCode.DownArrow))
            direccion += shift ? Vector3.back : Vector3.down;

        if (Input.GetKey(KeyCode.RightArrow))
            direccion += Vector3.right;

        if (Input.GetKey(KeyCode.LeftArrow))
            direccion += Vector3.left;

        // si hay alguna dirección, movemos con translate
        if (direccion != Vector3.zero)
        {
            // normalizamos para evitar aumento de velocidad en diagonales
            Vector3 movimiento = direccion.normalized * velocidad * Time.deltaTime;
            transform.Translate(movimiento, Space.World);
        }
    }
}
