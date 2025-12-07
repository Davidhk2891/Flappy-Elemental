using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Header("Slime Sprites")]
    public Sprite[] slimeAnimation = new Sprite[4];
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private bool isAnimating = false;
    private GameBalancer balancer;

    void Awake()
    {
        // Cache references
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void OnEnable()
    {
        // Apply dynamic values (like gravity)
        rb.gravityScale = balancer.playerGravity;
        sr.sprite = slimeAnimation[0];
        Debug.Log("OnEnable fired");
    }

    void Start()
    {
        // Since PlayerController relies on balancer. Load balancer last
        balancer = GameSettingsManager.Instance.balancer;
    }

    void Update()
    {
        Debug.Log("Update running. Player enabled = " + gameObject.activeSelf);
        bool tap = false;

        // Keyboard (for PC testing)
        if (Input.GetKeyDown(KeyCode.Space))
            tap = true;

        // Touch (for phone)
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            tap = true;

        if (tap && !isAnimating)
        {
            rb.linearVelocity = Vector2.up * balancer.playerJumpForce;
            StartCoroutine(FlapAnimation());
        }
    }

    private IEnumerator FlapAnimation()
    {
        isAnimating = true;

        float frameTime = balancer.playerSpriteJumpDuration / slimeAnimation.Length;

        for (int i = 1; i < slimeAnimation.Length; i++)
        {
            sr.sprite = slimeAnimation[i];
            yield return new WaitForSeconds(frameTime);
        }

        // Return to idle sprite
        sr.sprite = slimeAnimation[0];

        isAnimating = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Player collided with; " + collision.gameObject.name);
        GameOver();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Log for debuging
        Debug.Log($"Player entered trigger: {other.gameObject.name}");

        // Check if trigger has tag Boundry
        if (other.gameObject.CompareTag("Boundary"))
        {
            Debug.Log("Player hit floor or ceiling. Restarting game");
            GameOver();
        }
    }
    
    void GameOver()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
