using UnityEngine;

/*
- Moves the object up and down around its starting position
- Moves at a fixed speed
- Using Time.deltaTime (so it's smooth)
- Keeps all the movement on the parent (not the child)

This will be the vase for all animated obstacles later
*/
public class BouncerObstacle : MonoBehaviour
{

    public float topLimit = 3f;
    public float bottomLimit = -4f;
    public float moveSpeed = 2f;

    private bool movingUp = true;

    void Start()
    {
        
    }

    void Update()
    {
        // Move up or down based on direction
        if (movingUp)
        {
            transform.position += moveSpeed * Time.deltaTime * Vector3.up;

            // Check if we reached the top
            if (transform.position.y >= topLimit) movingUp = false;
        }
        else
        {
            transform.position += moveSpeed * Time.deltaTime * Vector3.down;
            if (transform.position.y <= bottomLimit) movingUp = true; 
        }
    }
}
