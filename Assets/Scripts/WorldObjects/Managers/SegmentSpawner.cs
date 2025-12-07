using UnityEngine;
using System.Collections;
using System.Linq;

public class SegmentSpawner : MonoBehaviour
{
    [Header("Pipe set Settings")]
    public PipePool pipePool;
    public Transform pipeSetEnvironment;

    [Header("Enemy segment Settings")]
    public Transform enemyEnvironment;
    public SpawnBounds spawnBounds;
    public EnemyType[] obstacleTypes;

    private GameBalancer balancer;

    [System.Serializable]
    public class EnemyPoolBinding
    {
        public string enemyName;
        public BaseObjectPool pool;
    }

    [SerializeField] private EnemyPoolBinding[] enemyPools;

    private void Start()
    {
        balancer = GameSettingsManager.Instance.balancer;
        StartCoroutine(SpawnSegments());
    }

    private IEnumerator SpawnSegments()
    {
        while (true)
        {
            // Entry pipe
            SpawnPipeSet();

            // Wait before spawning obstacles
            yield return new WaitForSeconds(balancer.globalDelayBeforeEnemies);

            // Obstacles segment
            for (int i = 0; i < balancer.globalEnemiesPerSegment; i++)
            {
                SpawnObstacle();
                yield return new WaitForSeconds(balancer.globalEnemySpawnInterval);
            }

            // Exit pipe
            yield return new WaitForSeconds(balancer.globalDelayAfterEnemies);
            SpawnPipeSet();

            // Delay before next segment (this will change)
            yield return new WaitForSeconds(balancer.checkpointGap);
        }
    }

    private void SpawnPipeSet()
    {
        // Pipe set
        float lowestPoint = transform.position.y - balancer.pipeHeightOffset;
        float highestPoint = transform.position.y + balancer.pipeHeightOffset;

        float randomY = Random.Range(lowestPoint, highestPoint);

        GameObject pipeSet = pipePool.GetObject();
        pipeSet.transform.SetParent(pipeSetEnvironment);
        pipeSet.transform.SetPositionAndRotation
        (
            new Vector3(balancer.globalObjectSpawnZone, randomY, 0f),
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
        enemy.transform.position = new Vector3(balancer.globalObjectSpawnZone, enemyCurrentY, 0f);

        // Inject spawn bounds into the enemy
        var enemyLogic = enemy.GetComponent<IEnemy>();
        enemyLogic?.OnSpawn(spawnBounds);

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