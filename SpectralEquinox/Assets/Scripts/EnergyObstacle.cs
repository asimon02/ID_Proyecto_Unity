using UnityEngine;

public class EnergyObstacle : MonoBehaviour
{
    // Este script sirve para identificar el objeto como un obstáculo de energía.
    // Al ser destruido, liberará el camino.
    
    private void OnDestroy() {
        Debug.Log("EnergyObstacle destruido.");
    }
}
