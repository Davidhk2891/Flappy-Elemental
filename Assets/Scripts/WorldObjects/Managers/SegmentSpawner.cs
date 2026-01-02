using UnityEngine;
using System.Linq;
using System.Collections;

public class SegmentSpawner : MonoBehaviour
{
    [Header("Pipe set Settings")]
    public PipePool pipePool;
    public Transform pipeSetEnvironment;

    [Header("Enemy segment Settings")]
    public SpawnBounds spawnBounds;
    public Transform enemyEnvironment;
    private string previousName = "";
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

        StartCoroutine(RunGameLoop());
    }

    private IEnumerator RunGameLoop()
    {
        while (true)
        {
            yield return StartCoroutine(SpawnEnemies());
            yield return new WaitForSeconds(balancer.globalDelayAfterEnemies);    

            yield return StartCoroutine(SpawnCheckpoint());
            yield return new WaitForSeconds(balancer.globalDelayBeforeEnemies);
        }
    }

    private IEnumerator SpawnEnemies()
    {
        int enemiesSpawnedInSegment = 0;
        float distanceSinceLastEnemy = 0f;

        // Spawn enemies until cap reached
        while (enemiesSpawnedInSegment <= balancer.globalEnemiesPerSegment)
        {
            // Accumulate distance traveled
            distanceSinceLastEnemy += balancer.globalWorldSpeed * Time.deltaTime;

            // If enough distance passed, spawn enemy
            if (distanceSinceLastEnemy >= balancer.globalEnemySpawnDistance)
            {
                distanceSinceLastEnemy = 0;
                SpawnEnemy();
                enemiesSpawnedInSegment++;
            }
            yield return null;
        }
    }

    private IEnumerator SpawnCheckpoint()
    {
        // Spawn first pipe set
        SpawnPipeSet();

        // Reset distance tracker
        float distanceSinceLastPipeSet = 0f;

        // Wait until the world travels the distance set in balancer
        while (distanceSinceLastPipeSet < balancer.pipeSetSpawnDistance)
        {
            distanceSinceLastPipeSet += balancer.globalWorldSpeed * Time.deltaTime;
            yield return null;
        }

        // Spawn second pipe set
        SpawnPipeSet();
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
    
    private void SpawnEnemy()
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

    // Algorithm for deciding what enemy to spawn based on weighted table
    private GameBalancer.EnemySpawnConfig RollEnemy()
    {
        var table = balancer.enemySpawnTable;
        var active = table.Where(e => e.enabled).ToList();

        if (active.Count == 0)
            return null;
        else if (active.Count == 1)
            return active[0];
        
        while (true)
        {
            // 1. Calculate total weight
            int total = active.Sum(e => e.weight);

            // 2. Get random number
            int roll = Random.Range(0, total);

            // 3. Walk through ranges until there is a match
            int cumulative = 0;
            foreach (var e in active)
            {
                cumulative += e.weight;
                if (roll < cumulative)
                {
                    if (!e.canRepeat && e.enemyName == previousName)
                        break; // Break out of foreach, go back to while

                    previousName = e.enemyName;
                    return e;   
                }
            }   
        }
    }
}
