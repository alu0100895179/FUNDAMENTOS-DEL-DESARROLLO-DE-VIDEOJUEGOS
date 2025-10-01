# FDV PRACTICA 2: Introducción a la programación de juegos 2D. Sprites.

## Alumno
- Nombre: Jaime Madico Cañete
- Alu: alu0100895179
- Correo: alu0100895179@ull.edu.es

### Demostración de movimiento horizonta y animaciones
![Demostración del carrusel](Docs/demo-desplazamiento-horizontal.gif)

## Entorno
- Unity 6.2: 6000.2.5f1
- Plataforma: Windows
- Input System: Unity New Input System (UnityEngine.InputSystem)

## Resumen
En esta práctica se trabaja el manejo de **Sprites** y su animación en un juego 2D con Unity.
Para las animaciones se han utilizado los Assets contenidos en el recurso: "Recursos 2D", facilitados en el material de "Técnicas de desarrollo 2D" del aula virtual.

Se exploran:

- Uso de *SpriteRenderer* y *Animator*.
- Generación de animaciones a partir de conjuntos de sprites (atlas).
- Movimiento por *Transform* (sin físicas por el momento) y detección de distancia recorrida para cambiar estados.
- Gestión de estados del animator mediante parámetros booleanos (ej. `isWalking`, `walkedFar`, `isBack`).

Además he decidido incorporar un temporizador de inactividad que, si no se detecta movimiento ni pulsación de las teclas durante 3 segundos, fuerza la animación "idle" donde nuestro personaje queda mirando al frente.

## Enunciado
Actividad Sprites

En esta actividad realizaremos pruebas con las herramientas para el manejo y edición de Sprites en un juego 2D que proporciona Unity. Los Sprites son uno de los elementos básicos en las aplicaciones 2D en Unity. Tienen componente Transform, como todo objeto en la escena y un componente Renderer.

Podemos utilizar Sprites o Atlas de Sprites como Assets.
Las animaciones de los elementos de la escena se generan a partir de un conjunto de Sprites. Si las imágenes de una animación están en un Atlas deben ser extraídas antes de generar la animación.
Los Sprites se pueden mover mediante el Transform o mediante Físicas. En esta práctica seguimos moviendo objetos utilizando el Transform.

Recursos:
- https://opengameart.org/
- http://untamed.wild-refuge.net/rpgxp.php
- Sprites 2D en la Asset Store

Realiza las actividades incluidas en el guión de la práctica.

---

## Archivos entregados

- Docs/
  - demo-desplazamiento-horizontal.gif  
- PRACTICA.md  
- Scenes/
  - SampleScene.unity  
  - SampleScene.unity.meta  
- Scripts/
  - MovimientoPersonaje2D.cs  
  - MovimientoPersonaje2D.cs.meta  
- Sprites/
  - idle-Back.anim  
  - idle-Back.anim.meta  
  - Idle-Front.anim  
  - Idle-Front.anim.meta  
  - RobotAController.controller  
  - RobotAController.controller.meta  
  - RobotSheet.png  
  - RobotSheet.png.meta  
  - walk-Left.anim  
  - walk-Left.anim.meta  
  - walk-Panic.anim  
  - walk-Panic.anim.meta  

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
