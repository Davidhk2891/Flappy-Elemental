using System.Collections;
using UnityEngine;

public class WallerController : BaseObjectController, IEnemy
{
    [Header("Waller variants")]
    [SerializeField] private Vector3 shortScale = new Vector3(0.15f, 0.6f, 1f);
    [SerializeField] private Vector3 mediumScale = new Vector3(0.15f, 1.2f, 1f);
    [SerializeField] private Vector3 longScale = new Vector3(0.15f, 1.8f, 1f);

    public enum WallerSize { Short, Medium, Long }
    private Transform spriteTransform;
    private float wallerHeight;
     
    protected override void Update()
    {
        base.Update();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    private void Awake()
    {
        // Cache the sprite transform height
        spriteTransform = GetComponentInChildren<SpriteRenderer>().transform;
    }

    public void OnSpawn(SpawnBounds bounds)
    {
        // 1. Reset rotation (always vertical)
        transform.rotation = Quaternion.identity;

        // 2. Pick/Apply size variant
        var chosenSize = PickWeightedWallerSize();
        ApplyWallerSize(chosenSize);

        // 3. Recalculate height after scaling
        wallerHeight = GetSpriteWorldHeight();

        // 4. Resize collider to sprite
        ResizeColliderToSprite();

        float halfHeight = wallerHeight / 2;

        // 5. Choose spawn side (top or bottom)
        bool spawnTop = Random.value > 0.5f;

        // 6. Compute Y spawn
        float YSpawn = spawnTop
            ? bounds.TopViewportY - halfHeight 
            : bounds.BottomViewportY + halfHeight;

        // 7. set final position
        var spawnPosition = new Vector3(
            balancer.globalObjectSpawnZone,
            YSpawn,
            -1f);

        transform.position = spawnPosition;
    }

    private void ResizeColliderToSprite()
    {
        BoxCollider2D col = GetComponent<BoxCollider2D>();
        if (col == null) return;

        // Get world size of sprite after scaling
        float spriteHeight = GetSpriteWorldHeight();
        float spriteWidth = col.size.x;

        // Convert world size back into collider local size
        Vector2 newColSize = new Vector2(spriteWidth, spriteHeight);
        col.size = newColSize;
    }

    private void ApplyWallerSize(GameBalancer.WallerSizeType size)
    {
        switch (size)
        {
            case GameBalancer.WallerSizeType.Short:
                spriteTransform.localScale = shortScale;
                break;
            case GameBalancer.WallerSizeType.Medium:
                spriteTransform.localScale = mediumScale;
                break;
            case GameBalancer.WallerSizeType.Long:
                spriteTransform.localScale = longScale;
                break;
        }
    }

    private GameBalancer.WallerSizeType PickWeightedWallerSize()
    {
        var table = balancer.wallerVariantWeights;
        // 1. Compute total weight
        int totalWeight = 0;
        foreach (var entry in table)
            totalWeight += entry.weight;

        // 2. Roll number
        int roll = Random.Range(0, totalWeight);

        // 3. Walk through cumulative distribution
        int cumulative = 0;
        foreach (var entry in table)
        {
            cumulative += entry.weight;
            if (roll < cumulative)
                return entry.size;
        }

        // Fallback
        return table[0].size;
    }

    private float GetSpriteWorldHeight()
    {
        var sr = spriteTransform.GetComponent<SpriteRenderer>();
        return sr.bounds.size.y;
    }
}
