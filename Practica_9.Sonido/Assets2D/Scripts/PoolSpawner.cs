using UnityEngine;

// Encargado de sacar los objetos del almacén y los pone en la escena.
public class PoolSpawner : MonoBehaviour
{
    [SerializeField] private ObjectPooler objectPooler;     // Referencia al almacén
    [SerializeField] private float spawnRate = 5f;          // Tiempo entre apariciones
    [SerializeField] private float xRange = 10f;             // Rango horizontal donde aparecerán

    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;

        // Cada 'spawnRate' segundos, intentamos sacar una poción
        if (timer >= spawnRate)
        {
            SpawnObject();
            timer = 0f;
        }
    }

    void SpawnObject()
    {
        // Pedimos un objeto al Pool
        GameObject obj = objectPooler.GetPooledObject();

        // Si el Pool nos ha dado uno (no devolvió null)
        if (obj != null)
        {
            
            // Sumamos la posición X actual del Spawner + el rango aleatorio
            float randomX = Random.Range(-xRange, xRange);
            Vector3 spawnPos = new Vector3(transform.position.x + randomX, transform.position.y, 0);
            
            // Lo colocamos en su sitio
            obj.transform.position = spawnPos;
            obj.transform.rotation = Quaternion.identity; // Rotación (0, 0, 0)
            
            // Importante encenderlo
            obj.SetActive(true);
        }
    }
}
