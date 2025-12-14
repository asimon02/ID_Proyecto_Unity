using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    // Guardar progreso del nivel
    public void SaveLevelProgress(int levelIndex, int stars)
    {
        // Guardar si el nivel está desbloqueado
        PlayerPrefs.SetInt("Level_" + levelIndex + "_Unlocked", 1);
        
        // Guardar el número de estrellas (solo si es mayor que el anterior)
        int currentStars = PlayerPrefs.GetInt("Level_" + levelIndex + "_Stars", 0);
        if (stars > currentStars)
        {
            PlayerPrefs.SetInt("Level_" + levelIndex + "_Stars", stars);
        }
        
        // Desbloquear el siguiente nivel
        PlayerPrefs.SetInt("Level_" + (levelIndex + 1) + "_Unlocked", 1);
        
        PlayerPrefs.Save();
    }
    
    // Obtener número de estrellas de un nivel
    public int GetLevelStars(int levelIndex)
    {
        return PlayerPrefs.GetInt("Level_" + levelIndex + "_Stars", 0);
    }
    
    // Verificar si un nivel está desbloqueado
    public bool IsLevelUnlocked(int levelIndex)
    {
        // El nivel 0 siempre está desbloqueado
        if (levelIndex == 0) return true;
        return PlayerPrefs.GetInt("Level_" + levelIndex + "_Unlocked", 0) == 1;
    }
    
    // Reiniciar todo el progreso
    public void ResetProgress()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }

    public void LoadMainMenu(){
        SceneManager.LoadScene("MenuInicial");
    }
}