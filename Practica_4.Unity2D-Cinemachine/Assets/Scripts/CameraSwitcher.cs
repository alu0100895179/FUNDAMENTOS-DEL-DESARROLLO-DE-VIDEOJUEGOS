using UnityEngine;
using UnityEngine.InputSystem; // Nuevo sistema de entrada

public class CameraSwitcher : MonoBehaviour
{
    [Tooltip("Array de cámaras virtuales para el switch. El índice 0 es '1', el 1 es '2', etc.")]
    public GameObject[] camarasVirtuales;

    void Start()
    {
        // Al empezar, nos aseguramos de que solo la primera cámara (índice 0) esté activa
        SwitchToCamera(0);
    }

    void Update()
    {
        // --- Tarea: Método detectar teclas ---
        // Cada número corresponderá a su cámara por el índice del array
        // Usamos 'wasPressedThisFrame' para que solo se llame UNA VEZ por pulsación.
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            SwitchToCamera(0); // Llama al índice 0
            Debug.Log("Pulsado tecla: 1");
        }

        if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            SwitchToCamera(1); // Llama al índice 1
            Debug.Log("Pulsado tecla: 2");
        }

        if (Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            SwitchToCamera(2); // Llama al índice 2
            Debug.Log("Pulsado tecla: 3");
        }
    }

    // --- Tarea: Método único para la UI y teclado ---
    // Este método público acepta el "índice" de la cámara que queremos activar (0, 1, 2...)
    public void SwitchToCamera(int index)
    {
        // Comprobamos que el índice que pedimos exista en el array
        if (index < 0 || index >= camarasVirtuales.Length)
        {
            return; // Si no existe (ej. pedimos cámara 5 y solo hay 3), no hace nada
        }

        // Bucle que recorre todas las cámaras del array
        for (int i = 0; i < camarasVirtuales.Length; i++)
        {
            // Si la cámara en la posición 'i' existe
            if (camarasVirtuales[i] != null)
            {
                // Activa la cámara si su 'i' es igual al 'index' que hemos pedido.
                // La desactiva si 'i' no es igual al 'index'.
                camarasVirtuales[i].SetActive(i == index);
            }
        }
    }
}