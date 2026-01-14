using UnityEngine;
using UnityEngine.InputSystem; // New Input System

public class ejercicio4 : MonoBehaviour
{
    public float velocidad = 5f;
    private AudioSource mySound;
    private bool moving = false;
    private int direction = 1;

    void Start()
    {
        // Obtenemos la referencia al AudioSource
        mySound = GetComponent<AudioSource>(); 
    }

    void Update()
    {
        if (Keyboard.current == null) return;
        
        // Usamos 'wasPressedThisFrame' para detectar solo el instante de la pulsación
        // Al pulsar 'P' -> Activar movimiento y sonido
        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            moving = true;
            
            // Si no está sonando ya, le damos play y nos aseguramos que esté en bucle
            if (!mySound.isPlaying)
            {
                mySound.loop = true;
                mySound.Play();
            }
        }

        // Al pulsar 'S' -> Parar, silenciar y cambiar dirección
        if (Keyboard.current.sKey.wasPressedThisFrame)
        {
            moving = false;
            mySound.Stop();
            mySound.loop = false;
            
            direction *= -1; 
            Debug.Log("Dirección cambiada. Ahora es: " + direction);
        }

        // Lógica de movimiento constante
        if (moving)
        {
            // Se moverá hacia adelante o atrás dependiendo de la variable 'direction'
            transform.Translate(Vector3.forward * velocidad * Time.deltaTime * direction);
        }
    }
}