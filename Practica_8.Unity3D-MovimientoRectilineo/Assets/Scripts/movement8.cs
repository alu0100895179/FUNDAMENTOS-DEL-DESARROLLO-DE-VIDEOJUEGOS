using UnityEngine;

public class movement8 : MonoBehaviour
{
    // Variables públicas para manipular en el Inspector
    public Transform goal;          // Transform del objetivo 
    public float speed = 4.0f;      // Velocidad para el movimiento 
    public float rotSpeed = 2.0f;   // Velocidad para el giro
    public float accuracy = 0.5f;   // Umbral para la detención

    // Para transicionar si tiene animaciones
    private Animator anim;

        void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // Calcular el vector de dirección hacia el objetivo
        Vector3 direction = goal.position - this.transform.position;
        
        // Para controlar las animaciones
        anim.SetFloat("Speed", direction.magnitude);
        
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
    }
}