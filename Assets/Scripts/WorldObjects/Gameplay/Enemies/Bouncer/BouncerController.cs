using System.Collections;
using UnityEngine;

/*
- Moves the object up and down around its starting position
- Moves at a fixed speed
- Using Time.deltaTime (so it's smooth)
- Keeps all the movement on the parent (not the child)
- Scrolls left at a fixed rate
- Deactivates itself when off-screen (ready for pooling)
This will be the vase for all animated obstacles later
*/
public class BouncerController : BaseObjectController
{
    public Sprite[] bouncerAnimation = new Sprite[3];
    private SpriteRenderer sr;
    private bool movingUp = true;
    private float topLimit;
    private float bottomLimit;
    private Coroutine bouncerAnimCoroutine;

    protected override void OnEnable()
    {
        base.OnEnable();

        sr = GetComponentInChildren<SpriteRenderer>();
        sr.sprite = bouncerAnimation[0];

        // Pick fixed boundries
        topLimit = balancer.bouncerTopLimit[Random.Range(0, balancer.bouncerTopLimit.Length)];
        bottomLimit = balancer.bouncerBottomLimit[Random.Range(0, balancer.bouncerBottomLimit.Length)];

        if (bouncerAnimCoroutine != null)
            StopCoroutine(bouncerAnimCoroutine);
        
        bouncerAnimCoroutine = StartCoroutine(BouncerMovementAnimation());
    }

    private void OnDisable()
    {
        if (bouncerAnimCoroutine != null)
        {
            StopCoroutine(bouncerAnimCoroutine);
            bouncerAnimCoroutine = null;
        }
    }

    protected override void Update()
    {
        // Leftward motion + off-screen kill check
        base.Update();
        // Own child behaviour
        VerticalMovement();
    }
    
    // Move up or down based on direction
    private void VerticalMovement()
    {
        if (movingUp)
        {
            transform.position += balancer.bouncerVerticalSpeed * Time.deltaTime * Vector3.up;

            // Check if we reached the top
            if (transform.position.y >= topLimit) movingUp = false;
        }
        else
        {
            transform.position += balancer.bouncerVerticalSpeed * Time.deltaTime * Vector3.down;

            // Check if we reached the bottom
            if (transform.position.y <= bottomLimit) movingUp = true;
        }
    }

    private IEnumerator BouncerMovementAnimation()
    {
        float chewDuration = balancer.bouncerChewDuration;
        float frameTime = chewDuration / bouncerAnimation.Length;

        while (true)
        {
            for (int i = 0; i < bouncerAnimation.Length; i++)
            {
                sr.sprite = bouncerAnimation[i];
                yield return new WaitForSeconds(frameTime);
            }
        }
    }
}
