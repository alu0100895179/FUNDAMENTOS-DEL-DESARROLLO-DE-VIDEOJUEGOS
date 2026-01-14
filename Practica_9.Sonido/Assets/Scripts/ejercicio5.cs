using UnityEngine;

public class ejercicio5 : MonoBehaviour
{
    private AudioSource mySound;
    
    // Variable pública para asignar el sonido de choque desde el Inspector.
    // Es útil para usar PlayOneShot y que el sonido no se corte si hay muchos choques seguidos.
    public AudioClip collisionClip; 

    void Start()
    {
        // Obtenemos la referencia al AudioSource de este mismo objeto
        mySound = GetComponent<AudioSource>();
    }

    // Haremos la coprobación de colisión con la opción 'IsTrigger'
    void OnTriggerEnter(Collider other)
    {
        if (collisionClip != null)
        {
            mySound.PlayOneShot(collisionClip);
        }
        else
        {
            // Si no asignamos variable, reproduce el que tenga el AudioSource por defecto
            mySound.Play();
        }

        Debug.Log("¡Colisión detectada (Trigger) con: " + other.gameObject.name + "!");
    }
}