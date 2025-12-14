using UnityEngine;
using UnityEngine.SceneManagement;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [Header("Panel de pausa")]
    public GameObject pauseMenuPanel;

    private bool isPaused = false;
    private TimerController timer;

    void Start()
    {
        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(false);

        // Buscar el TimerController en la escena
        timer = TimerController.FindFirstObjectByType<TimerController>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f; // Pausar el juego
        pauseMenuPanel.SetActive(true);

        // Pausar timer si existe
        if (timer != null)
            timer.PauseTimer();
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f; // Reanuda el juego
        pauseMenuPanel.SetActive(false);

        // Reanudar timer si existe
        if (timer != null)
            timer.ResumeTimer();
    }

    // Salir al selector sin guardar
    public void ExitToSelector()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Selector");
    }

    // Salir del juego
    public void QuitGame()
    {
        Time.timeScale = 1f;
        Application.Quit();
        Debug.Log("Quit Game"); // Solo visible en editor
    }
}
