using UnityEngine;

public class PulsarController : MonoBehaviour
{

    [Header("Pulse Settings")]
    // Reference to PulsarSprite transform
    public Transform sprite;
    // Smallest size
    public float minScale = 0.8f;
    // Largest size
    public float maxScale = 1.5f;
    // Pulse speed
    public float pulseSpeed = 6f;
    
    // Pulse timer
    private float pulseTimer = 0f;

    void Update()
    {
        Pulse();
    }

    private void Pulse()
    {
        pulseTimer += Time.deltaTime * pulseSpeed;

        // Sine oscillation: 0 -> 1 -> 0 - > 1...
        float t = (Mathf.Sin(pulseTimer) + 1f) * 0.5f;

        float scale = Mathf.Lerp(minScale, maxScale, t);

        sprite.localScale = new Vector3(scale, scale, 1f);
    }
}
