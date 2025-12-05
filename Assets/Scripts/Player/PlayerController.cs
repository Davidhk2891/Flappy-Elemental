using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    [Header("Flap settings")]
    public float flapStrength = 10f;
    public float flapDuration = 0.15f;
    [Header("Slime Sprites")]
    public Sprite[] slimeAnimation = new Sprite[4];
    private bool isAnimating = false;

    void Start()
    {
        // Get the Rigidbody 2D component attached to this GameObject
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = slimeAnimation[0];
    }

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
            rb.linearVelocity = Vector2.up * flapStrength;
            StartCoroutine(FlapAnimation());
        }
    }

    private IEnumerator FlapAnimation()
    {
        isAnimating = true;

        float frameTime = flapDuration / slimeAnimation.Length;

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
