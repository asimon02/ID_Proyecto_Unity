using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelSummaryUI : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject panel;
    public TextMeshProUGUI nivelCompletadoText;
    public TextMeshProUGUI tiempoText;
    public TextMeshProUGUI fuegosText;
    public TextMeshProUGUI bolasText;
    public Button continuarButton;

    private void Awake()
    {
        panel.SetActive(false);
        continuarButton.onClick.AddListener(OnContinuarClicked);
    }

    public void ShowSummary(float tiempo, int fuegos, int bolas)
    {
        panel.SetActive(true);

        nivelCompletadoText.text = "Nivel Completado";
        tiempoText.text = "Tiempo: " + tiempo.ToString("F2") + "s";
        fuegosText.text = "Fuegos fatuos conseguidos: " + fuegos;
        bolasText.text = "Bolas de luz lanzadas: " + bolas;
    }

    private void OnContinuarClicked()
{
    panel.SetActive(false); // Oculta la ventana
}
}
