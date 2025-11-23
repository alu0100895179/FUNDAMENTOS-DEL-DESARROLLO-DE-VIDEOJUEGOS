# FDV PRÁCTICA 7: Técnicas 2D (Fondos, Parallax y Pooling)

## Alumno
- Nombre: Jaime Madico Cañete
- ALU: alu0100895179
- Correo: alu0100895179@ull.edu.es

### Demostración de ejecución
![Practica Fondos Demo](Docs/prueba4-2.gif)

## Entorno
- Unity 6.2: 6000.2.7f2
- Plataforma: Windows
- Input System: Unity New Input System (UnityEngine.InputSystem)
- Motor de físicas: 2D Physics (Rigidbody2D, Collider2D)
- Tilemaps: Sistema de Unity (Grid, Tilemap, Tile Palette, Composite Collider 2D)
- Sistema de cámaras: Cinemachine (CinemachineBrain, CinemachineVirtualCamera, Confiner, TargetGroup, Impulse)
- Sistema UI: Unity UI (Canvas, Text / TMP)
- Control de versiones: Git (repositorio en GitHub)  
- Entrega: Repositorio + GIF demostrativo + .zip en Campus Virtual

## Resumen
En esta práctica se exploran técnicas fundamentales para la creación de entornos 2D dinámicos y optimizados, reutilizando el proyecto base de mecánicas. El objetivo es familiarizarse con diferentes métodos de *scrolling* (desplazamiento) de fondos, la creación de profundidad mediante el **efecto Parallax** y la optimización del rendimiento a través de la técnica de **Pooling** de objetos.

El **scrolling** de fondos es una técnica clave para simular mundos infinitos o entornos muy grandes sin necesidad de cargar un único objeto de gran tamaño. Esto se logra repitiendo texturas o `GameObjects` de fondo de forma cíclica.

El **efecto Parallax** añade una ilusión de profundidad 3D a una escena 2D, haciendo que las capas de fondo más lejanas se muevan más lentamente que las capas cercanas. Esto se consigue aplicando diferentes velocidades de *scrolling* a cada capa del fondo.

Finalmente, el **Pooling de Objetos** es una técnica de optimización esencial que reutiliza `GameObjects` (como proyectiles, monedas o enemigos) en lugar de destruirlos (`Destroy()`) e instanciarlos (`Instantiate()`) continuamente. Almacenar objetos en un "pool" reduce significativamente la carga en el procesador y el recolector de basura, previniendo "tirones" (*stutters*) en el juego.

Las mecánicas a implementar incluyen: *scrolling* de fondo con cámara fija, *scrolling* de fondo con cámara móvil (la técnica de "salto" o *leapfrogging*), *scrolling* por desplazamiento de textura (*offset*), efecto *parallax* usando tanto el método de *offset* como el de movimiento de *transforms*, y un sistema de *pooling* para objetos recolectables.

---

## Ficheros más importantes entregados

- PRACTICA.md
- Assets/
  - Scenes/
    - Fondos.unity
  - Scripts/
    - BackgroundScroller_A.cs
    - BackgroundScroller_B.cs
    - TextureScroller.cs
    - ParallaxController1.cs
    - ParallaxController2.cs
    - ParallaxTextureScroller.cs
    - AutoParallaxScroller.cs
    - ObjectPooler.cs
    - PoolSpawner.cs
    - CollectiblePotionPooled.cs
    - PlayerMovement.cs
    - PlayerStats.cs
  - Prefabs/
    - HiddenPlatform.prefab
    - jumpPotion.prefab
  - Materials/
    - bg_front_mat.mat
    - bg_mid_mat.mat
    - bg_back_mat.mat
  - Sprites/Background/
    - bg_front_repeat.png
    - bg_mid_repeat.png
    - bg_back_repeat.png

---

## Enunciado general de la práctica

Reutilizar el proyecto Unity 2D en el que se ha trabajado. El objetivo de la sesión es familiarizarse con técnicas empleadas para el scrolling de los fondos, así como el uso de diferentes cámaras en el juego. Adicionalmente, implementar un sistema de *pooling* de objetos y revisar el código de la entrega anterior para proponer mejoras de rendimiento.

Objetivos concretos:
- Tarea 1: Implementar *scrolling* de fondo con cámara fija (fondos se mueven).
- Tarea 2: Implementar *scrolling* de fondo con cámara móvil (fondos "saltan").
- Tarea 3: Implementar *scrolling* de fondo modificando el *offset* de la textura del material.
- Tarea 4: Aplicar efecto Parallax moviendo las posiciones de los fondos a diferentes velocidades.
- Tarea 5: Aplicar efecto Parallax modificando el *offset* de la textura de varios materiales.
- Tarea 6: Implementar un sistema de *pooling* de objetos para recolectables.
- Tarea 7: Revisar el código anterior y proponer mejoras de optimización.

