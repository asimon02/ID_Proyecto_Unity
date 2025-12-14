using UnityEngine;

public class PalancaController : MonoBehaviour
{
    private Animator animator;
    private int currentState = 0; // 0 = Idle, 1 = Activar, 2 = Desactivar
    private bool playerInRange = false;
    
    [Header("Configuración")]
    [Tooltip("Tecla para activar/desactivar la palanca")]
    public KeyCode interactionKey = KeyCode.K;
    
    [Tooltip("Tag del jugador (puede ser Player, Player1 o Player2)")]
    public string playerTag = "Player";

    [Header("Puerta Conectada")]
    [Tooltip("Arrastra aquí el GameObject de la Puerta")]
    public PuertaController puerta;

    // Nombres de los estados en el Animator
    private string[] stateNames = { "IdlePalanca", "Activar", "Desactiivar" };

    void Start()
    {
        animator = GetComponent<Animator>();
        
        if (animator == null)
        {
            Debug.LogError("No se encontró Animator en la Palanca. Asegúrate de que tiene un Animator Component.");
        }

        // Verificar que tiene un Collider2D con Is Trigger activado
        Collider2D col = GetComponent<Collider2D>();
        if (col == null)
        {
            Debug.LogError("La Palanca necesita un Collider2D. Añade un BoxCollider2D o CircleCollider2D.");
        }
        else if (!col.isTrigger)
        {
            Debug.LogWarning("El Collider2D de la Palanca debe tener 'Is Trigger' activado.");
        }
    }

    void Update()
    {
        // Si el jugador está en rango y presiona la tecla
        if (playerInRange && Input.GetKeyDown(interactionKey))
        {
            TogglePalanca();
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        // Verificar si es un jugador (Player, Player1 o Player2)
        if (other.CompareTag(playerTag) || other.CompareTag("Player1") || other.CompareTag("Player2"))
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // Cuando el jugador sale del rango
        if (other.CompareTag(playerTag) || other.CompareTag("Player1") || other.CompareTag("Player2"))
        {
            playerInRange = false;
        }
    }

    void TogglePalanca()
    {
        if (animator != null)
        {
            // Avanzar al siguiente estado
            if (currentState == 0) // De Idle a Activar
            {
                currentState = 1;
                // Abrir la puerta cuando se activa la palanca
                if (puerta != null)
                {
                    puerta.OpenDoor();
                }
            }
            else if (currentState == 1) // De Activar a Desactivar
            {
                currentState = 2;
                // Cerrar la puerta cuando se desactiva la palanca
                if (puerta != null)
                {
                    puerta.CloseDoor();
                }
            }
            else // De Desactivar a Activar
            {
                currentState = 1;
                // Abrir la puerta cuando se vuelve a activar
                if (puerta != null)
                {
                    puerta.OpenDoor();
                }
            }

            // Reproducir la animación correspondiente desde el inicio
            animator.Play(stateNames[currentState], 0, 0f);
            
            Debug.Log("Palanca: " + stateNames[currentState]);
        }
    }

    // Método público para verificar el estado de la palanca
    public bool IsActivated()
    {
        return currentState == 1; // true si está en estado "Activar"
    }

    public int GetCurrentState()
    {
        return currentState;
    }
}
