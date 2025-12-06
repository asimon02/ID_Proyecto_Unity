using UnityEngine;

public class PuertaController : MonoBehaviour
{
    private Animator animator;
    private bool isOpen = false;

    // Nombres de los estados en el Animator
    private const string IDLE_STATE = "IdlePuerta";
    private const string OPEN_STATE = "Open";
    private const string CLOSE_STATE = "Close";

    void Start()
    {
        animator = GetComponent<Animator>();
        
        if (animator == null)
        {
            Debug.LogError("No se encontró Animator en la Puerta. Asegúrate de que tiene un Animator Component.");
        }
    }

    // Método para abrir la puerta
    public void OpenDoor()
    {
        if (!isOpen && animator != null)
        {
            isOpen = true;
            animator.Play(OPEN_STATE, 0, 0f);
            Debug.Log("Puerta abierta");
        }
    }

    // Método para cerrar la puerta
    public void CloseDoor()
    {
        if (isOpen && animator != null)
        {
            isOpen = false;
            animator.Play(CLOSE_STATE, 0, 0f);
            Debug.Log("Puerta cerrada");
        }
    }

    // Método para alternar entre abrir/cerrar
    public void ToggleDoor()
    {
        if (isOpen)
        {
            CloseDoor();
        }
        else
        {
            OpenDoor();
        }
    }

    // Verificar si la puerta está abierta
    public bool IsOpen()
    {
        return isOpen;
    }
}