---

### Tarea 1: Implementar *scrolling* de fondo con cámara fija (fondos se mueven).
*Tarea: Aplicar un fondo con scroll a tu escena utilizando la técnica descrita en a.*

Esta técnica simula un mundo infinito en un juego donde la cámara permanece estática (como un *Endless Runner*). El efecto se logra desplazando dos `GameObjects` de fondo (`currentBackground` y `auxiliaryBackground`) continuamente hacia la izquierda.

Para ello, he creado un script gestor `BackgroundScroller_A.cs` que controla ambos fondos.

#### Configuración inicial
En `Start()`, guardo la posición inicial del fondo principal (`currentStartPosition`). Obtengo el ancho del sprite (`spriteWidth`) y posiciono el `auxiliaryBackground` exactamente a la derecha del principal.

```csharp

        // Guardamos la posición inicial del primer fondo
        currentStartPosition = currentBackground.position;

        // Obtenemos el ancho
        spriteWidth = currentBackground.GetComponent<SpriteRenderer>().bounds.size.x;

        // Posicionamos el fondo auxiliar
        auxiliaryBackground.position = new Vector3(currentBackground.position.x + spriteWidth, currentBackground.position.y, currentBackground.position.z);
```

#### Lógica de `Scroll` e intercambio
En `Update()`, muevo ambos fondos a la izquierda. Cuando el `currentBackground` se ha movido completamente fuera de vista (una distancia de `spriteWidth`), lo teletransporto a la derecha del `auxiliaryBackground` e intercambio sus roles.
```csharp
   
    /* Para dar sensación de desplazameinto, movemos ambos fondos a la izquierda*/
    Vector3 moveDelta = Vector2.left * scrollSpeed * Time.deltaTime;
    currentBackground.Translate(moveDelta, Space.World);
    auxiliaryBackground.Translate(moveDelta, Space.World);

    // Comprobamos si el fondo actual se ha movido la distancia de su ancho
    // Decido usar 'spriteWidth' completo
    if (currentBackground.position.x < currentStartPosition.x - spriteWidth)
    {
        // Teletransportamos el fondo actual a la derecha del auxiliar
        currentBackground.position = new Vector3(auxiliaryBackground.position.x + spriteWidth, currentBackground.position.y, currentBackground.position.z);

        // Intercambiamos los roles (auxiliar pasa aactual y viceversa)
        Transform temp = currentBackground;
        currentBackground = auxiliaryBackground;
        auxiliaryBackground = temp;

        // Actualizamos la nueva "posición inicial" para la siguiente comprobación
        currentStartPosition = currentBackground.position;
    }
```
![Ejercicio 1](Docs/prueba1-1.gif)

--- 

### Tarea 2: Implementar *scrolling* de fondo con cámara móvil (fondos estáticos que "saltan").
*Tarea: Aplicar un fondo con scroll a tu escena utilizando la técnica descrita en b.*

Esta es la técnica estándar para un juego de plataformas como el que se viene desarrollando en estas prácticas. La cámara se mueve con el jugador (ya gestionado por `Cinemachine` o un *script* de seguimiento), y los fondos, que están estáticos, se "teletransportan" para que el jugador nunca vea el final del mundo.

Para ello, he creado el script `BackgroundScroller_B.cs`, que gestiona los dos fondos (`background_A` y `background_B`).

#### Configuración inicial
En `Start()`, valido que la `mainCamera` esté asignada. Inicialmente se sugería en el enunciado calcular el ancho de la cámara (`cameraHalfWidth`, la mitad del ancho de la cámara, usando `orthographicSize * aspect`) para esta lógica. Pero dada la parametrización de mi escena y fondos, solo necesito el ancho del fondo (`spriteWidth`). Finalmente, posiciono `background_B` exactamente a la derecha de `background_A`.

```csharp

    // Validar la cámara
    if (mainCamera == null)
    { 
        Debug.LogError("BackgroundScroller: No se ha asignado una 'mainCamera'.");
        this.enabled = false; // Desactiva este script si no hay cámara
        return; 
    }
    
    // 'bounds.size.x' es el ancho del fondo
    spriteWidth = background_A.GetComponent<SpriteRenderer>().bounds.size.x;

    // Posicionamos B a la derecha de A
    background_B.position = new Vector3(background_A.position.x + spriteWidth, background_A.position.y, background_A.position.z);
```

