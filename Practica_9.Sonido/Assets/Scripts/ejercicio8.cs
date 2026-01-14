using UnityEngine;

public class ejercicio8 : MonoBehaviour
{
    private AudioSource mySound;
    private Vector3 lastPosition;

    // Para comprobar si tocamos el suelo
    public float groundDistance = 0.5f;

    // Variables para controlar el ritmo de los pasos
    // Cada cuánto tiempo suena un paso (0.5 = 2 pasos por segundo)
    public float stepRate = 0.5f; 
    private float nextStepTime = 0f;

    void Start()
    {
        // Obtenemos la referencia al AudioSource
        mySound = GetComponent<AudioSource>();
        
        // Inicializamos la primera posición posición como la el primer valor de "last"
        lastPosition = transform.position;
    }

    void Update()
    {
        // Comprobamos si la distancia recorrida desde el último frame es relevante
        bool isMoving = Vector3.Distance(transform.position, lastPosition) > 0.01f;

        // Lanzamos un rayo hacia abajo para ver si tocamos suelo
        bool isGrounded = Physics.Raycast(transform.position, Vector3.down, groundDistance);

        // Lógica de 'steps'
        // Solo entramos si nos movemos, tocamos suelo y ha pasado el tiempo suficiente
        if (isMoving && isGrounded && Time.time >= nextStepTime)
        {
            mySound.PlayOneShot(mySound.clip);

            // Reseteamos el reloj: "No vuelvas a sonar hasta dentro de 'stepRate' segundos"
            nextStepTime = Time.time + stepRate;
        }

        // Actualizamos la posición para el siguiente frame
        lastPosition = transform.position;
    }
}