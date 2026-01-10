using UnityEngine;

public class movement7 : MonoBehaviour
{
    // Variable pública para manipular en el Inspector
    public Transform goal; 
    public float speed = 2.0f;
    public float accuracy = 0.5f; // Umbral para la detención
    
    void Start()
    {}

    void Update()
    {
        /* A: ------------------------------------------------------------------------------------------------------------
        // Calcular el vector de dirección hacia el objetivo
        Vector3 direction = goal.position - this.transform.position;
        
        // Si la longitud del vector (distancia restante) es mayor que el umbral definido realizaremos la parada
        if (direction.magnitude > accuracy)
        {
            // Orientar hacia el objetivo
            this.transform.LookAt(goal.position);
            
            // Realizar el movimiento ya que seguimos lo suficientemente "lejos"
            this.transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);
        }
        -----------------------------------------------------------------------------------------------------------------*/

        // B:
        // Calcula la distancia absoluta entre los puntos
        if (Vector3.Distance(this.transform.position, goal.position) > accuracy)
        {
            // Recalculamos direction aquí dentro para movernos
            Vector3 direction = goal.position - this.transform.position;
            this.transform.LookAt(goal.position);
            this.transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);
        }
    }
}