#### Lógica de "salto" de los fondos
He implementado la lógica en `LateUpdate()` para que se ejecute **después** de que la cámara haya seguido al jugador.

La lógica del enunciado, comparaba bordes con centros y causaba parpadeos, entre algunos motivos, dado  que gestionaba solo 2 fondos de tamaño pequeño. En la versión final, mi script compara el **centro de la cámara** (`camX`) con el **centro de los fondos**:
1.  Identifico qué fondo está a la izquierda (`leftBackground`) y cuál a la derecha (`rightBackground`).
2.  Si el centro de la cámara (`camX`) pasa el centro del `rightBackground` (moviéndose a la derecha), el `leftBackground` "salta" al otro lado.
3.  Uso `else if` para la comprobación inversa (moviéndose a la izquierda).

```csharp
    camX = mainCamera.transform.position.x;

    // Identificar qué fondo es 'left' y cuál 'right'
    if (background_A.position.x < background_B.position.x)
    {
        leftBackground = background_A;
        rightBackground = background_B;
    }
    else
    {
        leftBackground = background_B;
        rightBackground = background_A;
    }

    /* --- LÓGICA DE SALTO DE FONDO (Teleport) --- */

    /* --- Moverse a la DERECHA --- */
    // Si el centro de la cámara pasa el centro del fondo 'right'
    if (camX > rightBackground.position.x)
    {
        // 'left' salta a la derecha de 'right'
        leftBackground.position = new Vector3(rightBackground.position.x + spriteWidth, leftBackground.position.y, leftBackground.position.z);
        Debug.Log($"Movido {leftBackground.name} a la derecha de {rightBackground.name}");
    }
    
    /* --- Moverse a la IZQUIERDA --- */
    // Si el centro de la cámara pasa el centro del fondo 'left'
    else if (camX < leftBackground.position.x)
    {
        // 'right' salta a la izquierda de 'left'
        rightBackground.position = new Vector3(leftBackground.position.x - spriteWidth, rightBackground.position.y, rightBackground.position.z);
        Debug.Log($"Movido {rightBackground.name} a la izquierda de {leftBackground.name}");
    }
```

![Ejercicio 2](Docs/prueba2-1.gif)

---

### Tarea 3: Scroll por desplazamiento de textura (Offset)
*Tarea: Aplicar un fondo a tu escena aplicando la técnica del desplazamiento de textura.*

Esta técnica es más eficiente que mover `GameObjects` físicos, ya que el objeto de fondo permanece estático en la escena. El *scroll* se simula moviendo el "punto de inicio" (*Offset*) de la textura *dentro* del material del objeto.

Para implementar esto correctamente, ha sido necesario usar un objeto 3D tipo **Quad** en lugar de un *Sprite Renderer*, ya que los Sprites bloquean las coordenadas UV que necesitamos manipular.

#### Configuración de Assets y GameObject
1.  **Objeto `Quad`:** He creado un objeto `Quad` en la escena para que actúe como pantalla de fondo y lo he escalado para cubrir la vista de la cámara.
2.  **Material y `Shader`:** He creado un material llamado `Background_Repeating`. Para evitar problemas de iluminación y asegurar que la textura se repita correctamente, he asignado el *shader* **`Universal Render Pipeline/Unlit`**.
3.  **Textura (`Wrap Mode: Repeat`):** He configurado la importación de la textura de fondo cambiando su `Wrap Mode` a **`Repeat`**. Esto es vital para que, al desplazar el *offset* más allá de 1, la imagen se repita cíclicamente sin dejar huecos ni estirarse.

#### Lógica del Script (`TextureScroller.cs`)
El script, asignado al Quad, accede al componente `Renderer`. En cada *frame*, calcula un nuevo desplazamiento (`offset`) basado en el tiempo acumulado (`Time.time`) y la velocidad deseada.

Este valor se aplica a la propiedad `mainTextureOffset` del material.

```csharp
using UnityEngine;

// Mueve el fondo cambiando el Offset de la textura, no el Transform
public class TextureScroller : MonoBehaviour
{
    [SerializeField] private float scrollSpeed = 0.5f;

    private Renderer rend;

    void Start()
    {
        // Obtenemos la referencia al Renderer
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        // 'Time.time' es el tiempo total desde que empezó el juego.
        // Multiplicado por la velocidad, nos da un 'offset' que siempre aumenta.
        float offset = Time.time * scrollSpeed;

        // El 'Wrap Mode: Repeat' se encarga de que la textura se repita
        // cuando el offset supera 1.
        // Solo movemos el eje X, el Y (vertical) se queda en 0.
        rend.material.mainTextureOffset = new Vector2(offset, 0);
    }
}
```

