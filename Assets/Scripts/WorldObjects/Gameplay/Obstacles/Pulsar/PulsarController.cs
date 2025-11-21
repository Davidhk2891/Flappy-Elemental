using UnityEngine;

public class PulsarController : BaseObjectController
{

    [Header("Pulse Settings")]
    // Resting size
    public float baseScale = 0.5f;
    // Minimum spike
    public float minPulseScale = 0.5f;
    // Maximum spike
    public float maxPulseScale = 1f;
    // Fast expansion
    public float growSpeed = 10f;
    // Slow contraction
    public float shrinkSpeed = 1.5f;
    // Time it stays big
    public float holdDuration = 1f;
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
        chooseRandomPeak();
        transform.localScale = Vector3.one * baseScale;
    }

    protected override void Update()
    {
        // Inherited leftward movement
        base.Update();

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

    private void chooseRandomPeak()
    {
        targetScale = Random.Range(minPulseScale, maxPulseScale);
    }

    private void Grow()
    {
        // Grow quickly
        transform.localScale = Vector3.MoveTowards(
            transform.localScale,
            Vector3.one * targetScale,
            growSpeed * Time.deltaTime
        );

        if (transform.localScale.x >= targetScale - 0.01){
            state = PulseState.Holding;
            holdTimer = holdDuration;
        }
    }

    private void Hold()
    {
        // Deduct current game time from holdTimer time
        holdTimer -= Time.deltaTime;
        if (holdTimer <= 0)
        {
            state = PulseState.Shrinking;
        }
    }

    private void Shrink()
    {
        // Shrink slowly toward base scale
        transform.localScale = Vector3.MoveTowards(
            transform.localScale,
            Vector3.one * baseScale,
            shrinkSpeed * Time.deltaTime
        );

        if (transform.localScale.x <= baseScale + 0.01f)
        {
            // Next heartbeat -> pick new random peak
            chooseRandomPeak();
            state = PulseState.Growing;
        }
    }
}
