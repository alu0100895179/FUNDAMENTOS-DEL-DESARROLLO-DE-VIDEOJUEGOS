# FDV PRACTICA 6: Mecánicas en Unity 2D (Salto, Plataformas y Recolección)

## Alumno
- Nombre: Jaime Madico Cañete
- ALU: alu0100895179
- Correo: alu0100895179@ull.edu.es

### Demostración de ejecución
![Ejercicio5-1](Docs/prueba5-1.gif)

## Entorno
- Unity 6.2: 6000.2.7f2
- Plataforma: Windows
- Input System: Unity New Input System (UnityEngine.InputSystem)
- Motor de físicas: 2D Physics (Rigidbody2D, Collider2D)
- Tilemaps: Sistema de Unity (Grid, Tilemap, Tile Palette, Composite Collider 2D)
- Sistema UI: Unity UI (Canvas, Text / TMP, Slider, Button)
- Control de versiones: Git (repositorio en GitHub)  
- Entrega: Repositorio + GIF demostrativo + .zip en Campus Virtual

## Resumen
En esta práctica se implementan mecánicas físicas básicas y comunes en juegos 2D reutilizando el proyecto base trabajado previamente. El objetivo es familiarizarse con el uso de `Rigidbody2D` y `Collider2D`, la gestión de colisiones y capas, la relación padre/hijo para plataformas móviles, la interacción con el sistema de entrada y la actualización de la UI en tiempo real. Todo esto en lo que se viene trabajando en las anteriores entregas para poder explotarlo mejor en la elaboración de **mecánicas**.

Las **mecánicas** en videojuegos son las reglas y acciones que definen la interacción del jugador con el juego; básicamente, es lo que el jugador _puede hacer_ (saltar, disparar, gestionar inventario, resolver puzles). Determinan cómo funciona el juego y la experiencia del jugador.

