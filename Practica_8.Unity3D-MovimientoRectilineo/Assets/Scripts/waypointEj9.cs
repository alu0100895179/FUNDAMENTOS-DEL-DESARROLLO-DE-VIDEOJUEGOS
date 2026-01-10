using UnityEngine;

public class waypointEj9: MonoBehaviour
{
    // Variables públicas para manipular en el Inspector
    public float speed = 4.0f;      // Velocidad para el movimiento 
    public float rotSpeed = 2.0f;   // Velocidad para el giro
    public float accuracy = 0.5f;   // Umbral para la detención

    private GameObject[] waypoints; // Array para almacenar las referencias a los WP
    private int currentWP = 0;      // Índice del waypoint actual

    void Start()
    {
        // Recuperamos los objetos de la escena que tengan la etiqueta "waypoint"
        waypoints = GameObject.FindGameObjectsWithTag("waypoint");

        if (waypoints.Length == 0)
            Debug.LogError("No se han encontrado objetos con la etiqueta 'waypoint'.");
    }

    void Update()
    {
        if (waypoints.Length == 0) return;

        // Identificar el objetivo actual en base al índice
        Vector3 destination = waypoints[currentWP].transform.position;
        Vector3 direction = destination - this.transform.position;

        // Si la longitud del vector (distancia restante) es mayor que el umbral definido realizaremos la parada
        if (direction.magnitude > accuracy)
        {
            // Calcular LA ROTACIÓN necesaria para mirar en esa dirección
            // LookRotation convierte un Vector3 (dirección) en un Quaternion (rotación)
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // INTERPOLAR (SLERP): Vamos desde mi rotación actual (transform.rotation) hacia la rotación deseada (targetRotation)
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotation, rotSpeed * Time.deltaTime);

            // Desplazamiento mediante Forward local
            this.transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
        else
        {
            currentWP++; // Pasamos al siguiente waypoint

            // Comprobamos si hemos llegado al final del array para volver al principio
            if (currentWP >= waypoints.Length)
            {
                currentWP = 0;
            }
        }
    }
}
