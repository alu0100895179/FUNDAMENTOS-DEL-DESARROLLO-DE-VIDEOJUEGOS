using UnityEngine;

// [RequireComponent] nos asegura que los otros dos scripts estén en el mismo GameObject.
[RequireComponent(typeof(CarruselManagerColor))]
[RequireComponent(typeof(CarruselManagerSize))]
public class CarruselManager : MonoBehaviour
{

  // Declaramos como privadas las clases de tamaño y color para la funcionalidad solicitada
  private CarruselManagerColor carruselColor;
  private CarruselManagerSize carruselSize;

  // Función de inicio del juego
  void Start(){
    // Se obtienen las referencias a los otros dos scripts.
    carruselColor = GetComponent<CarruselManagerColor>();
    carruselSize = GetComponent<CarruselManagerSize>();
  }

  // Update en cada frame del juego
  void Update()
  {
    // Si se pulsa la tecla espacio llamamos al método que cambia el color de los cubos.
    if (Input.GetKeyDown(KeyCode.Space))
    {
        Debug.Log("Tecla Space pulsada. Allá va carrusel de colores!!!");
        StartCoroutine(carruselColor.CambiarColor());
    }

    // Si se pulsa la tecla K se llama al método que cambia el tamaño de los cubos.
    if (Input.GetKeyDown(KeyCode.K))
    {
        Debug.Log("Tecla K pulsada. Allá va carrusel de tamaños!!!");
        StartCoroutine(carruselSize.CambiarTamano());
    }
  }
}