En **Unity**, estas mecánicas se gestionan principalmente programando **scripts** (normalmente en C#) que se adjuntan como **Componentes** a los **GameObjects** . Estos scripts contienen la lógica que define el comportamiento: cómo se mueve un personaje, cómo responde un enemigo o qué pasa al pulsar un botón. 

Las mecánicas a implementar incluyen: **salto controlado** (con comprobación de suelo), **plataformas móviles** (el jugador se convierte en hijo de la plataforma mientras está encima), gestión de **colisiones por capas** (incluir y excluir efectos de colisión), plataformas **invisibles** que se vuelven visibles al interactuar, y un **sistema de recolección** que aumenta puntuación y, al alcanzar umbrales, mejora la potencia de salto del jugador. Se entregará el código en scripts claramente organizados, gifs que demuestren cada mecánica y este informe en markdown que explica el desarrollo.

---

## Ficheros más importantes entregados

- Scenes/
  - Mecanicas.unity
- Scripts/
  - PlayerMovement.cs
  - PlayerStats.cs
  - Collectible.cs
  - PlatformMover.cs
  - HideOnStart.cs
  - eventCollisionController.cs
- Resources/
  - jumpPotion.prefab
- Animations/
  - knight/
    - knightAC.controller
    - idle_anim.anim
    - run_anim.anim
    - jump_anim.anim
    - fall_anim.anim
  - potions/
    - BluePotionAnim.anim
- Physics/
  - PlayerPhysics.physicsMaterial2D

---

## Enunciado general de la práctica

Reutilizar el proyecto Unity 2D en el que se ha trabajado previamente y desarrollar una escena con varios personajes (entre ellos un jugador controlado por teclado). Implementar las mecánicas que aparecen en el guion de la práctica sobre Mecánicas, centradas en el uso del motor de físicas. Seguir la política de entrega: repositorio GitHub con scripts y gifs demostrativos, y .zip del repositorio subido al campus virtual.

Objetivos concretos:
- Implementar un salto condicional (solo saltar cuando el jugador esté sobre el suelo).
- Implementar plataformas móviles y lograr que el jugador se mueva con ellas (parenting while on platform).
- Controlar colisiones mediante capas para incluir/excluir efectos.
- Implementar plataformas invisibles que se activan/visibilizan al colisionar.
- Implementar mecánica de recolección que aumente la puntuación y, al alcanzar un umbral, aumente la potencia de salto del jugador.
- Mostrar puntuación en pantalla en tiempo real y proveer feedback visual al recoger objetos.

---

### Ejercicio 1: Salto
*Recupera el Rigidbody2D del objeto y aplica una fuerza vertical al objeto cuando se pulse una tecla.Es necesario controlar que el jugador sólo salte si está en el suelo.*

Para implementar el salto, he modificado el script `PlayerMovement` que venía utilizando hasta ahora para que utilice el motor de físicas de Unity. Es importante ya que de este modo dejamos de usar `transform.Translate` (que "teletransporta") y pasamos a usar un **`Rigidbody2D`**, que es afectado por la gravedad y al que podemos aplicar fuerzas.

#### 1.1. Fuerza vertical
*Implementar una mecánica de salto aplicando una fuerza vertical.*

El script ahora fuerza la inclusión de un `Rigidbody2D` (con `[RequireComponent]`) y lo configura en `Start()`. Para que el personaje no se caiga congelo la rotación Z. Esto se puede ver en:

```csharp
// Se necesita un Rigidbody2D en el objeto: movimiento físico.
// Forzamos que se añada automáticamente si falta.
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    // ...
    [SerializeField] private float jumpForce = 5f; // Variable para el salto
    // ...
    private Rigidbody2D rb2D;       // Necesario para físicas y salto
    // ...
    void Start()
    {
        // ...
        rb2D = GetComponent<Rigidbody2D>(); // Obtenemos el Rigidbody
        // ...
        // Congelar la rotación Z para que el personaje no se caiga
        rb2D.constraints = RigdbodyConstraints2D.FreezeRotation;
    }
       
```

La lógica de salto se ha implementado en `Update()` para capturar la pulsación de la tecla de forma instantánea (usando `Keyboard.current.spaceKey.wasPressedThisFrame` para asegurar que el salto ocurra una sola vez por pulsación). He añadido una variable pública `jumpForce` para poder ajustar la potencia desde el *Inspector*.

 fuerza usando `rb2D.AddForce` con `ForceMode2D.Impulse`, que simula una "explosión" o impulso instantáneo, fuerza que resulta ideal para un salto.
```csharp
    // En Update se leen Inputs
    void Update()
    {
        // ... (Lectura de input horizontal) ...

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            // Aplicamos la fuerza de salto vertical
            // Usamos 'Impulse' para una "explosión" instantánea de fuerza
            rb2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        // ... (Lógica de volteo y animación) ...
    }
```
Convivencia con el movimiento físico (en `FixedUpdate`): el movimiento horizontal (que sí es continuo) se gestiona en `FixedUpdate()` para sincronizarse con el motor de físicas.

Se preserva la **velocidad vertical**: `new Vector2(h * speed, rb2D.linearVelocity.y)`. Esto nos permite modificar la velocidad horizontal (`h * speed`) **sin anular la fuerza de la gravedad o el impulso del salto (que se están aplicando en el eje Y)**.

```csharp
    void FixedUpdate()
    {
        // Mantenemos la velocidad vertical (rb2D.velocity.y) que ya tenga (gravedad o salto)
        // y solo modificamos la velocidad horizontal (h * speed).
        rb2D.linearVelocity = new Vector2(h * speed, rb2D.linearVelocity.y);
    }
```

![Ejercicio1-1](Docs/prueba1-1.gif)

---

#### 1.2. Control de Salto (Ground Check)
*En este apartado se debe agregar el código que maneja que sólo se salte si el jugador está en el suelo. Para ello se deben agregar una etiqueta al suelo y mediante una variable que actuará de flag, activar o desactivar el salto.*

Al implementar esta mejora conseguimos evitar el salto infinito en el aire.

1. Creación del Tag "Ground".
Al implementar esta mejora conseguimos evitar el salto infinito en el aire. Primero, he ido a `Edit > Project Settings > Tags and Layers` y he creado un nuevo **Tag** llamado **`Ground`**. He aplicado este Tag a todos los objetos del `Tilemap` y/o plataformas que deben considerarse "suelo".

2. Modificación del Script `PlayerMovement`.
He añadido una variable booleana `isGrounded` y la he inicializado en `false` por seguridad: `private bool isGrounded = false;`

La lógica de salto en `Update()` ahora comprueba directamente si `isGrounded` es ***true***. Ya no necesitamos poner el *flag* a *true* manualmente, porque el OnCollisionExit2D gestionará cuándo dejamos de estar en el suelo.
```csharp
    void Update()
    {
        // ... (Input horizontal y volteo) ...

        // --- 2. LEER INPUT DE SALTO ---
        // Se añade la comprobación '&& isGrounded'
        if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
        {
            // Aplicamos la fuerza de salto vertical
            rb2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
        // ... (Lógica de volteo y animación) ...
    }
```
Finalmente, he añadido los dos métodos de colisión al final del script para manejar el estado de `isGrounded`:
```csharp
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Si el jugador colisiona con un objeto con la etiqueta "Ground"
        if (collision.gameObject.CompareTag("Ground"))
        {
            // Estamos en el suelo
            isGrounded = true; 
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Si dejamos de tocar el suelo
        if (collision.gameObject.CompareTag("Ground"))
        {
            // Ya no estamos en el suelo
            isGrounded = false;
        }
    }
 ```   
Con esto conseguimos controlarlo limpiamente, ya que `isGrounded` siempre refleja la realidad física del jugador, permitiendo que la lógica de salto solo se preocupe de si el jugador "puede" saltar ahora mismo.

![Ejercicio1-2](Docs/prueba1-2.gif)

---

### Ejercicio 2: Mécánicas relacionadas con plataformas - Salto a una plataforma

Para esta mecánica, el objetivo es que el jugador detecte una plataforma que esté en movimiento. La solución sería hacer que el jugador se convierta en hijo (`parent`) de la plataforma al aterrizar sobre ella, y deje de serlo al saltar (`SetParent(null)`).

#### Creación del *Tag* `MovingPlatform`
Para diferenciar el suelo normal (estático) de las plataformas móviles, he creado un nuevo **Tag** llamado **`MovingPlatform`**.

Este Tag se lo he aplicado a los objetos de plataforma que tendrán movimiento. Para mis pruebas una plataforma móvil de madera. 

He modificado la lógica de `isGrounded` del ejercicio anterior para que el jugador sepa que está "en el suelo" tanto si toca un objeto `Ground` como si toca un `MovingPlatform`.

#### Modificación del *Script* `PlayerMovement`

He expandido los métodos `OnCollisionEnter2D` y `OnCollisionExit2D` para que manejen esta nueva lógica de "***parenting***" además de la lógica de `isGrounded`.

**`OnCollisionEnter2D` (Aterrizar):**
Cuando el jugador aterriza, comprobamos qué tipo de suelo es. Si es una plataforma móvil, además de poner `isGrounded = true`, usamos `transform.SetParent()` para anclar al jugador.

```csharp
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Comprobamos si es suelo
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }

        // Comprobamos si es una plataforma MOVIL ("MovingPlatform")
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            isGrounded = true;
            // Nos hacemos hijos de la plataforma para movernos con ella
            transform.SetParent(collision.transform);
            Debug.Log("¡Anclado a plataforma!");
        }
    }
```

**`OnCollisionExit2D` (Despegar):** Debemos gestionar este evento para cuando el jugador deja la plataforma (ya sea saltando o cayendo), se revierten ambos efectos: `isGrounded = false` y `transform.SetParent(null)` para que deje ser hijo de la plataforma.

```csharp
private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        } else
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            isGrounded = false;
            transform.SetParent(null);
        }
    }
```
Con esta implementación, si una plataforma marcada como `MovingPlatform` se mueve, el jugador (al ser su *hijo*) se moverá con ella automáticamente.

#### Notas: Dificultades encontradas

* **Preservación de escala**: por cuestiones estéticas deseadas en el videojuego, la plataforma presenta una ***escala*** diferente en la escena. Si hacemos al jugador "hijo" directamente sin gestionar la escala, vemos que modifica su tamaño para adaptarse al padre. Para preservar la estética de nuestro personaje, he de almacenar y preservar su escala con código como: 
```csharp
private Vector3 originalScale;
...
    private void OnCollisionEnter2D(Collision2D collision){
        
        ...
    
        // Compensamos nuestra escala original
        Vector3 platformScale = collision.transform.localScale;
        originalScale.x /= platformScale.x;
        originalScale.y /= platformScale.y;
        originalScale.z /= platformScale.z;

    }
    private void OnCollisionExit2D(Collision2D collision){
        
        ...

        // Restauramos la escala original ANTES de soltarnos
        Vector3 platformScale = collision.transform.localScale;
        originalScale.x *= platformScale.x;
        originalScale.y *= platformScale.y;
        originalScale.z *= platformScale.z;        
    }
```

* **Delegar desplazamiento al padre cuando está en la plataforma**: El principal problema fue un conflicto entre el `Rigidbody2D` (física) y el `SetParent` (transform). Mi `FixedUpdate` forzaba la velocidad horizontal a 0 (`rb2D.linearVelocity = 0`) cuando el jugador estaba "quieto" (**h == 0**). Esto "chocaba" constantemente con el movimiento de la plataforma, frenando al jugador. La solución fue añadir la condición que adjunto en el bloque de código: controlaré que **solo se frene al jugador si está estático y en contacto con en el suelo** (`transform.parent == null`). Si permanece sobre la plataforma (`transform.parent != null`), el script omite el "freno", permitiendo que el `SetParent` (la plataforma) mueva al jugador libremente.
```csharp
    void FixedUpdate()
    {
        // --- 4. APLICAR MOVIMIENTO HORIZONTAL ---
        if (h != 0f) // Si el jugador está pulsando A o D
        {
            // El control lo tiene el jugador
            // Calculamos la velocidad horizontal con h (izqd o dcha) y la velocidad que hayamos decidido
            // Indicamos que la velocidad en "Y" debe ser la misma que estaba gestionando
            rb2D.linearVelocity = new Vector2(h * speed, rb2D.linearVelocity.y);
        }
        else
        {
            // Si el jugador está quieto (h == 0). Frenamos al jugador:
            // si está en suelo ( NO está encima de una plataforma)
            if (transform.parent == null)
            {
                rb2D.linearVelocity = new Vector2(0, rb2D.linearVelocity.y);
            }
            // Si tiene un 'parent' (está en la plataforma)
            // No hacemos nada, dejamos que la plataforma mueva el Rigidbody libremente
        }
    }
```

![Ejercicio2-1](Docs/prueba2-1.gif)

---

### Ejercicio 3: Manejar colisiones con elementos de una capa determinada

*Podemos utilizar las capas para que el efecto de las colisiones sólo se tenga en cuenta cuando se pertenece a una determinada capa, o se descarten.*

Para este ejercicio, he reconfigurado la lógica de colisiones para que deje de usar `CompareTag("Ground")` y, en su lugar, utilice `Layers` (Capas), como método más eficiente y potente.

#### 1. Configuración del *Layer*
Primero, he ido a `Project Settings > Tags and Layers` y he revisad la existencia de la `Layer` llamada **"Terrain"** (usada en prácticas anteriores).

Luego, he revisado la asignación de esta capa `Terrain` a todos los objetos transitables de mi escena (tanto el `Tilemap` estático como la `MovingPlatform`).

#### 2. Refactorización del *Script* `PlayerMovement`
He modificado los métodos `OnCollisionEnter2D` y `OnCollisionExit2D` para que comprueben la capa (`layer`) del objeto con el que colisionan.

Para optimizar, he guardado el ID de la capa en `Start()` para no tener que buscarlo (`LayerMask.NameToLayer`) en cada colisión:
```csharp
    // ... (Componentes) ...
    private int groundLayer; // Variable para 'cachear' el ID de la capa

    void Start()
    {
        // ... (resto del Start) ...
        
        // Guardamos el ID de la capa "Terrain" UNA SOLA VEZ
        groundLayer = LayerMask.NameToLayer("Terrain");
    }
```

La lógica de `OnCollisionEnter2D` ahora comprueba en prinmer lugar el `Layer`. Si coincide, sabemos que estamos en el **suelo** (`isGrounded = true`). Además, mantenemos la comprobación del `Tag "MovingPlatform"` para gestionar la lógica de `SetParent`.
```csharp
    // --- CONTROL DE COLISIÓN (SUELO Y PLATAFORMAS MÓVILES) ---
    // Modificado teniendo en cuenta LAYERS
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Comprobamos si la capa del objeto es la capa "Terrain"
        if (collision.gameObject.layer == groundLayer)
        {
            isGrounded = true; 

            // Adicionalmente, comprobamos si es una plataforma móvil (por su Tag)
            if (collision.gameObject.CompareTag("MovingPlatform"))
            {
                transform.SetParent(collision.transform);
                
                // Compensamos nuestra escala original
                Vector3 platformScale = collision.transform.localScale;
                originalScale.x /= platformScale.x;
                originalScale.y /= platformScale.y;
                originalScale.z /= platformScale.z;
                
                Debug.Log("Anclado a plataforma");
            }
        }
    }
```

De igual forma, `OnCollisionExit2D` ahora comprueba la capa para `isGrounded` y el *Tag* para gestionar la liberación de la plataforma:
```csharp
    private void OnCollisionExit2D(Collision2D collision)
    {
        // Comprobamos si la capa del objeto es la capa "Terrain"
        if (collision.gameObject.layer == groundLayer)
        {
            isGrounded = false;

            // Adicionalmente, comprobamos si era una plataforma móvil
            if (collision.gameObject.CompareTag("MovingPlatform"))
            {
                // Restauramos la escala original ANTES de soltarnos
                Vector3 platformScale = collision.transform.localScale;
                originalScale.x *= platformScale.x;
                originalScale.y *= platformScale.y;
                originalScale.z *= platformScale.z;

                // Comprobación de seguridad
                if (transform.parent == collision.transform)
                {
                    transform.SetParent(null);
                }
                Debug.Log("Liberado de plataforma");
            }
        }
    }
```

---

### Ejercicio 4: Plataformas invisibles que se vuelven visibles
*Podemos hacer una gestión similar al caso anterior, sólo que en este caso nos interesa manejar los casos en el que se pertenece a la capa de plataformas invisibles. Una vez detectado el caso, se ejecutará la lógica que queramos llevar a cabo, incluido, si así lo consideramos volver la plataforma visible.*

Para este ejercicio, y siguiendo la pauta del enunciado, he centralizado la lógica de detección en el script `PlayerMovement`. Es el jugador quien, al colisionar con un objeto de la capa correcta, se encarga de "revelarlo" activando su `SpriteRenderer`.

#### Configuración del *Layer* y plataforma en la escena
1.  Primero, he ido a `Project Settings > Tags and Layers` y he creado una nueva `Layer` llamada **`PlatInv`**.
2.  He colocado un `GameObject` de plataforma en la escena y le he asignado la nueva capa `PlatInv`.
3.  He creado un script muy simple, `HideOnStart.cs`, y se lo he añadido **a la plataforma invisible**. La única función de este script es ocultar la plataforma en cuanto empieza el juego:
    ```csharp
    using UnityEngine;
    public class HideOnStart : MonoBehaviour
    {
        void Start()
        {
            // Oculta el renderer al iniciar el juego
            GetComponent<SpriteRenderer>().enabled = false;
        }
    }
    ```

#### Añadir mecánica a el *Script* `PlayerMovement`
He modificado el `PlayerMovement` para que sea consciente de esta nueva capa y la gestione.

Para optimizar, he guardado el ID de la nueva capa en `Start()`, siguiendo la misma práctica usada con la capa "Terrain":
```csharp
    // ... (Componentes) ...
    private int groundLayer; // Variable para 'cachear' el ID de la capa "Terrain"
    private int invisiblePlatformLayer; // NUEVO: ID de la capa "PlatInv"

    void Start()
    {
        // ... (resto del Start) ...
        
        // Guardamos el ID de la capa "Terrain" UNA SOLA VEZ
        groundLayer = LayerMask.NameToLayer("Terrain");
        // Guardamos el ID de la capa "PlatInv" UNA SOLA VEZ
        invisiblePlatformLayer = LayerMask.NameToLayer("PlatInv");
    }
```

He expandido el método `OnCollisionEnter2D` para que detecte la colisión con esta nueva capa. Cuando el jugador choca con un objeto en la capa `PlatInv`, lo considera "suelo" (para poder saltar) y, además, activa su `SpriteRenderer` para revelarlo.

```csharp
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // --- LÓGICA NUEVA: PLATAFORMAS INVISIBLES ---
        if (collision.gameObject.layer == invisiblePlatformLayer)
        {
            // 1. Contamos como "suelo" para poder saltar
            isGrounded = true; 
            
            // 2. Intentamos obtener el renderer de la plataforma que tocamos
            SpriteRenderer platformRenderer = collision.gameObject.GetComponent<SpriteRenderer>();
            
            // 3. Si tiene un renderer y no está ya visible
            if (platformRenderer != null && !platformRenderer.enabled)
            {
                // La hacemos visible
                platformRenderer.enabled = true;
            }
        }
    }
```

Dado que esta plataforma ahora se considera "suelo" para poder saltar, también he tenido que actualizar `OnCollisionExit2D` para que `isGrounded` se ponga en false al salir de ella, igual que hacemos con la capa "Terrain".

```csharp
    private void OnCollisionExit2D(Collision2D collision)
    {
        // Comprobamos si la capa del objeto es la capa "Terrain" o "PlatInv"
        if ((collision.gameObject.layer == groundLayer) || (collision.gameObject.layer == invisiblePlatformLayer))
        {
            isGrounded = false;

            ...
```

![Ejercicio4-1](Docs/prueba4-1.gif)

---

### Ejercicio 5: Mecánica de recolección

*La recolección de objetos se realiza implementando la lógica de la recolección en alguno de los eventos de colisión con ese objeto. En ocasiones se actualizará la UI, en otras se destruirá el objeto, en otras se mejora alguna característica del personaje. En esta tarea se debe implementar una mecánica en la que el jugador recolecte objetos que le supongan alguna recompensa. En concreto, aumentará su puntuación, al llega a un valor de incremento de puntuación, adquirirá más potencia de salto. La puntuación se mostrará continuamente en la pantalla. El objeto recolectado ya no se usará más.*

Para esta mecánica final, he adoptado un enfoque de **modularización de la lógica** para mantener el código limpio y organizado, tal como lo hemos ido trabajando. He dividido la lógica en tres scripts:

1.  **`PlayerMovement.cs` (Existente):** Se encarga solo de moverse y saltar. Le hemos añadido una función `UpgradeJump()` para que un script externo pueda modificar su `jumpForce`.
2.  **`Collectible.cs` (Nuevo):** Un script simple que va en el *prefab* de la poción (nuestro objeto recolectable).
3.  **`PlayerStats.cs` (Nuevo):** Un script "cerebro" que va en el *Player* y gestiona el conteo, la UI y la lógica de mejora.

#### Configuración del recolectable (Poción)
He creado un prefab de "Poción" (el objeto animado) y le he añadido:
* Un `Collider 2D` (ej. `BoxCollider2D`).
* La casilla `Is Trigger` marcada (para que se pueda atravesar, sin físicas, ya que no es necesario).
* El nuevo script `Collectible.cs`.

Este script (`Collectible.cs`) se activa cuando un objeto entra en su *trigger*. Comprueba si es el "Player" y, de ser así, llama al script `PlayerStats` del jugador antes de autodestruirse.

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

            // Destruimos la poción
            Destroy(gameObject);
        }
    }
