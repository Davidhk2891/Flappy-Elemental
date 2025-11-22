using UnityEngine;

public class TurnerController : BaseObjectController
{
    [Header("Turner settings")]
    // Degrees per second
    public float rotationSpeed = 180f;
    // Offset from center
    public Vector3 pivotOffset;

    //Child sprite transform
    private Transform sprite;

    void Awake()
    {
        sprite = transform.GetChild(0);
    }

    protected override void Update()
    {
        base.Update();
        RotateTurner();
    }

    private void RotateTurner()
    {
        // Move spirte to pivot offset
        sprite.localPosition = pivotOffset;

        // Rotate the parent (so rotation affects collider)    
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
    }
}
