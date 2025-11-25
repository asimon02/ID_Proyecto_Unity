# ID_Proyecto_Unity 🎮
## Proyecto Unity de la asignatura de Imagen Digital (ID)

Este es un juego **cooperativo local de plataformas y puzles** para dos jugadores. Los jugadores asumen roles asimétricos, representando las entidades de la **Vida** y la **Muerte**, y deben combinar sus habilidades únicas para superar obstáculos.

---

## 👥 Jugadores y Roles

* **Jugador 1: VIDA (Luz)**
* **Jugador 2: MUERTE (Sombra)**

---

## ✨ Funcionalidades y Habilidades

Las mecánicas de juego se dividen en habilidades específicas de cada rol y funcionalidades generales del nivel.

### 🌟 Rol: VIDA

| Funcionalidad | Descripción | Notas |
| :--- | :--- | :--- |
| **Revivir a Muerte** | La Vida puede devolver a la Muerte al juego "morir"|
| **Lanza Rayo de Luz** | Proyecta un rayo que puede interactuar con ciertos elementos del puzle |
| **Bola de Luz Pesada** | Habilidad temporal que permite a la Vida generar un objeto pesado para activar mecanismos o presionar botones |

---

### 👻 Rol: MUERTE

| Funcionalidad | Descripción | Notas |
| :--- | :--- | :--- |
| **Muere (Atraviesa Paredes)** | La Muerte puede "morir" de forma controlada para atravesar paredes o barreras inaccesibles para la Vida|
| **Checkpoint** | La Muerte puede marcar checkpoint y reaparecer ahi cuando quiera |
| **Teletransporte (Tumbas)**| Puede teletransportarse entre tumbas |

---

### ⚙️ Funcionalidades Generales (Nivel e Interacción)

| Funcionalidad | Descripción | Notas |
| :--- | :--- | :--- |
| **Activar Palanca** | Elemento de puzle interactivo |
| **Activar Botones** | Mecanismos de presión que pueden ser activados por cualquiera de los jugadores o por la **Bola de Luz Pesada** de Vida |
| **Puertas (Cárcel) con Llaves** | Puertas cerradas que requieren una **Llave** específica para ser abiertas|
| **Monedas (Fuego Fatuo)** | Coleccionables dispersos por el nivel |
| **Puerta Final de Cada Nivel**| Entrada al siguiente nivel. Requiere que ambos jugadores estén presentes |
| **Estatua Final del Juego** | Punto de objetivo final del último nivel |

---

### ☠️ Condiciones de Muerte y Penalización

Las siguientes condiciones resultan en la muerte de cualquiera de los jugadores (excepto la "muerte" controlada de Muerte):

* **Muerte por Caer en el Vacío**
* **Muerte por Caer en Neblina / Agua Tóxica**
* **Muerte por Pinchos u Obstáculos Peligrosos**

**Penalización:** Empiezan desde el principio ese nivel.
---

## 📝 Próximos Pasos / TO-DO