```

#### Configuraciones del *Player*: Stats
He añadido el script `PlayerStats.cs` al `GameObject` del Jugador. Este script actúa como el gestor central de la mecánica de recolección.

**`PlayerStats.cs`:**
Este script se encarga de:
1.  **Contar** (`currentJumpPower`).
2.  **Actualizar la UI** (`jumpPowerText`).
3.  **Gestionar la mejora de salto** (`jumpPowerThreshold`, `upgradedJumpValue`).
4.  **Aplicar efectos visuales** (la corrutina `ApplyPowerUp`).

* En `Start()`, se asegura de que el `PlayerMovement` tenga la fuerza de salto base (`baseJumpForce`).
```csharp
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();

        // Al empezar, nos aseguramos de que el PlayerMovement
        // tenga el salto base que define este script.
        if (playerMovement != null)
        {
            playerMovement.UpgradeJump(baseJumpForce);
        }

        UpdateUI();

        playerSpriteRenderer = GetComponent<SpriteRenderer>();
        if (playerSpriteRenderer != null)
        {
            originalPlayerColor = playerSpriteRenderer.color;   // Guardar el color original
        }
    }
```

* La lógica de incrementar el contador de pociones recolectadas se hace mediante el método `AddJumpPower(int amount)`, el cual se llamará desde el script del recolectable. También se comprobará en este método que se ha haya llegado al objetivo para llamar a la función `UpgradeJump` que actualiza este parámetro de salto en el script de movimiento.
```csharp
    public void AddJumpPower(int amount)
    {
        // Incrementamos la cantidad recibida como parámetro
        currentJumpPower += amount;

        UpdateUI();

        // Comprobamos si hemos llegado al objetivo
        if (currentJumpPower >= jumpPowerThreshold)
        {
            // Aplicamos la mejora UpgradeJump
            playerMovement.UpgradeJump(upgradedJumpValue);

            // Aplicar efectos gráficos de la mejora de salto
            StartCoroutine(ApplyPowerUp());
        }
    }
