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

    //New arrays for randimez Y coordinates

    [Header("Spawn Positions")]
    public float[] possibleYPositions = new float[] { -3f, -1.5f, 0f, 1.5f, 3f };


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

        if (bouncer == null)
            return;
            
 bouncer.transform.SetParent(environmentParent);

        float randomY;

        if (possibleYPositions != null && possibleYPositions.Length > 0)
        {
            int index = Random.Range(0, possibleYPositions.Length);
            randomY = possibleYPositions[index];
        }
        else
        {
            randomY = Random.Range(minY, maxY);
        }

        bouncer.transform.position = new Vector3(spawnX, randomY, 0f);
        bouncer.SetActive(true);
    }
}