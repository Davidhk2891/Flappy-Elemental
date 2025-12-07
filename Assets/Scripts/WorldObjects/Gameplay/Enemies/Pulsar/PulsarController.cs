using UnityEngine;

public class PulsarController : BaseObjectController
{
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
        transform.localScale = Vector3.one * balancer.pulsarBaseScale;
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
        targetScale = Random.Range(balancer.pulsarMinPulseScale, balancer.pulsarMaxPulseScale);
    }

    private void Grow()
    {
        // Grow quickly
        transform.localScale = Vector3.MoveTowards(
            transform.localScale,
            Vector3.one * targetScale,
            balancer.pulsarGrowSpeed * Time.deltaTime
        );

        if (transform.localScale.x >= targetScale - 0.01){
            state = PulseState.Holding;
            holdTimer = balancer.pulsarHoldDuration;
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
            Vector3.one * balancer.pulsarBaseScale,
            balancer.pulsarShrinkSpeed * Time.deltaTime
        );

        if (transform.localScale.x <= balancer.pulsarBaseScale + 0.01f)
        {
            // Next heartbeat -> pick new random peak
            chooseRandomPeak();
            state = PulseState.Growing;
        }
    }
}
