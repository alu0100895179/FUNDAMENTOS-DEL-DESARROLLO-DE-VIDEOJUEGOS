using UnityEngine;
using System.Collections.Generic;

// Almacén que guarda los objetos desactivados.
public class ObjectPooler : MonoBehaviour
{
    [Header("Configuración del Pool")]
    [SerializeField] private GameObject objectToPool;   // El prefab
    [SerializeField] private int amountToPool = 10;     // Cantidad máxima de objetos

    // La lista que guardará nuestros objetos
    private List<GameObject> pooledObjects;

    void Awake()
    {
        pooledObjects = new List<GameObject>();

        // Bucle para crear todos los objetos al inicio del juego
        for (int i = 0; i < amountToPool; i++)
        {
            // Crear
            GameObject obj = Instantiate(objectToPool);
            
            // Desactivar (Guardar en el almacén)
            obj.SetActive(false);
            
            // Añadir a la lista
            pooledObjects.Add(obj);
            
            // Organizarlos como hijos de este objeto para no ensuciar la jerarquía
            obj.transform.SetParent(transform); 
        }
    }

    // Método para pedir un objeto prestado del almacén
    public GameObject GetPooledObject()
    {
        // Recorremos la lista buscando uno que esté desactivado
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];  // Retornamos el que está libre
            }
        }
        // Si llegamos aquí, es que todos están en uso. Devolvemos null.
        return null;
    }
}