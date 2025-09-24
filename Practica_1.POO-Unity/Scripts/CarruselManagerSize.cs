using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CarruselManagerSize : MonoBehaviour
{
  // Array de cubos que se van a redimensionar.
  // Asignaremos los cubos a este array desde el Inspector.
  public GameObject[] cubos_t;

  // El nuevo tamaño que se aplicará a todos los cubos.
  // También podemos cambiar este valor en el Inspector.
  public Vector3 nuevoTamano = new Vector3(2f, 2f, 2f);

  // Tiempo de espera entre iteraciones.
  public float tiempoDeEsperaT = 1.0f;
  // Función de inicio del juego
  void Start(){}
  // Update en cada frame del juego
  void Update(){}

  // Función de cambio de color
  public IEnumerator CambiarTamano()
  {
    // Si no hay cubos asignados, saldríamos del método.
    if (cubos_t != null)
    {
      // Recorremos el array de cubos
      for (int i = 0; i < cubos_t.Length; i++)
      {
        // Verificamos que el cubo exista.
        if (cubos_t[i] != null)
        {
          // Accedemos al componente Transform y cambiamos su scale.
          // Usamos 'localScale' para cambiar el tamaño relativo al padre.
          Debug.Log("Cambiando tamaño del cubo: "+i+".");
          cubos_t[i].transform.localScale = nuevoTamano;
          // Esperamos el tiempo señalado en tiempoDeEsperaT para dar sensación de carrusel
          yield return new WaitForSeconds(tiempoDeEsperaT);
        }
      }
    }
    // Salida del método indicando error
    else
    {
      Debug.LogError("Array de cubos no asignado en CarruselManagerSize.");
      yield break;
    }
  }
}
