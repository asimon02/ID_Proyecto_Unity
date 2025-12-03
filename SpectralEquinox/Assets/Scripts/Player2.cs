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
    private int fuegosFatuos;
    public TMP_Text textFuegoFatuo;
    public AudioSource audioSource;
    public AudioClip fuegoFatuoClip;

    void Start() {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
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
