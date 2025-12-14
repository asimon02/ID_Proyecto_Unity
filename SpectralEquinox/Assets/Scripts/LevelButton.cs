using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelButton : MonoBehaviour
{
    [Header("Configuración")]
    public int levelIndex;
    public string sceneName;
    
    [Header("Referencias UI")]
    public Button button;
    public GameObject lockIcon;
    public GameObject[] stars;
    public RawImage levelImage;

    private void Start()
    {
        UpdateButtonState();
        button.onClick.AddListener(LoadLevel);
    }
    
    private void UpdateButtonState()
    {
        bool unlocked = GameManager.Instance.IsLevelUnlocked(levelIndex);

        // Activar o desactivar el botón
        button.interactable = unlocked;

        // Mostrar/ocultar candado
        if (lockIcon != null)
            lockIcon.SetActive(!unlocked);

        // Cambiar color de la imagen según estado
        if (levelImage != null)
            {
                Color c = levelImage.color;
                c.a = unlocked ? 1f : 0.1f;
                levelImage.color = c;
            }

        // Mostrar estrellas si está desbloqueado
        if (unlocked)
        {
            int starsEarned = GameManager.Instance.GetLevelStars(levelIndex);
            for (int i = 0; i < stars.Length; i++)
                stars[i].SetActive(i < starsEarned);
        }
        else
        {
            foreach (GameObject star in stars)
                star.SetActive(false);
        }
    }
    
    private void LoadLevel()
    {
        SceneManager.LoadScene(sceneName);
    }
}