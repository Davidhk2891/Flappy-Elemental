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

    // Distance-based spawning
    private float distanceSinceLastEnemy = 0f;

    // Enemies spawned in segment
    private int enemiesSpawnedInSegment = 0;

    // Cached last x position to measure distance moved
    private float lastXPosition;
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
        lastXPosition = transform.position.x;

        balancer = GameSettingsManager.Instance.balancer;
        StartCoroutine(SpawnSegments());
    }

    private IEnumerator SpawnSegments()
    {
        while (true)
        {
            yield return null;
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
            new Vector3(balancer.globalObjectSpawnZone, randomY, -1f),
            transform.rotation
        );
        pipeSet.SetActive(true);
    }
    
    private void SpawnObstacle()
    {
        // Pick based on weight
        GameBalancer.EnemySpawnConfig config = RollEnemy();

        BaseObjectPool pool = FindPool(config.enemyName);
        if (pool == null)
        {
            Debug.Log("No pool found for enemy: " + config.enemyName);
            return;
        }

        // Get object from its pool
        GameObject enemy = pool.GetObject();

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

    private BaseObjectPool FindPool(string name)
    {
        foreach (var binding in enemyPools)
        {
            if (binding.enemyName == name)
                return binding.pool;
        }
        return null;
    }

    private GameBalancer.EnemySpawnConfig RollEnemy()
    {
        var table = balancer.enemySpawnTable;
        var active = table.Where(e => e.enabled).ToList();
        if (active.Count == 0)
            return null;
        
        // 1. Calculate total weight
        int total = active.Sum(e => e.weight);

        // 2. Get random number
        int roll = Random.Range(0, total);

        // 3. Walk through ranges until we match
        int cumulative = 0;
        foreach (var e in active)
        {
            cumulative += e.weight;
            if (roll < cumulative)
                return e;
        }

        // 4. Fallback (should never happen)
        return active[0];
    }
}