![Ejercicio 3](Docs/prueba3-1.gif)

---

### Ejercicio 4: Efecto Parallax

El efecto **Parallax** es una técnica visual donde las imágenes del fondo se mueven más lento que las del primer plano al desplazarse la cámara, creando una ilusión de profundidad y 3D en un entorno 2D.

En Unity, se consigue separando los elementos del escenario en distintas capas (layers) y asignándoles un script que actualice su posición siguiendo a la cámara, pero multiplicando el movimiento por un factor de velocidad distinto para cada capa (cuanto más "lejos" esté la capa, menor debe ser el multiplicador para que se mueva menos).


#### 4.1. Aplicar efecto Parallax moviendo las posiciones de los fondos a diferentes velocidades.
*Tarea: Aplicar efecto parallax usando la técnica de scroll en la que se mueve continuamente la posición del fondo.*

En esta tarea, he aplicado este efecto utilizando la técnica de desplazamiento de posición (*Transform*), reutilizando la lógica de bucle infinito de la **Tarea 1** (Cámara fija), pero aplicada a múltiples capas con diferentes velocidades relativas.

Para ello, he creado el script `ParallaxController1.cs`, que añade la variable `parallaxFactor` para modular la velocidad.

El script funciona igual que el *scroller* básico, gestionando dos fondos (`currentBackground` y `auxiliaryBackground`) que se alternan. La diferencia clave está en el cálculo de la velocidad en el `Update()`:

1.  Se define un `parallaxFactor` (entre 0 y 1).
2.  Se calcula la `finalSpeed` multiplicando la velocidad base por este factor.
3.  Los objetos más lejanos tendrán un factor cercano a 0 (movimiento lento), y los cercanos un factor de 1 (movimiento normal).

```csharp
[SerializeField] private float scrollSpeed = 2f;
[Tooltip("Multiplicador de velocidad (0 = quieto, 1 = velocidad normal)")]
[SerializeField] [Range(0f, 1f)] private float parallaxFactor = 0.5f; 

    .
    .
    .

void Update()
{
    /* Calculamos la velocidad real de esta capa aplicando el factor */
    float finalSpeed = scrollSpeed * parallaxFactor;

    /* Para dar sensación de desplazameinto, movemos ambos fondos a la izquierda */
    Vector3 moveDelta = Vector2.left * finalSpeed * Time.deltaTime;
    currentBackground.Translate(moveDelta, Space.World);
    auxiliaryBackground.Translate(moveDelta, Space.World);

    // Comprobamos si el fondo actual se ha movido la distancia de su ancho
    // Decido usar 'spriteWidth' completo
    if (currentBackground.position.x < currentStartPosition.x - spriteWidth)
    {
        // Teletransportamos el fondo actual a la derecha del auxiliar
        currentBackground.position = new Vector3(auxiliaryBackground.position.x + spriteWidth, currentBackground.position.y, currentBackground.position.z);

        // Intercambiamos los roles (auxiliar pasa a actual y viceversa)
        Transform temp = currentBackground;
        currentBackground = auxiliaryBackground;
        auxiliaryBackground = temp;

        // Actualizamos la nueva "posición inicial" para la siguiente comprobación
        currentStartPosition = currentBackground.position;
    }
}
```

He organizado la escena creando tres objetos gestores, uno para cada capa de profundidad, y les he asignado diferentes factores de movimiento y posición en la escena (Z) para lograr el efecto visual deseado:

* **Capa Fondo (`ParallaxManBack`):** Factor **0.1**. Se mueve muy lento, simulando estar muy lejos.
* **Capa Intermedia (`ParallaxManInt`):** Factor **0.5**. Se mueve a velocidad media.
* **Capa Frontal (`ParallaxManFront`):** Factor **1**. Se mueve a la velocidad estándar, simulando cercanía.

![Ejercicio 4](Docs/prueba4-1.gif)

---

#### 4.2. Aplicar efecto Parallax con cámara móvil y scroll infinito.
*Tarea: Aplicar efecto parallax usando la técnica de scroll en la que se mueve continuamente la posición del fondo (adaptado a cámara móvil).*

En esta tarea, he adaptado la lógica de **scroll infinito** con cámara móvil (la técnica de "salto" de la **Tarea 2**) para incluir el efecto de profundidad Parallax.

Para ello, he creado el script `ParallaxController2.cs`. La diferencia fundamental con el *scroller* normal es que los fondos no permanecen estáticos en el mundo, sino que se desplazan ligeramente en función del movimiento de la cámara para crear la ilusión de profundidad.

