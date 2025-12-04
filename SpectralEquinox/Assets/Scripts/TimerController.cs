using TMPro;
using UnityEngine;

public class TimerController : MonoBehaviour
{
    public TMP_Text timerText;
    private float elapsedTime = 0f; 
    private bool isRunning = true;

    void Start()
    {
        if (timerText != null)
            timerText.text = "00:00";
    }
    void Update()
    {
        if (!isRunning) return;

        elapsedTime += Time.deltaTime;

        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void PauseTimer()
    {
        isRunning = false;
    }

    public void ResetTimer()
    {
        elapsedTime = 0f;
        isRunning = true;
    }
}
