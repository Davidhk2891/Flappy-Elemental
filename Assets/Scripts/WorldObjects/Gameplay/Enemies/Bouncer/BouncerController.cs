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
    private bool movingUp = true;

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
            if (transform.position.y >= balancer.bouncerTopLimit) movingUp = false;
        }
        else
        {
            transform.position += balancer.bouncerVerticalSpeed * Time.deltaTime * Vector3.down;
            if (transform.position.y <= balancer.bouncerBottomLimit) movingUp = true;
        }
    }
}
