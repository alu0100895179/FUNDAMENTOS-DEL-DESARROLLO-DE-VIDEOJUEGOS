using UnityEngine;

public class movement6 : MonoBehaviour
{
    // Variable pública para manipular en el Inspector
    public Transform goal; 
    public float speed = 2.0f;
    void Start()
    {}

    void Update()
    {
        if (goal == null) return;
        /* A ------------------------------------------------------------- 
        // Orientamos el personaje hacia el objetivo:
        this.transform.LookAt(goal.position);

        // Nos movemos en su eje "Forward" (Z local)
        // Al no especificar 'relativeTo', por defecto es Space.Self
        this.transform.Translate(Vector3.forward * speed * Time.deltaTime);
        ------------------------------------------------------------------- */
        
        /* B --------------------------------------------------------------
        // Orientamos el personaje
        this.transform.LookAt(goal.position);

        // Calculamos el vector dirección (Global)
        Vector3 direction = goal.position - this.transform.position;

        // Aplicamos el movimiento sin especificar espacio (Por defecto: Space.Self)
        this.transform.Translate(direction.normalized * speed * Time.deltaTime);
        ------------------------------------------------------------------- */
        
        // Orientamos el personaje
        this.transform.LookAt(goal.position);

        // Calculamos el vector dirección (Global)
        Vector3 direction = goal.position - this.transform.position;

        // Movemos respecto al MUNDO
        this.transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);
    }
}