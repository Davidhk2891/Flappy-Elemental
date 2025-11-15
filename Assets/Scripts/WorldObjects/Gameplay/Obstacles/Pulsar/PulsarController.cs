using Unity.VisualScripting;
using UnityEngine;

public class PulsarController : MonoBehaviour
{

    [Header("Pulse Settings")]
    // Reference to PulsarSprite transform
    public Transform sprite;
    
    // Resting size
    public float baseScale = 1f;
    // Minimum spike
    public float minPulseScale = 0.5f;
    // Maximum spike
    public float maxPulseScale = 1f;

    // Fast expansion
    public float growSpeed = 8f;
    // Slow contraction
    public float shrinkSpeed = 2f;
    // Time it stays big
    public float holdDuration = 0.25f;

    private float targetScale;
    private float holdTimer;
    private enum PulseState
    {
        Growing,
        Holding,
        Shrinking
    }
    private PulseState state = PulseState.Growing;

    void Start()
    {
        // Pick first random pulse size
        targetScale = Random.Range(minPulseScale, maxPulseScale);
        sprite.localScale = Vector3.one * baseScale;
    }

    void Update()
    {
        switch (state)
        {
            case PulseState.Growing:
                Grow();
                break;
            case PulseState.Holding:
                Hold();
                break;
            case PulseState.Shrinking:
                Shrink();
                break;
        }
    }

    private void Grow()
    {
        
    }

    private void Hold()
    {
        
    }

    private void Shrink()
    {
        
    }
}
