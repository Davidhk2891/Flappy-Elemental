using System.Collections;
using UnityEngine;

public class OrbSpawner : MonoBehaviour
{
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private SpawnBounds verticalBounds;
    public Transform OrbEnvironment;
    private GameBalancer balancer;
    private float spawnX;

    [System.Serializable]
    public class OrbPoolBinding
    {
        public string orbName;
        public BaseObjectPool pool;
    }
    [SerializeField] private OrbPoolBinding[] orbPools;

    private void Start()
    {
        balancer = GameSettingsManager.Instance.balancer;

        spawnX = balancer.globalObjectSpawnZone;

        StartCoroutine(RunOrbsLoop());
    }

    private IEnumerator RunOrbsLoop()
    {
        yield return StartCoroutine(SpawnOrbs());    
    }

    private IEnumerator SpawnOrbs()
    {
        Vector3 coinPosition;
        int enemiesSpawnedInSegment = 0;
        float distanceSinceLastCoin = 0f;

        // Spawn orbs as long as there are enemies to spawn
        while (enemiesSpawnedInSegment <= balancer.globalEnemiesPerSegment)
        {
            // Accumulate distance traveled
            distanceSinceLastCoin += balancer.globalWorldSpeed * Time.deltaTime;

            // If enough distance passed, spawn orbs
            if (distanceSinceLastCoin > balancer.globalOrbSpawnDistance 
                && TryFindValidSpawnPosition(out coinPosition))
            {
                distanceSinceLastCoin = 0f;
                SpawnCoin(coinPosition);
                enemiesSpawnedInSegment++;
            }
            yield return null;
        }
    }

    /*
    1) Copy from SpawnEnemy() in SegmentSpawner
    2) You also have to pass the pool which you need to add to scene
    */
    // Logic for spawning individual coin
    /*
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
    */
    private void SpawnCoin(Vector3 coinPosition)
    {
        // Call Orb config class from balancer
        GameBalancer.OrbSpawnConfig config = balancer.orbSpawnTable[0];

        // Get the orbs pool by matching name from balancer
        BaseObjectPool pool = FindOrbPool(config.orbName);
        if (pool == null)
        {
            Debug.Log("No pool found for orb " + config.orbName);
            return;
        }
        
        // Get orb object from its pool
        GameObject orb = pool.GetObject();

        // Parent it
        orb.transform.SetParent(OrbEnvironment);

        // Get orb current y
        var orbCurrentY = coinPosition.y;

        // Set spawn position (x controlled globally, y controlled by OrbController)
        orb.transform.position = new Vector3(balancer.globalObjectSpawnZone, orbCurrentY, 0f);

        // Enable orb (object pooling)
        orb.SetActive(true);
    }

    private BaseObjectPool FindOrbPool(string name)
    {
        foreach (var orbPool in orbPools)
        {
            if (orbPool.orbName == name)
            {
                return orbPool.pool;
            }
        }
        return null;
    }

    private bool TryFindValidSpawnPosition(out Vector3 result)
    {
        // Max attempts for spawning orb
        int maxAttempts = balancer.maxSpawningAttempts;
        // Orb radius for safety check (is the space in radius available?)
        float radius = balancer.spawnAttemptRadius;

        for (int i = 0; i < maxAttempts; i++)
        {
            float y = Random.Range(verticalBounds.BottomLimit, verticalBounds.TopLimit);
            Vector3 tryPos = new Vector3(spawnX, y, 0f);

            // If OverlapCircle returns null -> No collider -> (x,y) spot is open for business
            if (!Physics2D.OverlapCircle(tryPos, radius, obstacleMask))
            {
                result = tryPos;
                return true;
            }
        }

        // Fallback
        result = Vector2.zero;
        return false;
    }
}