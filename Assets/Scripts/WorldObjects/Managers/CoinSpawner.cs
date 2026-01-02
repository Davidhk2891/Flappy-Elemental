using System.Collections;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private SpawnBounds verticalBounds;
    public Transform CoinEnvironment;
    private GameBalancer balancer;
    private float spawnX;

    private void Start()
    {
        balancer = GameSettingsManager.Instance.balancer;

        spawnX = balancer.globalObjectSpawnZone;

        StartCoroutine(RunCoinsLoop());
    }

    private IEnumerator RunCoinsLoop()
    {
        yield return StartCoroutine(SpawnCoins());    
    }

    private IEnumerator SpawnCoins()
    {
        Vector2 position;
        int enemiesSpawnedInSegment = 0;
        float distanceSinceLastCoin = 0f;

        // Spawn coins while there are enemies to spawn
        while (enemiesSpawnedInSegment <= balancer.globalEnemiesPerSegment)
        {
            // Accumulate distance traveled
            distanceSinceLastCoin += balancer.globalWorldSpeed * Time.deltaTime;

            // If enough distance passed, spawn coin
            if (distanceSinceLastCoin > balancer.globalCoinSpawnDistance 
                && TryFindValidSpawnPosition(out position))
            {
                distanceSinceLastCoin = 0f;
                SpawnCoin(position);
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
    private void SpawnCoin(Vector2 position)
    {
        
    }

    private bool TryFindValidSpawnPosition(out Vector2 result)
    {
        // Max attempts for spawning coin
        int maxAttempts = balancer.maxSpawningAttempts;
        // Coin radius for safety check (is the space in radius available?)
        float radius = balancer.spawnAttemptRadius;

        for (int i = 0; i < maxAttempts; i++)
        {
            float y = Random.Range(verticalBounds.BottomLimit, verticalBounds.TopLimit);
            Vector2 tryPos = new Vector2(spawnX, y);

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