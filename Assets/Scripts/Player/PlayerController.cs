using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    public float flapStrength = 5f;

    void Start()
    {
        // Get the Rigidbody 2D component attached to this GameObject
        rigidBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Listens for space bar press. When pressed, y * flapStrength
            Debug.Log("Jumped");
            rigidBody.linearVelocity = Vector2.up * flapStrength;
        }
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