La lógica en `LateUpdate()` combina dos comportamientos:

1.  **Movimiento Parallax:** Calculo cuánto se ha movido la cámara desde el último frame (`deltaX`). Muevo los fondos en la misma dirección multiplicando ese delta por el `parallaxFactor`.
    * Si el factor es cercano a **1**, el fondo se mueve casi a la par que la cámara (parece estar muy lejos, como el cielo).
    * Si el factor es **0**, el fondo no se mueve (se comporta como suelo estático).
2.  **Scroll Infinito (Teleport):** Mantengo la lógica de comprobar centros para "teletransportar" el fondo que queda fuera de visión y colocarlo al otro extremo, garantizando que el fondo nunca se acabe.

```csharp
[Tooltip("0 = Estático en el mundo, 1 = Se mueve con la cámara (muy lejos)")]
[SerializeField] [Range(0f, 1f)] private float parallaxFactor = 0.5f;

// ... (Variables de cámara y fondos) ...
private float lastCamX; // Para calcular el delta de movimiento

void LateUpdate()
{
    camX = mainCamera.transform.position.x;

    /* --- LÓGICA DE MOVIMIENTO PARALLAX --- */
    // 1. Calculamos cuánto se ha movido la cámara en este frame
    float deltaX = camX - lastCamX;
    
    // 2. Calculamos cuánto debe moverse el fondo
    float parallaxSpeed = deltaX * parallaxFactor;

    // 3. Movemos ambos fondos físicamente
    background_A.Translate(new Vector3(parallaxSpeed, 0, 0));
    background_B.Translate(new Vector3(parallaxSpeed, 0, 0));

    // Actualizamos la última posición
    lastCamX = camX;

    /* --- LÓGICA DE SALTO DE FONDO (Teleport Infinito) --- */
    // (Reutilización de la lógica de la Tarea 2 para el bucle infinito)
    .
    .
    .
}
```

Para la configuración de la escena, he reutilizado la estructura jerárquica organizada en la tarea anterior, compuesta por tres objetos gestores (`ParallaxManBack`, `ParallaxManInt` y `ParallaxManFront`). A cada uno de estos gestores le he asignado el nuevo script `ParallaxController2`, ajustando el `parallaxFactor` para definir su comportamiento respecto al movimiento de la cámara:

* **Capa Fondo (`ParallaxManBack`):** Factor **0.9**. Al moverse casi a la misma velocidad que la cámara, da la sensación de estar muy lejos (como el cielo o montañas distantes), ya que apenas cambia su posición relativa respecto al encuadre.
* **Capa Intermedia (`ParallaxManInt`):** Factor **0.5**. Se desplaza a la mitad de la velocidad de la cámara, generando una clara sensación de profundidad media.
* **Capa Frontal (`ParallaxManFront`):** Factor **0**. Al tener un factor nulo, no se aplica ningún desplazamiento extra. Estos objetos permanecen estáticos en las coordenadas del mundo, comportándose como el suelo o los elementos en primer plano por los que transita el jugador.

![Ejercicio 4-2](Docs/prueba4-2.gif)

#### Nota: Suavizado del movimiento
Al implementar el Parallax, se percibía un movimiento brusco (*jitter*) en los fondos. Esto es causado por la desincronización entre el cálculo de físicas del jugador y el renderizado de frames.

La solución efectiva fue activar la opción **Interpolate** en el componente `Rigidbody2D` del jugador. Esto suaviza su posición visual entre pasos físicos, haciendo que la cámara (y por ende el efecto Parallax) se mueva de forma fluida.

---

### Tarea 5: Aplicar efecto Parallax modificando el *offset* de la textura de varios materiales.
*Tarea: Aplicar efecto parallax actualizando el offset de la textura.*

En esta técnica, utilizamos un único objeto estático (un Quad) que contiene **múltiples materiales** (capas). El efecto de profundidad se logra desplazando el *offset* de la textura de cada material a una velocidad diferente en respuesta al movimiento de la cámara.

Para ello, he creado el script `AutoParallaxScroller.cs`.

