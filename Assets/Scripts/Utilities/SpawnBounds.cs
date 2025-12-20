using UnityEngine;

public class SpawnBounds : MonoBehaviour
{
    public float BottomViewportY { get; private set; }
    public float TopViewportY { get; private set; }
    public float BottomLimit { get; private set; }
    public float TopLimit { get; private set; }

    [SerializeField] private Collider2D floorCollider;
    [SerializeField] private Collider2D ceilingCollider;
    [SerializeField] private float buffer = 2f;

    private void Awake()
    {
        Camera cam = Camera.main;

        TopViewportY = cam.transform.position.y + cam.orthographicSize;
        BottomViewportY = cam.transform.position.y - cam.orthographicSize;

        float floorHeight = floorCollider.bounds.size.y;
        float ceilingHeight = ceilingCollider.bounds.size.y;

        // Correct: Use floor height at the bottom
        BottomLimit = BottomViewportY + floorHeight + buffer;

        // Correct: Use ceiling height at the top
        TopLimit = TopViewportY - ceilingHeight - buffer;
    }
}