```

* Voy actualizando la UI con el contador ***`currentJumpPower`***.
```csharp
    private void UpdateUI()
    {
        if (jumpPowerText != null)
        {
            jumpPowerText.text = "JUMP POWER: " + currentJumpPower;
        }
    }
```

* Finalmente, uso la corrutina `ApplyPowerUp()` para gestionar la temporalidad de la mejora. El "Super Salto" no es permanente; es un *buff* que dura un tiempo determinado controlado con (`upgradeColorDuration`).

    Esta corrutina es una secuencia de eventos pausada en el tiempo:
    1.  **Activación:** Inmediatamente, da feedback visual al jugador: cambia el color del sprite a amarillo y el texto de la UI a "SUPER JUMP!".
    2.  **Espera:** Pausa la corrutina (pero no el juego) durante los 10 segundos especificados (`yield return new WaitForSeconds(...)`). Durante este tiempo, el jugador disfruta del salto mejorado.
    3.  **Reseteo:** Una vez transcurrido el tiempo, el código se reanuda y revierte todo al estado original: restaura el `baseJumpForce`, resetea el `currentJumpPower` a 0, actualiza la UI al contador normal y devuelve el sprite a su color original.
```csharp
    private IEnumerator ApplyPowerUp()
    {
        if (playerSpriteRenderer != null)
        {
            playerSpriteRenderer.color = upgradeColor;              // Cambiar a color de mejora
            jumpPowerText.text = "SUPER JUMP!";                     // Actualizamos UI para indicar la mejora
            yield return new WaitForSeconds(upgradeColorDuration);  // Esperar
            playerMovement.UpgradeJump(baseJumpForce);              // Restablecemos salto   
            currentJumpPower = 0;                                   // Restablecemos Jump Power   
            jumpPowerText.text = "JUMP POWER: " + currentJumpPower; // Actualizamos UI para indicar fin de la mejora
            playerSpriteRenderer.color = originalPlayerColor;       // Volver al color original
        }
    }
