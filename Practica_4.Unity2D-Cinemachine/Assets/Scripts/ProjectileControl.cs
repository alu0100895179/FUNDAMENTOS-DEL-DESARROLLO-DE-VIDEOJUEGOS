using UnityEngine;

public class ProjectileControl : MonoBehaviour
{
    private SpriteRenderer sr;  // Renderer de mi proyectil
    private Animator anim;      // Referencia al componente Animator

    void Awake()
    {
        // Obtenemos los componentes al iniciar
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        if (anim == null)
        {
            Debug.LogError("¡ERROR GRAVE! El prefab del proyectil no tiene un componente Animator.", gameObject);
        }

        // Ocultamos el proyectil al empezar
        sr.enabled = false;
    }

    // Renombro la función para que sea más clara
    public void ShootProjectile()
    {
        // 1. Hacemos visible el proyectil
        sr.enabled = true;

        // 2. Activamos el trigger "Dispara" en el Animator
        anim.SetTrigger("shoot");
    }

    public void DestroyProjectile()
    {
        // Destruye el GameObject al que este script está asociado.
        Destroy(gameObject);
    }
}