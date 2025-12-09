using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class MenuTransitions : MonoBehaviour
{
    [Header("Paneles")]
    public CanvasGroup mainMenu;
    public CanvasGroup optionsMenu;
    public CanvasGroup creditsMenu;

    [Header("Configuración")]
    public float transitionDuration = 0.5f;
    public float slideDistance = 500f; // distancia de deslizamiento

    // Guardar posición inicial de cada panel
    private Vector3 mainMenuPos;
    private Vector3 optionsMenuPos;
    private Vector3 creditsMenuPos;

    private void Start()
    {
        // Guardar posiciones iniciales
        mainMenuPos = mainMenu.transform.localPosition;
        optionsMenuPos = optionsMenu.transform.localPosition;
        creditsMenuPos = creditsMenu.transform.localPosition;

        // Mostrar solo el mainMenu al inicio
        ShowPanel(mainMenu, mainMenuPos);
        optionsMenu.gameObject.SetActive(false);
        creditsMenu.gameObject.SetActive(false);
    }


    public void StartGame()
    {
        SceneManager.LoadScene("Selector");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OpenOptions()
    {
        StartCoroutine(SwitchPanel(mainMenu, mainMenuPos, optionsMenu, optionsMenuPos));
    }

    public void CloseOptions()
    {
        StartCoroutine(SwitchPanel(optionsMenu, optionsMenuPos, mainMenu, mainMenuPos));
    }

    public void OpenCredits()
    {
        StartCoroutine(SwitchPanel(optionsMenu, optionsMenuPos, creditsMenu, creditsMenuPos));
    }

    public void CloseCredits()
    {
        StartCoroutine(SwitchPanel(creditsMenu, creditsMenuPos, optionsMenu, optionsMenuPos));
    }

    private IEnumerator SwitchPanel(CanvasGroup from, Vector3 fromPos, CanvasGroup to, Vector3 toPos)
    {
        // Activar panel nuevo antes de animar
        to.gameObject.SetActive(true);

        // Fade out + slide del panel actual
        StartCoroutine(FadeAndSlide(from, fromPos, fromPos - Vector3.right * slideDistance, false));

        // Fade in + slide del panel nuevo
        yield return StartCoroutine(FadeAndSlide(to, toPos + Vector3.right * slideDistance, toPos, true));
    }

    private IEnumerator FadeAndSlide(CanvasGroup cg, Vector3 start, Vector3 end, bool fadeIn)
    {
        float time = 0f;
        cg.interactable = false;
        cg.blocksRaycasts = false;

        cg.alpha = fadeIn ? 0f : 1f;
        cg.transform.localPosition = start;

        while (time < transitionDuration)
        {
            float t = time / transitionDuration;
            cg.alpha = fadeIn ? t : 1f - t;
            cg.transform.localPosition = Vector3.Lerp(start, end, t);
            time += Time.deltaTime;
            yield return null;
        }

        cg.alpha = fadeIn ? 1f : 0f;
        cg.transform.localPosition = end;
        cg.interactable = fadeIn;
        cg.blocksRaycasts = fadeIn;

        if (!fadeIn) cg.gameObject.SetActive(false);
    }

    private void ShowPanel(CanvasGroup cg, Vector3 pos)
    {
        cg.alpha = 1f;
        cg.interactable = true;
        cg.blocksRaycasts = true;
        cg.transform.localPosition = pos;
    }
}