using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    public float flapStrength = 10f;

    [Header("Kirby Sprites")]
    public Sprite kirbyIdle;
    public Sprite kirbyFlap;
    public float flapDuration = 0.15f;

    void Start()
    {
        // Get the Rigidbody 2D component attached to this GameObject
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = kirbyIdle;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Listens for space bar press. When pressed, y * flapStrength
            rb.linearVelocity = Vector2.up * flapStrength;
            StartCoroutine(FlapAnimation());
        }
    }

    private IEnumerator FlapAnimation()
    {
        sr.sprite = kirbyFlap;
        yield return new WaitForSeconds(flapDuration);
        sr.sprite = kirbyIdle;
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
