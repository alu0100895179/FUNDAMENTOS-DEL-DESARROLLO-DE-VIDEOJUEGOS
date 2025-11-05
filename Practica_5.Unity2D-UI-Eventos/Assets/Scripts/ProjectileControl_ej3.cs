using UnityEngine;

public class ProjectileControl_ej3 : MonoBehaviour
{
    private SpriteRenderer sr;  // Renderer de mi proyectil
    private Animator anim;      // Referencia al componente Animator

    void Start()
    {
        // Obtenemos los componentes al iniciar
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        // Ocultamos el proyectil al empezar
        sr.enabled = false;
    }

    // Renombro la función para que sea más clara
    public void Shoot()
    {
        // 1. Hacemos visible el proyectil
        sr.enabled = true;

        // 2. Activamos el trigger "Dispara" en el Animator
        anim.SetTrigger("shoot");
    }

    public void HideAfterAnimation()
    {
        sr.enabled = false; // Ocultar de nuevo al finalizar la animación
        Debug.Log("Se llama a función: HideAfterAnimation");
    }   

}
