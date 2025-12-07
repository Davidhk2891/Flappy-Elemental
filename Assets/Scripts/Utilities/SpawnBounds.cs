using UnityEngine;

public class SpawnBounds : MonoBehaviour
{
    public float BottomLimit { get; private set; }
    public float TopLimit { get; private set; }

    [SerializeField] private Collider2D floorCollider;
    [SerializeField] private Collider2D ceilingCollider;
    [SerializeField] private float buffer = 1.5f;

    private void Awake()
    {
        Camera cam = Camera.main;

        float topViewportY = cam.transform.position.y + cam.orthographicSize;
        float bottomViewportY = cam.transform.position.y - cam.orthographicSize;

        float floorHeight = floorCollider.bounds.size.y;
        float ceilingHeight = ceilingCollider.bounds.size.y;

        // Correct: Use floor height at the bottom
        BottomLimit = bottomViewportY + floorHeight + buffer;

        // Correct: Use ceiling height at the top
        TopLimit = topViewportY - ceilingHeight - buffer;
    }
}