using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player2 : MonoBehaviour {
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

    void Start() {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
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

        if (Input.GetKey(KeyCode.LeftArrow))  move = -1;
        if (Input.GetKey(KeyCode.RightArrow)) move = 1;

        rb2D.linearVelocity = new Vector2(move * speed, rb2D.linearVelocity.y);

        if(move != 0) {
            float scaleX = Mathf.Sign(move) * 0.5f;
            transform.localScale = new Vector3(scaleX, 0.5f, 0.5f);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded) {
            rb2D.linearVelocity = new Vector2(rb2D.linearVelocity.x, jumpForce);
        }

        animator.SetFloat("Speed", Mathf.Abs(move));
        animator.SetFloat("VerticalVelocity", rb2D.linearVelocity.y);
        animator.SetBool("isGrounded", isGrounded);
    }

    private void FixedUpdate() {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        Debug.Log("Colisión con: " + collision.name); // Muestra el nombre del objeto con el que colisiona

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
