using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CarruselManagerColor : MonoBehaviour
{
  // Array de GameObjects (cubos) que vamos a colorear.
  // Asigna los cubos a este array desde el Inspector de Unity.
  public GameObject[] cubos_c;

  // Array de colores que se asignarán a los cubos.
  // Define los colores en el Inspector. La clase Color de Unity usa
  // valores RGBA de 0 a 1.
  public Color[] colores;

  // Tiempo de espera entre iteraciones
  public float tiempoDeEsperaC = 1.0f;

  // Tamaño mínimo de ambos arrays
  public int tamano = 5;

    // Función de inicio del juego
    void Start(){}
    // Update en cada frame del juego
    void Update(){}

    public IEnumerator CambiarColor()
    {
      // Si los arrays no tienen el mismo tamaño, usamos el más pequeño
      // para evitar un 'IndexOutOfRangeException'.
      tamano = Mathf.Min(cubos_c.Length, colores.Length);
      // Si no hay cubos o colores asignados, salimos del método para evitar errores.
      if (cubos_c != null && colores != null)
      {
        // Recorremos el array de cubos y asignamos un color a cada uno.
        for (int i = 0; i < tamano; i++)
        {
          Debug.Log("Cambiando color del cubo: "+i+".");
          // Obtenemos el componente Renderer del cubo.
          Renderer renderer = cubos_c[i].GetComponent<Renderer>();
          // Asignamos el color del array al material del cubo.
          renderer.material.color = colores[i];
          // Esperamos el tiempo señalado en tiempoDeEsperaC para dar sensación de carrusel
          yield return new WaitForSeconds(tiempoDeEsperaC);
        }
      }
      // Salida del método indicando error
      else{
        Debug.LogError("Arrays de cubos o colores no asignados en CarruselManagerColor.");
        yield break;
      }
    }
}
