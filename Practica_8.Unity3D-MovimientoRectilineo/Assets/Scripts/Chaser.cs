using UnityEngine;
using UnityEngine.InputSystem;

public class Chaser : MonoBehaviour
{
    public Transform goal; 
    // B: Campo público para ajustar velocidad en el Inspector
    [Range(1f, 20f)] // Añade un slider en el editor
    public float speed = 3.0f;

    // C: Cuánto aumenta la velocidad (por defecto x2)
    public float turboMultiplier = 2.0f; 

    // Para transicionar si tiene animaciones
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // Si existe el objeto meta no hacemos nada
        if (goal == null) return;

        // Mirar al objetivo
        transform.LookAt(goal.position);

        // Recalcular dirección hacia la nueva posición del cubo
        Vector3 direction = goal.position - this.transform.position;
        Debug.DrawRay(transform.position, direction, Color.red);
        anim.SetFloat("Speed", direction.magnitude);

        // C: Lógica de la velocidad mediante tecla "espacio"
        float currentSpeed = speed;
        if (Keyboard.current != null && Keyboard.current.spaceKey.isPressed)
        {
            // Aplicamos el multiplicador temporalmente
            currentSpeed = speed * turboMultiplier;
            Debug.Log("Tecla ESPACIO pulsada: ¡APLICANDO TURBO!");
        }

        // Moverse hacia él (Space.World para tener en cuenta coodenadas del mundo)
        transform.Translate(direction.normalized * currentSpeed * Time.deltaTime, Space.World);
    }
}
