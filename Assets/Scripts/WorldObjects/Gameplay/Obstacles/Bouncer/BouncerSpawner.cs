using UnityEngine;

public class BouncerSpawner : MonoBehaviour
{
    [Header("Bouncer Spawner Settings")]
    public BouncerPool bouncerPool;
    // Spawn rate 
    public float spawnRate = 3f;
    // Bottom limit
    public float minY = -3f;
    // Top limit
    public float maxY = 3f;
    // Spawn position off-screen to the right
    public float spawnX = 7f;

    public Transform environmentParent;

    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnRate)
        {
            SpawnBouncer();
            timer = 0f;
        }
    }

    private void SpawnBouncer()
    {
        GameObject bouncer = bouncerPool.GetBouncer();
        bouncer.transform.SetParent(environmentParent);

        if (bouncer != null)
        {
            float randomY = Random.Range(minY, maxY);
            bouncer.transform.position = new Vector3(spawnX, randomY, 0f);
            bouncer.SetActive(true);
        }
    }
}
