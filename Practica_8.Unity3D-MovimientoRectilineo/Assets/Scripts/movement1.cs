using UnityEngine;

public class movement1 : MonoBehaviour
{
    // Variable pública para manipular en el Inspector (X, Y, Z)
    // A: public Vector3 goal = new Vector3(0, 0, 0.1f);
    // B: public Vector3 goal = new Vector3(0, 0, 0.1f);
    // C: public Vector3 goal = new Vector3(0, 0.1f, 0.1f); 
    // D: public Vector3 goal = new Vector3(0, 0, 0.1f); 

    public Vector3 goal = new Vector3(0.01f, 0.01f, 0.01f); 
    void Start()
    {
        // OPCIÓN ORIGINAL: Moverse solo una vez (Teletransporte)
        // this.transform.Translate(goal);
        // A:
        // goal = goal * 0.5f;
    }

    void Update()
    {
        // A: Moverse continuamente
        // Al ejecutarse en cada frame, el vector 'goal' se suma continuamente a la posición.
        // this.transform.Translate(goal);

        // E: Versión estable
        // Duplicamos los valores X,Y,Z (que es lo mismo que multipicar goal*2)
        this.transform.Translate(goal * 2.0f);
        // E: Versión exponiencial
        // Se duplica en cada frame la velocidad
        // goal = goal * 2;
        // this.transform.Translate(goal);
    }
}