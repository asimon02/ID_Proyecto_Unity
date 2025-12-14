using UnityEngine;

public class EnergyObstacle : MonoBehaviour
{
    private Animator animator;
    private Collider2D col;

    void Start()
    {
        animator = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
    }

    // Este método será llamado por la Bola de Luz
    public void Explode()
    {
        // 1. Desactivar el collider para que ya no sea un obstáculo físico
        if (col != null)
        {
            col.enabled = false;
        }

        // 2. Activar la animación de explosión
        if (animator != null)
        {
            animator.SetTrigger("explosion_idle");
            
            // 3. Destruir el objeto después de que termine la animación
            // Ajusta este tiempo (0.5f) a la duración real de tu animación
            Destroy(gameObject, 0.5f); 
        }
        else
        {
            // Si no hay animator, destruir inmediatamente
            Destroy(gameObject);
        }
    }

    private void OnDestroy() {
        Debug.Log("EnergyObstacle destruido.");
    }
}
