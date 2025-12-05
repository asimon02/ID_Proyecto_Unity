using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {
    public float speed = 3f;
    private Rigidbody2D rb2D;
    private float move;
    public float jumpForce = 6.5f;
    private bool isGrounded;
    public Transform groundCheck;
    public float groundRadius = 0.1f;
    public LayerMask groundLayer;
    private Animator animator;
    private static int fuegosActuales = 0;
    public GameObject[] fuegoFatuosUI;
    public AudioSource audioSource;
    public AudioClip fuegoFatuoClip;
    private bool isPlayer1;
    private float playerScale;
    private bool ghostMode = false; 

    void Start() {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        
        if (gameObject.CompareTag("Player1")) {
            isPlayer1 = true;
            playerScale = 0.25f;
        } else if (gameObject.CompareTag("Player2")) {
            isPlayer1 = false;
            playerScale = 0.5f;
        }
        if (fuegoFatuosUI == null || fuegoFatuosUI.Length == 0)
        {
            GameObject container = GameObject.Find("FuegoContainer");
            if (container != null)
            {
                fuegoFatuosUI = new GameObject[3];
                for (int i = 0; i < 3; i++)
                {
                    fuegoFatuosUI[i] = container.transform.Find("FuegoFatuo" + (i + 1)).gameObject;
                    fuegoFatuosUI[i].SetActive(false);
                }
            }
            else Debug.LogError("No se encontró 'FuegoContainer' en la escena.");
        }
    }

   void Update() {
        move = 0;

        if (isPlayer1) {
            // Player1: A, W, D
            if (Input.GetKey(KeyCode.A)) move = -1;
            if (Input.GetKey(KeyCode.D)) move = 1;

            if (Input.GetKeyDown(KeyCode.W) && isGrounded) {
                rb2D.linearVelocity = new Vector2(rb2D.linearVelocity.x, jumpForce);
            }

            // Player1 solo puede activar el modo fantasma, pero no desactivarlo
            if (Input.GetKeyDown(KeyCode.Q) && ghostMode == false) {
                ghostMode = true;
                gameObject.layer = LayerMask.NameToLayer("Ghost");
                Debug.Log("Player1 pasó a modo fantasma");
            }

        } else {
            // Player2: flechas
            if (Input.GetKey(KeyCode.LeftArrow)) move = -1;
            if (Input.GetKey(KeyCode.RightArrow)) move = 1;

            if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded) {
                rb2D.linearVelocity = new Vector2(rb2D.linearVelocity.x, jumpForce);
            }
            // Player2 puede devolver a Player1 a modo normal
            if (Input.GetKeyDown(KeyCode.R)) {
                GameObject p1 = GameObject.FindGameObjectWithTag("Player1");

                if (p1 != null) {
                    PlayerController pc = p1.GetComponent<PlayerController>();

                    pc.ghostMode = false;
                    p1.layer = LayerMask.NameToLayer("Player1");
                    Debug.Log("Player2 devolvió a Player1 a modo normal");
                }
            }
        }

        rb2D.linearVelocity = new Vector2(move * speed, rb2D.linearVelocity.y);

        if (move != 0) {
            float scaleX = Mathf.Sign(move) * playerScale;
            transform.localScale = new Vector3(scaleX, playerScale, playerScale);
        }

        animator.SetFloat("Speed", Mathf.Abs(move));
        animator.SetFloat("VerticalVelocity", rb2D.linearVelocity.y);
        animator.SetBool("isGrounded", isGrounded);
    }

    private void FixedUpdate() {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        Debug.Log("Colisión con: " + collision.name);

        if (collision.CompareTag("Coin")) {
            Debug.Log("Moneda recogida. Fuegos actuales: " + fuegosActuales);

            Destroy(collision.gameObject);

            if (fuegosActuales < fuegoFatuosUI.Length) {
                Debug.Log("Activando: " + fuegoFatuosUI[fuegosActuales].name);
                fuegoFatuosUI[fuegosActuales].SetActive(true);
                fuegosActuales++;
            }
        }

        if (collision.CompareTag("Death")) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
