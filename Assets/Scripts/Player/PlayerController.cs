using UnityEngine;

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
}
