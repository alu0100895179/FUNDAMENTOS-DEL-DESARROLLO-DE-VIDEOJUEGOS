using UnityEngine;

public class AmbienceSoundZone : MonoBehaviour
{
    // Referencia al sonido propio del objeto
    private AudioSource localAmbience;
    
    // Si arrastro aquí el AudioSource del bosque, lo pausará al entrar
    // Si se deja vacío (None) no se pausará ningún sonido (ideal para que se mezclen)
    public AudioSource globalAmbience;

    void Start()
    {
        // Obtengo la referencia al componente de audio local
        localAmbience = GetComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Compruebo si el objeto que entra es el Player
        if (other.CompareTag("Player"))
        {
            // Reproduzco el sonido local si no está sonando ya
            if (!localAmbience.isPlaying)
            {
                localAmbience.Play();
            }

            // Pauso el ambiente global si se ha especificado un valor
            if (globalAmbience != null) 
            {
                globalAmbience.Pause();
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // Verifico si el jugador sale de la zona
        if (other.CompareTag("Player"))
        {
            // Detengo el sonido local
            localAmbience.Stop();

            // Reanudo el ambiente global desde donde se pausó si se había especificado previamente
            if (globalAmbience != null)
            {
                globalAmbience.UnPause(); 
            }
        }
    }
}