using UnityEngine;

public class WallerController : BaseObjectController
{
    [SerializeField] private float topY = 6.5f;
    [SerializeField] private float bottomY = -6.5f;
    protected override void Update()
    {
        base.Update();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        PositionWallerInY();
    }

    private void PositionWallerInY()
    {
        // Pick 0 or 1
        int yPosTarget = Random.Range(0, 2);

        // Set Y position
        float yPos = yPosTarget == 0 ? topY : bottomY;

        // Set position in screen. Only change y
        Vector3 pos = transform.position;
        pos.y = yPos;
        transform.position = pos;

    }
}
