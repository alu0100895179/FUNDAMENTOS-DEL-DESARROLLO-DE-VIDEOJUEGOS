# FDV PRACTICA 3: Físicas 2D y Tilemaps

## Alumno
- Nombre: Jaime Madico Cañete
- ALU: alu0100895179
- Correo: alu0100895179@ull.edu.es

### Demostración de físicas y mapa
![Demostración del carrusel](Docs/demo-desplazamiento-horizontal.gif)

## Entorno
- Unity 6.2: 6000.2.5f1
- Plataforma: Windows
- Input System: Unity New Input System (UnityEngine.InputSystem)
- Motor de físicas: 2D Physics Engine (Rigidbody2D, Collider2D)
- Tilemaps: Sistema de Unity (Grid, Tilemap, Tile Palette, Composite Collider 2D)

## Resumen
En esta práctica se ha trabajado con el motor de físicas 2D y el sistema de Tilemaps de Unity para construir un entorno jugable en 2D. A partir del proyecto anterior (movimiento y animación de sprites), se ha ampliado con nuevos componentes que permiten:

- Detectar colisiones y triggers entre objetos físicos, así como diferenciar el comportamiento en ambos casos.
- Estudiar las diferencias entre las diferentes configuraciones de cuerpos (Dynamic, Kinematic, Static).
- Implementar un entorno con Tilemaps que delimita el escenario.
- Usar colliders compuestos (`composite`) para mejorar la detección de colisiones.
- Aplicar diferentes comportamientos físicos según el tipo de objeto o capa.

---

## Archivos entregados

- Docs/
  - demo-desplazamiento-horizontal.gif  
- PRACTICA.md  
- Scenes/
  - SampleScene.unity  
- Scripts/
  - MovimientoPersonaje2D.cs  
- Sprites/
  - idle-Back.anim  


---

## Enunciado
En esta actividad realizaremos pruebas con el motor de físicas 2D y el editor de mapas 2D que proporciona Unity. Los componentes de mayor interés son
- Rigidbody: Acceso a las simulaciones físicas
- Collider: Detección de las colisiones.

Respecto al mapa de juego, se debe trabajar con los objetos:
- Grid
- Tilemap
- Tile Palette
- Tile map collider
- Composite collider

La descripción de las actividades a realizar se recogen en el documento de la práctica sobre mapas y físicas.
La forma de entrega será similar a las prácticas anteriores.

### Ejercicio 1.1 Físicas 2D

En este ejercicio se ponen a prueba diferentes configuraciones de objetos físicos en Unity.

Para ello he incluido scripts en cada uno de los tipos de objetos: Dinámico, Cinemático, Estático y ..........
prográmales eventos OnCollision2D y OnTrigger2D que muestren un mensaje con cada uno de los tipos de evento en consola. Configura adecuadamente el collider y/o Rigidbody, además del evento que corresponda para poder imprimir el mensaje en la consola. En caso de no poderse indicar por qué.

|   Nº  | Configuración                                   | Resultado esperado                                                  | Resultado real (observado)                           | Conclusión                                                    |
| :---: | :---------------------------------------------- | :------------------------------------------------------------------ | :--------------------------------------------------- | :------------------------------------------------------------ |
| **1** | Ninguno tiene `Rigidbody2D`                     | No se genera ningún evento.                                         | ✅ No aparecen mensajes en consola.                   | Correcto — sin `Rigidbody2D`, Unity no calcula colisiones.    |
| **2** | Un objeto tiene físicas (`Dynamic`), el otro no | Se generan eventos **OnCollision2D** en ambos.                      | ✅ Logs de colisión en A y B.                         | Funciona — basta con que uno tenga `Rigidbody2D`.             |
| **3** | Ambos con físicas (`Dynamic`)                   | Se generan eventos **OnCollision2D** en ambos.                      | ✅ Logs en ambos objetos.                             | Correcto — interacción física completa.                       |
| **4** | Ambos con físicas; A tiene 10× más masa         | Igual que la prueba 3; misma detección, diferente respuesta física. | ✅ Misma detección, A apenas se mueve.                | Correcto — la masa no afecta a los callbacks.                 |
| **5** | Un objeto tiene físicas, el otro es `isTrigger` | Se generan eventos **OnTrigger2D**.                                 | ✅ Logs `OnTriggerEnter2D` en ambos.                  | Correcto — `isTrigger` funciona con un solo Rigidbody.        |
| **6** | Ambos tienen físicas, uno marcado `isTrigger`   | Se generan eventos **OnTrigger2D**.                                 | ✅ Logs `OnTriggerEnter2D`/`Exit2D` en ambos.         | Correcto — el trigger se detecta entre cuerpos físicos.       |
| **7** | Uno es `Kinematic`, el otro `Dynamic`           | Se generan eventos **OnCollision2D** al moverse el cinemático.      | ✅ Logs de colisión mientras A (cinemático) se mueve. | Correcto — colisión detectada aunque A no responda a fuerzas. |

---

## Explicación y puntos clave

- **Entrada:** Se usa el *New Input System* (`Keyboard.current`) para leer las flechas.
- **Movimiento horizontal:** Se aplica con `transform.Translate(...)` multiplicado por `speed * Time.deltaTime`.
- **Flip de sprite:** `spriteRenderer.flipX` alterna la orientación visual al moverse a izquierda/derecha.
- **Animator:** controla tres parámetros booleanos (nombres usados en código: `isWalking`, `walkedFar`, `isBack`) que existen en la máquina de estados del personaje.
- **walkedFar:** se activa cuando se acumula más distancia que `activationDistance`. Este contador se retesea a 0 modificando `totalDistance` cuando se activa.
- **Temporizador de inactividad:** `idleTimer` cuenta segundos cuando no hay movimiento ni entradas; si supera `idleThreshold` (3s) fuerza `isWalking=false`, `isBack=false` (mirar al frente) y desactiva `walkedFar` para volver contar el desplazamiento antes de cambiar la animación.

---

## Conclusión
La práctica muestra la integración de entrada, renderizado y animación en Unity 2D usando sprites. Además se decide implementar algo de lógica UX (mirar al frente tras 3s de inactividad) que mejora la sensación de vida del personaje.