#### Configuración de materiales
He configurado el componente `Mesh Renderer` del Quad para aceptar múltiples materiales (fondo, medio, frente).
* He creado tres materiales distintos con *shader* `Universal Render Pipeline/Unlit`: `bg_front_mat`, `bg_mid_mat` y `bg_back_mat`.
* Las texturas tienen `Wrap Mode: Repeat` para permitir el desplazamiento infinito del *offset*.
* Los materiales `bg_front_mat` y `bg_mid_mat` (capas superiores) tienen su `Surface Type` configurado como **Transparent** para permitir ver las capas inferiores a través de las partes vacías de la imagen.
**Orden y Velocidad:**
    * **Velocidad:** El orden en la lista de materiales (`Element 0`, `Element 1`...) se usa para la fórmula de velocidad. El **Elemento 0** es el más rápido (cercano) y los últimos son los más lentos (lejanos).
    * **Orden visual (renderizado):** Para asegurar que las capas se dibujen en el orden correcto independientemente de su posición en la lista, he ajustado la propiedad **Sorting Priority** en cada material. Por ejemplo, al material del fondo (`bg_back_mat`) le he asignado una prioridad de **-2**, asegurando que se renderice detrás de todo.

#### Lógica del script
He implementado un sistema de *scroll* automático constante, simulando un movimiento continuo (estilo *Endless Runner*). El script itera sobre todos los materiales asignados al `Renderer` en cada *frame*.

Para generar el efecto Parallax, aplico una fórmula matemática basada en el índice del material:
`Vector2 movementStep = (speedOffset * direction) / (i + 1.0f);`

Esto resulta en:
* **Índice 0 (Frente):** Se divide por 1, resultando en la velocidad máxima (capa más cercana).
* **Índices Superiores (Fondo):** Se divide por un número mayor, reduciendo la velocidad progresivamente para las capas lejanas.

```csharp

// Movimiento automático constante aplicando la fórmula de capas
public class AutoParallaxScroller : MonoBehaviour
{
    [Tooltip("Velocidad base del scroll")]
    [SerializeField] private float scrollSpeed = 0.5f;
    [Tooltip("Dirección del movimiento (Vector2.right mueve la textura a la derecha, lo que hace que el fondo parezca ir a la izquierda)")]
    [SerializeField] private Vector2 direction = Vector2.right;

    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        // Iteramos por todos los materiales (capas) asignados al Quad
        for (int i = 0; i < rend.materials.Length; i++)
        {
            Material m = rend.materials[i];
            float speedOffset = scrollSpeed * Time.deltaTime;
            Vector2 movementStep = (speedOffset * direction) / (i + 1.0f);
            m.mainTextureOffset += movementStep;
        }
    }
}
```

![Ejercicio 5](Docs/prueba5-1.gif)

#### Modificación: Seguimiento de cámara y sincronización
Inicialmente, el script `AutoParallaxScroller` desplazaba el fondo automáticamente por tiempo (estilo *Endless Runner*). Sin embargo, para un juego de plataformas donde el jugador puede detenerse o retroceder, el fondo debe sincronizarse con el movimiento real. Además, al desplazarse el jugador largas distancias, el objeto Quad se quedaba estático en la escena y terminaba saliendo del encuadre.

Para solucionar esto, he evolucionado el script a `ParallaxTextureScroller.cs` con dos cambios fundamentales en `LateUpdate`:

1.  **Seguimiento del `Quad`:** El objeto Quad ahora actualiza su posición en cada frame para coincidir con la cámara, asegurando que el fondo siempre cubra la pantalla.
2.  **Cálculo por `Delta`:** El desplazamiento del *offset* ya no usa `Time.deltaTime`, sino la diferencia de movimiento de la cámara (`deltaX`). Si el personaje se detiene, el fondo se detiene.

```csharp
    void LateUpdate()
    {
        // 1. Mover el Quad para que siga a la cámara (evita que el fondo se quede atrás)
        transform.position = new Vector3(cameraTransform.position.x, transform.position.y, transform.position.z);

        // 2. Calcular Parallax basado en el movimiento de la cámara (Delta) en vez del tiempo
        float deltaX = cameraTransform.position.x - lastCameraX;

        for (int i = 0; i < rend.materials.Length; i++)
        {
            Material m = rend.materials[i];
            // Aplicamos el desplazamiento según cuánto se ha movido la cámara
            m.mainTextureOffset += new Vector2(deltaX * (parallaxSensitivity / (i + 1.0f)), 0);
        }

        lastCameraX = cameraTransform.position.x;
    }
```

![Ejercicio 5](Docs/prueba5-2.gif)

---

### Tarea 6: Implementar un sistema de *pooling* de objetos para recolectables.
*Tarea: En tu escena 2D crea un prefab que sirva de base para generar un tipo de objetos sobre los que vas a hacer un pooling de objetos que se recolectarán continuamente en tu escena. Cuando un objeto es recolectado debe pasar al pool y dejar de visualizarse.*

