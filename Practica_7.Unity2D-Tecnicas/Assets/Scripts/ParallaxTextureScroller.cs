using UnityEngine;

// Efecto Parallax modificando el offset y siguiendo a la cámara
public class ParallaxTextureScroller : MonoBehaviour
{
    [SerializeField] private float parallaxSensitivity = 0.05f;
    [SerializeField] private Transform cameraTransform;

    private Renderer rend;
    private float lastCameraX;

    void Start()
    {
        rend = GetComponent<Renderer>();
        if (cameraTransform == null) 
            cameraTransform = Camera.main.transform;
        lastCameraX = cameraTransform.position.x;
    }

    void LateUpdate()
    {
        // --- Mover el Quad para que siga a la cámara ---
        // Mantenemos la Z e Y original del Quad, pero copiamos X de la cámara
        transform.position = new Vector3(cameraTransform.position.x, transform.position.y, transform.position.z);

        // --- Calcular Parallax ---
        float deltaX = cameraTransform.position.x - lastCameraX;

        for (int i = 0; i < rend.materials.Length; i++)
        {
            Material m = rend.materials[i];
            m.mainTextureOffset += new Vector2(deltaX * (parallaxSensitivity / (i + 1.0f)), 0);
        }

        lastCameraX = cameraTransform.position.x;
    }
}