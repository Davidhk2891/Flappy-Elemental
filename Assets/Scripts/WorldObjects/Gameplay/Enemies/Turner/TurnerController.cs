using UnityEngine;

public class TurnerController : BaseObjectController
{
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
        sprite.localPosition = balancer.turnerPivotOffset;

        // Rotate the parent (so rotation affects collider)    
        transform.Rotate(0f, 0f, balancer.turnerRotationSpeed * Time.deltaTime);
    }
}
