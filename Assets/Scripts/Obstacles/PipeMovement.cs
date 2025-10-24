using UnityEngine;

public class PipeMovement : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float deadZone = -5f;

    void Update()
    {
        // Move pipe left every frame, frame-rate independent
        transform.position += Vector3.left * moveSpeed * Time.deltaTime;

        // When pipe goes off screen, destroy it
        if (transform.position.x < deadZone)
        {
            Destroy(gameObject);
        }
    }
}
