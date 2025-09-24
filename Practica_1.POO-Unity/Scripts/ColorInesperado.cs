using UnityEngine;

public class ColorInesperado : MonoBehaviour
{
    private Renderer rend;
    public Color colorEsperado;

    void Start()
    {
        rend = GetComponent<Renderer>();
        // Asignamos un valor incorrecto a la variable
        colorEsperado = Color.red;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Depurar el color que se está mostrando
            /* El color que se asignará es: RGBA(1.000, 0.000, 0.000, 1.000)
            UnityEngine.Debug:Log (object)
            ColorInesperado:Update () (at Assets/Scripts/ColorInesperado.cs:19) */
            Debug.Log("El color que se asignará es: " + colorEsperado);
            // Intentamos cambiar el color del material al valor de la variable
            rend.material.color = colorEsperado;
        }
    }
}
