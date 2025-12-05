using UnityEngine;

public class WallerController : BaseObjectController, IEnemy
{
    [SerializeField] private float topY = 7.5f;
    [SerializeField] private float bottomY = -7;
    [SerializeField] private SpawnBounds spawnBounds;
    private float randomY;
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
        Debug.Log($"Waller Y position is: {transform.position.y}");
    }

    public void onSpawn(SpawnBounds bounds)
    {
        spawnBounds = bounds;

        randomY = Random.Range(spawnBounds.BottomLimit, spawnBounds.TopLimit);

        var spawnPosition = new Vector3(transform.position.x, randomY, 0f);

        transform.position = spawnPosition;
    }
}
