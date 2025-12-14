using UnityEngine;

public class BolaLuzController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 1. Verificar si chocamos con un obstáculo de energía
        EnergyObstacle obstacle = collision.GetComponent<EnergyObstacle>();
        
        if (obstacle != null)
        {
            Debug.Log("Bola de luz golpeó el obstáculo de energía. Iniciando explosión.");
            
            // Llamar al método de explosión del obstáculo para activar la animación
            obstacle.Explode();
            
            // Destruir la bola de luz (este proyectil) inmediatamente
            Destroy(this.gameObject);
        }
        // 2. Opcional: Destruirse si choca con paredes (Layer "Ground" o "Wall")
        // else if (collision.gameObject.layer == LayerMask.NameToLayer("Ground")) {
        //     Destroy(this.gameObject);
        // }
    }
}
