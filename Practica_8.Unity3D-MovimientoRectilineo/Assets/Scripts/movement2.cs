using UnityEngine;

public class movement2 : MonoBehaviour
{
    // Variable pública para manipular en el Inspector (X, Y, Z)
    public Vector3 goal = new Vector3(0.0f, 0.0f, 80.0f); 
    public float speed = 2.0f;
    void Start()
    {}

    void Update()
    {
        // A: Normalizar el vector de desplazamiento
        // this.transform.Translate(goal.normalized);

        // B: Control de la velocidad
        // this.transform.Translate(goal.normalized * speed);

        // C: Control de la velocidad y Frames por segundo
        this.transform.Translate(goal.normalized * speed * Time.deltaTime);
    }
}
