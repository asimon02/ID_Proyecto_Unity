using UnityEngine;
using UnityEngine.SceneManagement;

public class TestButton : MonoBehaviour
{
    public void CompleteLevel1With3Stars()
    {
        GameManager.Instance.SaveLevelProgress(0, 3);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public void CompleteLevel2With2Stars()
    {
        GameManager.Instance.SaveLevelProgress(1, 2);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public void ResetAllProgress()
    {
        GameManager.Instance.ResetProgress();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}