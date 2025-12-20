using UnityEngine;

public class WallerController : BaseObjectController, IEnemy
{
    float wallerHeight;
     
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
        var sr = GetComponentInChildren<SpriteRenderer>();
        wallerHeight = sr.bounds.size.y;
    }

    public void OnSpawn(SpawnBounds bounds)
    {
        // Rotation
        transform.rotation = Quaternion.identity;

        // Random spawn place (top or bottom)
        bool spawnTop = Random.value > 0.5f;

        // Waller half height
        float halfHeight = wallerHeight / 2;

        // Compute Y spawn
        float YSpawn = spawnTop ? bounds.TopViewportY - halfHeight :
         bounds.BottomViewportY + halfHeight;

        // Compute position
        var spawnPosition = new Vector3(balancer.globalObjectSpawnZone, YSpawn, -1f);
        transform.position = spawnPosition;

    }
}
