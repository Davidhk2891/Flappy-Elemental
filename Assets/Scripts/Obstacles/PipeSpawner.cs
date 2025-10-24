using UnityEngine;

public class PipeSpawner : MonoBehaviour
{
    // Reference to PipePair prefab
    public GameObject pipePrefab;
    // How often to spawn new pipes
    public float spawnRate = 2f;
    // How much random vertical variance the gap can have
    public float heightOffset = 1f;

    private float timer = 0f;

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

        Instantiate(pipePrefab, new Vector3(transform.position.x, randomY, 0), Quaternion.identity);
        Debug.Log($"Pipe set spawned with randomY at {randomY}");
    }
}
