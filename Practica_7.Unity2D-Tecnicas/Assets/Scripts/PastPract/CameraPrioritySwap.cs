using UnityEngine;
using Unity.Cinemachine; // Usamos la librería de Cinemachine para acceder a propiedades que nos interesan de la cámara

public class CameraPrioritySwap : MonoBehaviour
{
    public CinemachineCamera confineCamera;
    public CinemachineCamera freeCamera;

    [Tooltip("Tag del objeto que puede activar este script")]
    public string targetTag = "Player";

    // Comprobación de choque físico
    private void OnCollisionEnter2D(Collision2D other)
    {
        // Comprobamos si el objeto que ha chocado tiene el Tag
        if (other.gameObject.CompareTag(targetTag))
        {
            //  Invertimos las prioridades entre las cámaras
            // Guardamos el valor de la cámara A
            int tempPriority = confineCamera.Priority;

            // Damos a la cámara A el valor de la B
            confineCamera.Priority = freeCamera.Priority;

            // Damos a la cámara B el valor guardado de la A
            freeCamera.Priority = tempPriority;
        }
    }
}
