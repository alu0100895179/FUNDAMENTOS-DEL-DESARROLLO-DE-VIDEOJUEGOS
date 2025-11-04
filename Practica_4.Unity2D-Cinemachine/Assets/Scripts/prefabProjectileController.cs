using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;                  // Línea para leer el teclado con InputSystem nuevo

public class PrefabProjectileController : MonoBehaviour
{
    // Información y parámetros configurables desde el inspector para probar
    [Header("Configuración de Disparo")]
    [Tooltip("Distancia desde el centro del monstruo donde aparecerá el proyectil.")]
    public float projectileOffset = 1.7f;             
    [Tooltip("Segundos que el proyectil hijo del controlador permanecerá con vida.")]
    public float projectileLifetime = 4.0f;         // Podemos dejarlo amplio porque se vuelve invisible antes de destruirlo

    private GameObject _projectilePrefab;

    // Se suscribe al evento cuando el objeto se activa.
    private void OnEnable()
    {
        // --- NOMBRE CORREGIDO AQUÍ ---
        // Ahora escucha a la clase correcta: eventCollsionController
        eventCollsionController.OnPowerUpCollected += Shoot;
    }

    // Se desuscribe cuando el objeto se desactiva.
    private void OnDisable()
    {
        // --- Y TAMBIÉN AQUÍ ---
        eventCollsionController.OnPowerUpCollected -= Shoot;
    }

    void Awake()
    {
        _projectilePrefab = Resources.Load<GameObject>("projectileFire");

        if (_projectilePrefab == null)
            Debug.LogError("No se pudo encontrar el prefab 'projectileFire' en la carpeta Resources.");
    }

    // Escuchamos la pulsación de la tecla 'F' para disparar
    void Update()
    {
        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            Debug.Log("Disparo del proyectil mediante pulsación letra 'F'");
            Shoot();
        }
    }

    public void Shoot()
    {
        if (_projectilePrefab == null) return;

        // 1. INSTANCIAMOS EL PROYECTIL Y LO HACEMOS HIJO DIRECTAMENTE
        // Al pasar 'transform' como segundo parámetro, el nuevo objeto se crea como hijo de este
        GameObject projectileInstance = Instantiate(_projectilePrefab, transform);

        // 2. LE DAMOS UNA POSICIÓN LOCAL (relativa al padre)
        // Lo movemos a la derecha del padre. Si el padre está volteado (con scale.x = -1)
        // esta posición local lo pondrá automáticamente a su izquierda en el mundo.
        projectileInstance.transform.localPosition = new Vector3(projectileOffset, 0, 0);
        
        // 3. PROGRAMAMOS SU DESTRUCCIÓN
        Destroy(projectileInstance, projectileLifetime);
        
        // 4. LE DAMOS LA ORDEN DE ACTIVAR LA ANIMACIÓN
        ProjectileControl projectileScript = projectileInstance.GetComponent<ProjectileControl>();
        if (projectileScript != null)
        {
            projectileScript.ShootProjectile();
        }
    }
}

