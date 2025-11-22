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
    }
}
