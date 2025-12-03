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
    private static int fuegosFatuos;
    public TMP_Text textFuegoFatuo;
    public AudioSource audioSource;
    public AudioClip fuegoFatuoClip;
    private bool isPlayer1;
    private float playerScale;

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
        } else {
            // Player2: Flechas
            if (Input.GetKey(KeyCode.LeftArrow)) move = -1;
            if (Input.GetKey(KeyCode.RightArrow)) move = 1;
            
            if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded) {
                rb2D.linearVelocity = new Vector2(rb2D.linearVelocity.x, jumpForce);
            }
        }

        rb2D.linearVelocity = new Vector2(move * speed, rb2D.linearVelocity.y);

        if(move != 0) {
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
        if(collision.transform.CompareTag("Coin")) {
            audioSource.PlayOneShot(fuegoFatuoClip);
            Destroy(collision.gameObject);
            fuegosFatuos++;
            textFuegoFatuo.text = fuegosFatuos.ToString();
        }

        if(collision.transform.CompareTag("Death")) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
