using UnityEngine;

public class ejercicio6 : MonoBehaviour
{
    // Variable pública para asignar sonido desde el inspector
    public AudioClip collisionClip;
    private AudioSource mySound;
    
    // A esta velocidad sonará a con el volumen a su máximo. Si va a 5, sonará a la mitad.
    public float maxSpeedVolume = 10f;

    // Variables para calcular la velocidad manual
    private Vector3 lastPos;
    private float actualSpeed;

    void Start()
    {
        mySound = GetComponent<AudioSource>();
        // Obtenemos el Rigidbody del propio cubo o player
        lastPos = transform.position;
    }

    // Usamos FixedUpdate para calcular físicas/velocidad de forma constante
    void FixedUpdate()
    {
        // Distancia que se ha movido desde el último frame
        float movSpace = Vector3.Distance(transform.position, lastPos);

        // Velocidad = Espacio / Tiempo
        actualSpeed = movSpace / Time.fixedDeltaTime;

        // Actualizamos la posición para el siguiente cálculo
        lastPos = transform.position;
    }


    void OnTriggerEnter(Collider other)
    {
        // Calculamos el volumen en funciuón de la velocidad con la fórmula: Velocidad / maxVolume
        // Mathf.Clamp01 asegura que el volumen nunca sea menor que 0 ni mayor que 1
        float dynamicVolume = Mathf.Clamp01(actualSpeed / maxSpeedVolume);

        if (collisionClip != null)
        {
            // Pasamos el volumen como segundo argumento a PlayOneShot
            mySound.PlayOneShot(collisionClip, dynamicVolume);
        }
        else
        {
            // Si usamos Play() normal, ajustamos el volumen del AudioSource antes de sonar
            mySound.volume = dynamicVolume;
            mySound.Play();
        }

        Debug.Log($"Choque a velocidad: {actualSpeed} -> Volumen aplicado: {dynamicVolume}");
    }
}