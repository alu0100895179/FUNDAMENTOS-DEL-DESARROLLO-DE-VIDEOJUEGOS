using UnityEngine;

public class CuboDeColores : MonoBehaviour
{
    public Color color = Color.white;
    public float tamano = 1f;
    public Vector3 posicion;
    Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
        rend.material.color = color;
        posicion = transform.position;
    }

    void Update()
    {
        // Aplicar cambio de color desde inspector mediante variable
        if (rend.material != null)
            rend.material.color = color;

        // Actualizar tamaño desde 'tamano'
        transform.localScale = Vector3.one * tamano;
        
        // Actualizar posicion desde inspector mediante variable
        transform.position = posicion;
    }
}