```
#### Configuraciones del *Player*: Movimiento
* **`PlayerMovement.cs`:** Al `PlayerMovement` solo he hecho privada la variable (`jumpForce; `) y he añadido una función pública (`UpgradeJump`) para que el `PlayerStats` pueda ordenarle cambiar su fuerza de salto.

```csharp
// ... (resto del script PlayerMovement) ...
[Header("Movimiento")]
[SerializeField] private float speed = 5f;        // Velocidad de movimiento
[SerializeField] private float baseJumpForce = 14f; // Valor inicial
private float jumpForce;                        // Variable de salto actual

// ... (en Start) ...
// La línea 'jumpForce = baseJumpForce' ha sido movida al PlayerStats, que incializará la fuerza de salto base

// Esta función la llamará 'PlayerStats' cuando se cumpla la condición
public void UpgradeJump(float newJumpForce)
{
    jumpForce = newJumpForce;
}
// ... (el resto del script no cambia) ...
```
El resumen del sistema es moudalar pero sencillo: las pociones son "simples" (solo detectan al jugador e indican el aumento del contador antes desaparecer), el PlayerStats es el "cerebro" que cuenta y aplica mejoras, y el PlayerMovement solo obedece órdenes sobre su fuerza de salto.

![Ejercicio5-1](Docs/prueba5-1.gif)

---

### Mejoras opcionales

Manejar por separado mediante animaciones la caída y el salto, teniendo cuenta si el *`Player`* disminuye o aumenta su coordenada "Y".

#### Estados base (en suelo)
Mientras **`Grounded`** sea **`true`**, el personaje alterna entre dos estados:
* **`idle`**: El estado por defecto si **`Speed`** es `0`.
* **`running`**: Transiciona desde `idle` si **`Speed`** es `> 0.1`. Vuelve a `idle` si **`Speed`** baja a `0`.

#### Despegue (salto vs. caída)
Cuando **`Grounded`** se vuelve **`false`**, se usa **`VelocityY`** (la velocidad vertical) para decidir, así como la variable **`RealJump`** que actúa como "chivato" de cuándo el jugador pulsa "Espacio":
* **A `jump` (Salto):** Si `Grounded` es `false` **Y** `VelocityY` es **`> 0.1`** (subiendo). La variable **`RealJump`** nos ayuda a distinguir falsos positivos por cambios ligeros en el terreno.
* **A `fall` (Caída):** Si `Grounded` es `false` **Y** `VelocityY` es **`< -0.6`** (bajando, como al caer de un bordillo). Damos algo de margen en la comparación para distinguir falsos positivos por cambios ligeros en el terreno.

#### Transición en el aire
Hay una transición clave para pasar de la animación de subida a la de bajada:
* **De `jump` a `fall`**: Ocurre en cuanto **`VelocityY`** es **`< 0.1`**. Esto sucede en el pico del salto, cuando el personaje deja de subir y empieza a caer. Esta transición no debe tener "`Exit Time`" para ser instantánea.

### Aterrizaje
* Desde `jump` no se podrá regresar directamente a un estado de suelo (consideraremos, por pequeña que sea, que en algún momento tiene que dejar de subir para empezar a caer).
* Desde `fall` sí que se gestionarán se regresa al suelo, únicamente al estado `idle`, transición que se activa en cuanto **`Grounded`** vuelve a ser **`true`**. Si inmediatamente reaunada la marcha se modificará `Speed`y volverá a `running`.
```

![Ejercicio6-1](Docs/prueba6-1.gif)