El **Pooling de Objetos** es un patrón de optimización esencial para evitar la sobrecarga de crear y destruir objetos constantemente. En lugar de usar `Instantiate` y `Destroy`, creamos una lista fija de objetos al inicio, los desactivamos y los reciclamos.

Para implementar esto, he creado tres scripts que interactúan entre sí: `ObjectPooler.cs` (el almacén), `PoolSpawner.cs` (el generador) y `CollectiblePotionpooled.cs` (el objeto reciclable).

#### 1. El Almacén (`ObjectPooler.cs`)
Este script inicializa la "piscina" de objetos. En `Awake()`, instancia un número fijo (`amountToPool`) del prefab indicado y los desactiva (`SetActive(false)`), añadiéndolos a una lista.

Ofrece el método público `GetPooledObject()`, que recorre la lista y devuelve el primer objeto que encuentre desactivado, listo para ser usado.

```csharp
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
```

#### 2. El Generador (`PoolSpawner.cs`)
Este script simula la aparición continua de items. Tiene un temporizador en `Update()` y, cada cierto tiempo (`spawnRate`), pide un objeto al `ObjectPooler`. Si recibe uno válido, lo posiciona aleatoriamente en la escena y lo activa (`SetActive(true)`).

Como el *spawner* se mueve con el jugador, he modificado el cálculo de posición para que sea relativo a su posición actual: `transform.position.x + randomX`.

```csharp
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
        
        // Lo colocamos en su sitio y reseteamos rotación
        obj.transform.position = spawnPos;
        obj.transform.rotation = Quaternion.identity; // Rotación (0, 0, 0)
        
        // Importante encenderlo
        obj.SetActive(true);
    }
}
```

#### 3. El objeto recolectable (`CollectiblePotionpooled.cs`)
He adaptado el script de recolección para el sistema de *pooling*. La lógica de colisión es idéntica a la original, pero la acción final cambia para permitir el reciclaje del objeto:

* **Antes:** `Destroy(gameObject);` (Eliminaba el objeto de la memoria).
* **Ahora:** `gameObject.SetActive(false);` (Apaga el objeto, devolviéndolo al *pool* para ser reutilizado por el *Spawner* más adelante).

```csharp
private void OnTriggerEnter2D(Collider2D other)
{
    // Comprobamos si el objeto que ha entrado es el "Player"
    if (other.CompareTag("Player"))
    {
        // Intentamos obtener el script de Stats del jugador
        PlayerStats stats = other.GetComponent<PlayerStats>();
        
        if (stats != null)
        {
            // Le decimos al script del jugador que sume 1
            stats.AddJumpPower(1);
        }

        // NO usamos Destroy(gameObject);
        // Usamos SetActive(false). Esto "devuelve" el objeto al pool (lo apaga).
        gameObject.SetActive(false);
    }
}
```

![Ejercicio 6](Docs/prueba6.gif)

---

### Tarea 7: Revisión de código y optimización
*Tarea: Revisa tu código de la entrega anterior e indica las mejoras que podrías hacer de cara al rendimiento.*

Tras analizar el código desarrollado durante la práctica y compararlo con las directrices de "buenas prácticas", he identificado puntos de mejora y puntos donde ya se está cumpliendo con la optimización.

#### Mejora implementada: Optimización de strings en UI (`PlayerStats.cs`)
En el script `PlayerStats.cs`, actualizamos la interfaz de usuario cada vez que cambia la puntuación o el poder de salto.

**Código Original (Poco eficiente):**
```csharp
// Genera "basura" cada vez que se llama porque crea un nuevo string en memoria
jumpPowerText.text = "JUMP POWER: " + currentJumpPower;
```
Concatenar cadenas con `+` dentro de métodos recurrentes genera asignaciones de memoria innecesarias que el Recolector de Basura (GC) debe limpiar, causando picos de CPU.

**Mejora Propuesta (StringBuilder):**
Siguiendo las notas facilitadas en el aula, he optimizado esto utilizando `System.Text.StringBuilder`. Al cachear el objeto `StringBuilder`, reutilizamos la memoria en lugar de crear nuevas cadenas constantemente.

*Modificación en `PlayerStats.cs`:*
```csharp
using System.Text; // Necesario para StringBuilder

public class PlayerStats : MonoBehaviour
{
    // ... variables ...
    private StringBuilder sb = new StringBuilder(); // Cacheamos el Builder

    // Método para actualizar la UI optimizado para consumo de recursos
    private void UpdateUI()
    {
        if (jumpPowerText != null)
        {
            sb.Clear();                     // Limpiamos el builder anterior
            sb.Append(currentJumpPower);
            jumpPowerText.text = sb.ToString();
        }
    }
}
```


