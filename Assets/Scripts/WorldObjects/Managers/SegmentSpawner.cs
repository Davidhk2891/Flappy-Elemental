using UnityEngine;
using System.Collections;

public class SegmentSpawner : MonoBehaviour
{
    [Header("Pipe set Settings")]
    public PipePool pipePool;
    public Transform pipeSetEnvironment;
    public float pipeSpawnX = 10f;
    public float pipeHeightOffset = 4f;

    [Header("Obstacle segment Settings")]
    public Transform obstacleEnvironment;
    public int obstaclesPerSegment = 5;
    public float delayBeforeObstacles = 3.5f;
    public float intervalBetweenObstacles = 3f;
    public float delayAfterObstacles = 1f;
    public float obstacleSpawnX = 10f;
    public float obstacleMinY = -3f;
    public float obstacleMaxY = 3f;
    public ObstacleType[] obstacleTypes;

    [Header("Checkpoint")]
    public float checkpointPipeGap = 3f;


    private void Start()
    {
        StartCoroutine(SpawnSegments());
    }

    private IEnumerator SpawnSegments()
    {
        while (true)
        {
            // Entry pipe
            SpawnPipeSet();

            // Wait before spawning obstacles
            yield return new WaitForSeconds(delayBeforeObstacles);

            // Obstacles segment
            for (int i = 0; i < obstaclesPerSegment; i++)
            {
                SpawnObstacle();
                yield return new WaitForSeconds(intervalBetweenObstacles);
            }

            // Exit pipe
            yield return new WaitForSeconds(delayAfterObstacles);
            SpawnPipeSet();

            // Delay before next segment (this will change)
            yield return new WaitForSeconds(checkpointPipeGap);
        }
    }

    private void SpawnPipeSet()
    {
        // Pipe set
        float lowestPoint = transform.position.y - pipeHeightOffset;
        float highestPoint = transform.position.y + pipeHeightOffset;

        float randomY = Random.Range(lowestPoint, highestPoint);

        GameObject pipeSet = pipePool.GetObject();
        pipeSet.transform.SetParent(pipeSetEnvironment);
        pipeSet.transform.SetPositionAndRotation
        (
            new Vector3(pipeSpawnX, randomY, 0f),
            transform.rotation
        );
        pipeSet.SetActive(true);
    }
    
    private void SpawnObstacle()
    {
        // Pick based on weight
        ObstacleType type = PickRandomObstacleType();

        // Get object from its pool
        GameObject obstacle = type.pool.GetObject();

        // Parent it
        obstacle.transform.SetParent(obstacleEnvironment);

        // Spawn position (X controlled here, Y controlled by each obstacle internally)
        obstacle.transform.position = new Vector3(obstacleSpawnX, 0f, 0f);

        // Enable it (object pooling)
        obstacle.SetActive(true);
    }

    private ObstacleType PickRandomObstacleType()
    {
        // 1. Calculate total weight
        int totalWeight = 0;
        foreach (var o in obstacleTypes)
            totalWeight += o.weight;
        
        // 2. Get random number
        int random = Random.Range(0, totalWeight);

        // 3. Walk through ranges until we match
        int cumulative = 0;
        foreach (var o in obstacleTypes)
        {
            cumulative += o.weight;
            if (random < cumulative)
                return o;
        }

        // 4. Fallback (should never happen)
        return obstacleTypes[0];       
    }
}