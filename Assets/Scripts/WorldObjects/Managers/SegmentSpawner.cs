using UnityEngine;
using System.Collections;
using System.Linq;
using Unity.VisualScripting;

public class SegmentSpawner : MonoBehaviour
{
    [Header("Pipe set Settings")]
    public PipePool pipePool;
    public Transform pipeSetEnvironment;
    public float pipeSpawnX = 10f;
    public float pipeHeightOffset = 4f;

    [Header("Enemy segment Settings")]
    public Transform enemyEnvironment;
    public int enemiesPerSegment = 10;
    public float delayBeforeEnemies = 3.5f;
    public float intervalBetweenEnemies = 3f;
    public float delayAfterEnemies = 1f;
    public float enemySpawnX = 10f;
    public float enemyMinY = -3f;
    public float enemyMaxY = 3f;
    public EnemyType[] obstacleTypes;

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
            yield return new WaitForSeconds(delayBeforeEnemies);

            // Obstacles segment
            for (int i = 0; i < enemiesPerSegment; i++)
            {
                SpawnObstacle();
                yield return new WaitForSeconds(intervalBetweenEnemies);
            }

            // Exit pipe
            yield return new WaitForSeconds(delayAfterEnemies);
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
        EnemyType type = PickRandomObstacleType();

        // Get object from its pool
        GameObject enemy = type.pool.GetObject();

        // Parent it
        enemy.transform.SetParent(enemyEnvironment);

        // Get enemy current y
        var enemyCurrentY = enemy.transform.position.y;

        // Spawn position (X controlled here, Y controlled by each obstacle internally)
        enemy.transform.position = new Vector3(enemySpawnX, enemyCurrentY, 0f);

        // Enable it (object pooling)
        enemy.SetActive(true);
    }

    private EnemyType PickRandomObstacleType()
    {
        // Filter out disabled obstacles
        var activeObstacles = obstacleTypes.Where(o => o.enabled).ToList();
        if (activeObstacles.Count == 0)
            return null;

        // 1. Calculate total weight
        int totalWeight = 0;
        foreach (var o in activeObstacles)
            totalWeight += o.weight;
        
        // 2. Get random number
        int random = Random.Range(0, totalWeight);

        // 3. Walk through ranges until we match
        int cumulative = 0;
        foreach (var o in activeObstacles)
        {
            cumulative += o.weight;
            if (random < cumulative)
                return o;
        }

        // 4. Fallback (should never happen)
        return activeObstacles[0];       
    }
}