using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

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
    private SpriteRenderer sr;
    private int originalLayer;

    public Slider ghostSlider;
    public float ghostDrainRateByGhostMode = 5f;
    public Transform bolaSpawnPoint; 
    public GameObject bolaDeLuzPrefab;     
    public float bolaSpeed = 10f;          
    public float ghostDrainByAction = 10f;

    void Start() {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        if (gameObject.CompareTag("Player1")) {
            isPlayer1 = true;
            playerScale = 0.25f;
            originalLayer = LayerMask.NameToLayer("Player1");
        } else if (gameObject.CompareTag("Player2")) {
            isPlayer1 = false;
            playerScale = 0.5f;
            originalLayer = LayerMask.NameToLayer("Player2");
        }

        Transform container = GameObject.Find("FuegoContainer")?.transform;

        if (container == null) {
            Debug.LogError("No se encontró 'FuegoContainer' en la escena");
            return;
        }

        if (ghostSlider == null) {
            ghostSlider = container.Find("GhostSlider")?.GetComponent<Slider>();
            if (ghostSlider == null)
                Debug.LogError("No se encontró 'GhostSlider' dentro de FuegoContainer");
        }

        if (fuegoFatuosUI == null || fuegoFatuosUI.Length == 0) {
            fuegoFatuosUI = new GameObject[3];
            for (int i = 0; i < 3; i++) {
                Transform fuego = container.Find("FuegoFatuo" + (i + 1));
                if (fuego != null) {
                    fuegoFatuosUI[i] = fuego.gameObject;
                    fuegoFatuosUI[i].SetActive(false);
                } else {
                    Debug.LogError("No se encontró FuegoFatuo" + (i + 1));
                }
            }
        }

        // Evitar colisiones entre jugadores usando IgnoreCollision
        SetupPlayerCollisionIgnore();
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

            // Player1 solo puede activar el modo fantasma (Q), pero no desactivarlo
            if (Input.GetKeyDown(KeyCode.Q) && ghostMode == false) {
                ghostMode = true;
                SetLayerRecursively(gameObject, LayerMask.NameToLayer("Ghost"));
                Color c = sr.color;
                c = new Color(0.85f, 0.85f, 0.85f, 0.55f); // Baja la opacidad
                sr.color = c;
                Debug.Log("Player1 pasó a modo fantasma");
            }
            if (ghostMode) {
                ghostSlider.value -= ghostDrainRateByGhostMode * Time.deltaTime;
                if (ghostSlider.value <= 0f) {
                    ghostSlider.value = 0f;
                    ghostSlider.fillRect.gameObject.SetActive(false);
                    ExitGhostMode();
                    Debug.Log("Player1 salió del modo fantasma automáticamente");
                } else
                {
                    ghostSlider.fillRect.gameObject.SetActive(true);
                }
            }
            if (Input.GetKeyDown(KeyCode.S)) {
                float checkRadius = 1.5f; // radio de detección
                Collider2D[] graves = Physics2D.OverlapCircleAll(transform.position, checkRadius);

                Debug.Log("Detectadas " + graves.Length + " colisiones cerca del jugador.");

                // Filtrar solo las que tengan tag "Grave"
                List<Transform> nearbyGraves = new List<Transform>();
                foreach (var g in graves) {
                    if (g.CompareTag("Grave")) {
                        nearbyGraves.Add(g.transform);
                        Debug.Log("Tumba cercana encontrada: " + g.name);
                    }
                }

                if (nearbyGraves.Count == 0) {
                    Debug.Log("No hay tumbas cercanas para usar.");
                    return; // salir si no hay tumbas cerca
                }

                // Elegir otra tumba aleatoria distinta de la actual
                Transform currentGrave = nearbyGraves[0]; // la más cercana
                List<Transform> otherGraves = new List<Transform>();
                foreach (var g in GameObject.FindGameObjectsWithTag("Grave")) {
                    if (g.transform != currentGrave) otherGraves.Add(g.transform);
                }

                if (otherGraves.Count == 0) {
                    Debug.Log("No hay otras tumbas para teletransportarse.");
                    return; // salir si no hay otra tumba
                }

                Transform targetGrave = otherGraves[Random.Range(0, otherGraves.Count)];
                
                if (ghostSlider != null) {
                    if (ghostSlider.value >=10f){
                        transform.position = targetGrave.position;
                    }
                    ghostSlider.value -= ghostDrainByAction;
                    if (ghostSlider.value <= 0f) {
                        ghostSlider.value = 0f;
                        ghostSlider.fillRect.gameObject.SetActive(false);
                    }
                }
                Debug.Log("Player1 se teletransportó a: " + targetGrave.name + " en posición " + targetGrave.position);
            }
        } else {
            // Player2: flechas
            if (Input.GetKey(KeyCode.LeftArrow)) move = -1;
            if (Input.GetKey(KeyCode.RightArrow)) move = 1;

            if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded) {
                rb2D.linearVelocity = new Vector2(rb2D.linearVelocity.x, jumpForce);
            }
            // Player2 puede devolver a Player1 a modo normal
            if (Input.GetKeyDown(KeyCode.P) && ghostMode == false) {
                GameObject p1 = GameObject.FindGameObjectWithTag("Player1");

                if (p1 != null) {
                    PlayerController pc = p1.GetComponent<PlayerController>();
                    pc.ExitGhostMode();
                    Debug.Log("Player2 devolvió a Player1 a modo normal");
                }
            }
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (ghostSlider != null) {
                    if(ghostSlider.value >= 10f) {
                        LanzarBolaDeLuz();
                    }
                    ghostSlider.value -= ghostDrainByAction;
                    if (ghostSlider.value <= 0f)
                    {
                        ghostSlider.value = 0f;
                        ghostSlider.fillRect.gameObject.SetActive(false);
                    } 
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

    public void ExitGhostMode() {
        ghostMode = false;
        SetLayerRecursively(gameObject, originalLayer);
        sr.color = new Color(1f, 1f, 1f, 1f); // vuelve a opacidad normal
    }

    private void SetLayerRecursively(GameObject obj, int layer) {
        if (obj == null) return;
        obj.layer = layer;
        foreach (Transform child in obj.transform) {
            SetLayerRecursively(child.gameObject, layer);
        }
    }

    private void SetupPlayerCollisionIgnore() {
        // Obtener todos los colliders de este jugador
        Collider2D[] myColliders = GetComponentsInChildren<Collider2D>();
        
        // Buscar el otro jugador
        GameObject otherPlayer = null;
        if (isPlayer1) {
            otherPlayer = GameObject.FindGameObjectWithTag("Player2");
        } else {
            otherPlayer = GameObject.FindGameObjectWithTag("Player1");
        }
        
        if (otherPlayer != null) {
            // Obtener todos los colliders del otro jugador
            Collider2D[] otherColliders = otherPlayer.GetComponentsInChildren<Collider2D>();
            
            // Ignorar colisiones entre todos los pares de colliders
            foreach (Collider2D myCol in myColliders) {
                foreach (Collider2D otherCol in otherColliders) {
                    Physics2D.IgnoreCollision(myCol, otherCol, true);
                    Debug.Log($"Ignorando colisión entre {myCol.name} y {otherCol.name}");
                }
            }
        }
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

    void LanzarBolaDeLuz()
    {
        if (bolaDeLuzPrefab == null || bolaSpawnPoint == null) return;

        // Instanciar la bola en el spawn point
        GameObject bola = Instantiate(bolaDeLuzPrefab, bolaSpawnPoint.position, Quaternion.identity);
        bola.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

        Rigidbody2D rb = bola.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // Dirección según hacia donde mira el personaje
            float direccion = transform.localScale.x > 0 ? 1f : -1f;
            rb.linearVelocity = new Vector2(direccion * bolaSpeed, 0f);
        }

        Destroy(bola, 3f); // se destruye tras 3 segundos
    }

}