#### Mejora implementada: Creación de prefab para "plataformas invisibles" (`HiddenPlatform`)

Para finalizar la implementación de las plataformas invisibles, he realizado tres ajustes clave: convertir el objeto en un **Prefab**, configurar la física para que solo detecte el suelo por la parte superior y optimizar el rendimiento eliminando scripts innecesarios.

##### Creación del Prefab, mejora de rendimiento
Convertir la plataforma en un prefab es fundamental para la reutilización y el mantenimiento. Si decido cambiar el sprite o la física de la plataforma, el cambio se propagará a todas las instancias del nivel.

**Pasos realizados:**
1.  He arrastrado el `GameObject` **HiddenPlatform** desde la Jerarquía a la carpeta `Assets/Prefabs` (o `Resources`).
2.  He eliminado el objeto original de la escena.
3.  He arrastrado el nuevo Prefab a la escena en las posiciones deseadas.

##### Colisión "solo por arriba" ("One Way Platform")
Para que la plataforma solo se considere "suelo" cuando el jugador está encima (y permita saltar a través de ella desde abajo o atravesarla lateralmente sin frenarse), la solución más robusta y eficiente en Unity es usar un **`Platform Effector 2D`**.

**Añadir Efefector al Prefab:**
1.  En el componente **`Box Collider 2D`**, he marcado la casilla **`Used By Effector`**.
2.  He añadido el componente **`Platform Effector 2D`**.
3.  En el Effector, he desactivado `Use Side Bounce` y `Use Side Friction` (opcional, para evitar roces laterales).
4.  El valor **Surface Arc** se mantiene en `180`, lo que significa que la física sólida solo se aplica en el semicírculo superior.


De esta forma, el motor de físicas gestiona automáticamente que el jugador pueda atravesarla desde abajo, pero aterrice sobre ella. Mi script de `PlayerMovement` detectará la capa `PlatInv` solo cuando *los pies estén sobre* la superficie del effector.

##### Eliminar `HideOnStart`, mejora del rendimiento
Esta es la forma más eficiente: **Configuración en el editor en lugar de lógica en `Start`.**

Tener un script (`MonoBehaviour`) en cada plataforma que se ejecuta en `Start()` solo para hacer `GetComponent<SpriteRenderer>().enabled = false;` consume recursos de CPU innecesarios (llamada al ciclo de vida de Unity + búsqueda de componente) cada vez que se carga la escena o se instancia el objeto.

1.  He **eliminado** el componente `Hide On Start` del Prefab.
2.  En el componente **`Sprite Renderer`** del Prefab, he **desmarcado la casilla de activación** (el check junto al nombre del componente).

Con esto la plataforma nace invisible por defecto (sin coste de procesamiento). La lógica de "hacerse visible" reside en el jugador (`PlayerMovement`), que al tocarla ejecuta `platformRenderer.enabled = true`. Al activar el renderer, Unity simplemente empieza a dibujarlo.


#### Buenas prácticas ya aplicadas en el proyecto
Revisando los scripts entregados (`PlayerMovement.cs`, `BackgroundScroller.cs`, etc.), he verificado que ya cumplos con la mayoría de recomendaciones de rendimiento:

* **Uso de `CompareTag`:** En todos los scripts de colisión (`OnCollisionEnter2D`, `OnTriggerEnter2D`), utilizamos `collision.gameObject.CompareTag("Player")` en lugar de `collision.tag == "Player"`. Esto es mucho más rápido porque evita la asignación de memoria de comparar strings puros.
* **Cacheo de Componentes (Reducir `GetComponent`):** En todos los métodos `Start()`, almacenamos las referencias a `Rigidbody2D`, `Animator`, `Renderer` y `Camera`. Evito utilizar `GetComponent` o `Find` dentro de `Update()` o `FixedUpdate()`, evitando la sobrecarga de búsqueda en cada frame.
* **Cacheo de `LayerMask`:** En `PlayerMovement.cs`, guardo los ID de las capas (`LayerMask.NameToLayer`) en variables enteras (`groundLayer`) durante el `Start()`. Esto evita tener que buscar la capa por su nombre (string) cada vez que el personaje toca el suelo.
* **Object Pooling:** La implementación de la **Tarea 6** es en sí misma una de las mayores optimizaciones posibles, sustituyendo la costosa instanciación y destrucción de objetos por la activación/desactivación de una lista pre-generada.
* **Cacheo de `Camera.main`:** En los scripts de Parallax, compruebo y cacheo la referencia a la cámara (`cameraTransform`) en el inicio, evitando el uso costoso de `Camera.main` en cada frame.