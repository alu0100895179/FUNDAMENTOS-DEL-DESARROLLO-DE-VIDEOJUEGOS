using UnityEngine;

public class ProjectileControl_ej1 : MonoBehaviour
{
    private SpriteRenderer sr;  // Renderer de mi proyectil

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.enabled = false;     // Ocultar al iniciar
    }

    public void MostrarProyectil()
    {   
        sr.enabled = true;      // Mostrar al pulsar el botón
    }
}