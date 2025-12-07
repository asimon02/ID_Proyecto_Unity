using UnityEngine;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;

public class PuertaController : MonoBehaviour
{
    private Animator animator;
    private bool isOpen = false;
    private HashSet<GameObject> playersInDoor = new HashSet<GameObject>();
    private bool isResetting = false;

    // Nombres de los estados en el Animator
    private const string IDLE_STATE = "IdlePuerta";
    private const string OPEN_STATE = "Open";
    private const string CLOSE_STATE = "Close";

    void Start()
    {
        animator = GetComponent<Animator>();
        
        if (animator == null)
        {
            Debug.LogError("No se encontró Animator en la Puerta. Asegúrate de que tiene un Animator Component.");
        }
    }

    // Método para abrir la puerta
    public void OpenDoor()
    {
        if (!isOpen && animator != null)
        {
            isOpen = true;
            animator.Play(OPEN_STATE, 0, 0f);
            animator.Play(OPEN_STATE, 0, 0f);
            Debug.Log("Puerta abierta");
            CheckWinCondition();
        }
    }

    // Método para cerrar la puerta
    public void CloseDoor()
    {
        if (isOpen && animator != null)
        {
            isOpen = false;
            animator.Play(CLOSE_STATE, 0, 0f);
            Debug.Log("Puerta cerrada");
        }
    }

    // Método para alternar entre abrir/cerrar
    public void ToggleDoor()
    {
        if (isOpen)
        {
            CloseDoor();
        }
        else
        {
            OpenDoor();
        }
    }

    // Verificar si la puerta está abierta
    public bool IsOpen()
    {
        return isOpen;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player1") || other.CompareTag("Player2"))
        {
            playersInDoor.Add(other.gameObject);
            CheckWinCondition();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player1") || other.CompareTag("Player2"))
        {
            playersInDoor.Remove(other.gameObject);
        }
    }

    private void CheckWinCondition()
    {
        if (isOpen && playersInDoor.Count >= 2 && !isResetting)
        {
            // Check if Player1 is in ghost mode
            foreach (GameObject player in playersInDoor)
            {
                if (player.CompareTag("Player1"))
                {
                    PlayerController pc = player.GetComponent<PlayerController>();
                    if (pc != null && pc.IsInGhostMode())
                    {
                        Debug.Log("Player1 is in ghost mode. Cannot win yet.");
                        return;
                    }
                }
            }

            Debug.Log("Both players in open door and Player1 is alive. Starting reset sequence...");
            StartCoroutine(ResetSequence());
        }
    }

    private IEnumerator ResetSequence()
    {
        isResetting = true;
        
        // Initial delay before fade starts
        yield return new WaitForSeconds(1f);
        
        float fadeDuration = 1f;
        float elapsedTime = 0f;

        // Get sprite renderers
        List<SpriteRenderer> renderers = new List<SpriteRenderer>();
        foreach (GameObject player in playersInDoor)
        {
            if (player != null)
            {
                SpriteRenderer sr = player.GetComponent<SpriteRenderer>();
                if (sr != null) renderers.Add(sr);
            }
        }

        // Fade out loop
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);

            foreach (var sr in renderers)
            {
                if (sr != null)
                {
                    Color c = sr.color;
                    c.a = alpha;
                    sr.color = c;
                }
            }
            yield return null;
        }

        // Ensure completely transparent
        foreach (var sr in renderers)
        {
            if (sr != null)
            {
                Color c = sr.color;
                c.a = 0f;
                sr.color = c;
            }
        }

        // Optional extra delay after fade
        yield return new WaitForSeconds(1f);

        // Restart
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
