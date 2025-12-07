using UnityEngine;

public class WallerController : BaseObjectController, IEnemy
{
    protected override void Update()
    {
        base.Update();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    public void OnSpawn(SpawnBounds bounds)
    {
        // Random Y spawning
        float randomY = Random.Range(bounds.BottomLimit, bounds.TopLimit);
        var spawnPosition = new Vector3(transform.position.x, randomY, 0f);
        transform.position = spawnPosition;

        // Random angle
        float randomAngle = Random.Range(balancer.wallerStartingAngle, balancer.wallerEndingAngle);
        transform.rotation = Quaternion.Euler(0f, 0f, randomAngle);
    }
}
