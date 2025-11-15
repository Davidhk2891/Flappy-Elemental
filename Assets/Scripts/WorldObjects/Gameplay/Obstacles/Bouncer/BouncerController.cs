using Unity.VisualScripting;
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
    [Header("Vertical Movement")]
    public float topLimit = 3f;
    public float bottomLimit = -4f;
    public float moveSpeed = 2f;
    private bool movingUp = true;


    [Header("Animation Frames")]            // NEW
    public Sprite[] animationFrames;        // NEW
    public float animationSpeed = 0.1f;  


    private SpriteRenderer sr;              // NEW
    private int frameIndex = 0;             // NEW
    private float frameTimer = 0f;          // NEW

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();     // NEW
    }

    protected override void Update()
    {
        // Leftward motion + off-screen kill check
        base.Update();

        Animate();           // NEW
        VerticalMovement();
    }

    // NEW
    private void Animate()
    {
        if (animationFrames == null || animationFrames.Length == 0)
            return;

        frameTimer += Time.deltaTime;

        if (frameTimer >= animationSpeed)
        {
            frameIndex = (frameIndex + 1) % animationFrames.Length;
            sr.sprite = animationFrames[frameIndex];
            frameTimer = 0f;
        }
    }

    // Move up or down based on direction
    private void VerticalMovement()
    {
        if (movingUp)
        {
            transform.position += moveSpeed * Time.deltaTime * Vector3.up;

            if (transform.position.y >= topLimit)
                movingUp = false;
        }
        else
        {
            transform.position += moveSpeed * Time.deltaTime * Vector3.down;

            if (transform.position.y <= bottomLimit)
                movingUp = true;
        }
    }
}