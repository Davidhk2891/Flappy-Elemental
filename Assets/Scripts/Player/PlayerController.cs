using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour
{
    [Header("Slime Sprites")]
    public Sprite[] slimeAnimation = new Sprite[4];
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private bool isAnimating = false;
    private GameBalancer balancer;
    private RunSessionManager run;
    private bool playerIsAlive = false;

    // Runs 1st
    void Awake()
    {
        // Cache references
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Runs 2nd
    void OnEnable()
    {
        // Nothing that relies on balancer should be here
        playerIsAlive = true;
    }

    // Runs 3rd
    void Start()
    {
        // Since PlayerController relies on balancer. Load balancer last
        balancer = GameSettingsManager.Instance.balancer;
        run = RunSessionManager.Instance;   

        rb.gravityScale = balancer.playerGravity;
        sr.sprite = slimeAnimation[0];
    }

    // Runs 4th
    void Update()
    {
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

        TrackDistance();
        PrintOrbsAndDistance();
    }

    private void PrintOrbsAndDistance()
    {
        Debug.Log($"Depth: {run.distanceTraveled}");
        Debug.Log($"Orbs: {run.orbsCollected}");
    }

    private void TrackDistance()
    {
        if (!playerIsAlive) return;

        float worldSpeed = balancer.globalWorldSpeed;

        // Distance = speed * deltaTime
        float distanceThisFrame = worldSpeed * Time.deltaTime;

        run.AddDistance(distanceThisFrame);
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
        GameOver();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if trigger has tag Boundry
        if (other.gameObject.CompareTag("Boundary"))
        {
            GameOver();
        }
    }
    
    void GameOver()
    {
        playerIsAlive = false;
        run.ResetSession();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
