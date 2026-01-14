using UnityEngine;
using Unity.Cinemachine; // Usamos la librería de Cinemachine para acceder a propiedades que nos interesan de la cámara
using UnityEngine.InputSystem; // Nuevo sistema de entrada

public class ZoomController : MonoBehaviour
{
    [Tooltip("Cámara virtual del jugador que queremos controlar")]
    public CinemachineCamera vcam;

    [Tooltip("Velocidad a la que la cámara hará zoom")]
    public float zoomSpeed = 2f;

    [Tooltip("Valor mínimo de OrthographicSize (Máximo Zoom In)")]
    public float minZoom = 3f;

    [Tooltip("Valor máximo de OrthographicSize (Máximo Zoom Out)")]
    public float maxZoom = 10f;

    // Variable privada para guardar el tamaño de zoom deseado
    private float targetOrthographicSize;

    void Start()
    {
        // Valor de inicio de zoom, el actual de la cámara
        if (vcam != null)
        {
            targetOrthographicSize = vcam.Lens.OrthographicSize;
        }
    }

    void Update()
    {
        // Solo ejecutar si tenemos acceso a la cámara
        if (vcam != null)
        {
            // --- Detección de Teclas ---
            // Si pulsamos 'W', queremos hacer Zoom In (un valor más pequeño)
            if (Keyboard.current.wKey.isPressed)
                targetOrthographicSize -= zoomSpeed * Time.deltaTime;

            // Si pulsamos 'S', queremos hacer Zoom Out (un valor más grande)
            if (Keyboard.current.sKey.isPressed)
                targetOrthographicSize += zoomSpeed * Time.deltaTime;

            // --- Aplicar zoom teniendo en cuenta límites ---
            // Me apoyo en la función "Clamp" de la librería utilidades matemáticas "Mathf", para mantener el valor en el umbral.
            targetOrthographicSize = Mathf.Clamp(targetOrthographicSize, minZoom, maxZoom);
            // Aplicamos el nuevo tamaño a la lente de la cámara
            // Usamos Lerp para un efecto de zoom suave en lugar de un corte brusco 
            vcam.Lens.OrthographicSize = Mathf.Lerp(vcam.Lens.OrthographicSize, targetOrthographicSize, Time.deltaTime * zoomSpeed);
        } else return;
    }
}
