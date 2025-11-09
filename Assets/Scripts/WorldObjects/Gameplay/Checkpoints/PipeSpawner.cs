using UnityEngine;

public class PipeSpawner : MonoBehaviour
{
    [Header("Pipe Spawner Settings")]
    // Reference to PipePair prefab
    public GameObject pipePrefab;
    // How often to spawn new pipes
    public float spawnRate = 2f;
    // How much random vertical variance the gap can have
    public float heightOffset = 1f;

    public Transform environmentParent;

    private float timer = 0f;

    public PipePool pipePool;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnRate)
        {
            SpawnPipe();
            timer = 0f;
        }
    }
    
    void SpawnPipe()
    {
        float lowestPoint = transform.position.y - heightOffset;
        float highestPoint = transform.position.y + heightOffset;

        float randomY = Random.Range(lowestPoint, highestPoint);

        GameObject pipe = pipePool.GetPipe();
        pipe.transform.SetParent(environmentParent);
        pipe.transform.SetPositionAndRotation
        (
            new Vector3(transform.position.x, randomY, 0f),
            transform.rotation
        );
        pipe.SetActive(true);
        Debug.Log($"[Pool] pipe set spawned at y={randomY}");
    }
}
