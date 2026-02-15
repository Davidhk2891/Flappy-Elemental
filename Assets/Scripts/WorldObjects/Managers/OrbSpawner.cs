using System.Collections;
using System.Linq;
using UnityEngine;

public class OrbSpawner : MonoBehaviour
{
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private SpawnBounds verticalBounds;
    public Transform OrbEnvironment;
    private GameBalancer balancer;
    private float spawnX;
    private int enemiesSpawnedInSegment = 0;
    private float distanceSinceLastOrb = 0f;

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

        spawnX = balancer.globalCollectibleSpawnZone;

        StartCoroutine(InitAfterFrame());
    }

    private void OnEnable(){
        SegmentSpawner.OnNewSegment += ResetOrbCounter;
        SegmentSpawner.OnEnemySpawned += OnEnemySpawned;
    }
    private void OnDisable(){
        SegmentSpawner.OnNewSegment -= ResetOrbCounter;
        SegmentSpawner.OnEnemySpawned -= OnEnemySpawned;
    }
    private void ResetOrbCounter(){
        enemiesSpawnedInSegment = 0;
        distanceSinceLastOrb = 0f;
    }
    private void OnEnemySpawned()
    {
        enemiesSpawnedInSegment++;
    }

    private IEnumerator InitAfterFrame()
    {
        yield return null;
        StartCoroutine(RunOrbsLoop());
    }

    private IEnumerator RunOrbsLoop()
    {
        while (true)
        {
            yield return StartCoroutine(SpawnOrbs());

            // Prevents tight loops
            yield return null;       
        }
    }

    private IEnumerator SpawnOrbs()
    {     
        distanceSinceLastOrb = 0f;

        // Spawn orbs as long as there are enemies to spawn
        while (enemiesSpawnedInSegment < balancer.globalEnemiesPerSegment)
        {
            // Accumulate distance traveled
            distanceSinceLastOrb += balancer.globalWorldSpeed * Time.deltaTime;

            // If enough distance passed, and (x,y) is available, spawn orbs
            if (distanceSinceLastOrb > balancer.globalOrbSpawnDistance)
            {
                bool found = false;
                Vector3 spawnPos = Vector3.zero;

                // Wait for the overlap check to complete AFTER physics step
                yield return StartCoroutine(
                    TryFindValidSpawnPosition_Coroutine((ok, pos) => 
                    {
                        found = ok;
                        spawnPos = pos;
                    })
                );

                if (found)
                {
                    distanceSinceLastOrb = 0f;
                    SpawnOrb(spawnPos);
                }
            }
            yield return null;
        }

        // Ensures stable pacing
        yield return null;
    }

    private void SpawnOrb(Vector3 orbPosition)
    {
        // Call Orb config class from balancer
        GameBalancer.OrbSpawnConfig config = RollOrbs();

        // Get the orbs pool by matching name from balancer
        BaseObjectPool pool = FindOrbPool(config.orbName);
        if (pool == null) return;
        
        // Get orb object from its pool
        GameObject orb = pool.GetObject();

        // Parent it
        orb.transform.SetParent(OrbEnvironment);

        // Get orb current y
        var orbCurrentY = orbPosition.y;

        // Set spawn position (x controlled globally, y controlled by OrbController)
        orb.transform.position = new Vector3(spawnX, orbCurrentY, 0f);

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

    private IEnumerator TryFindValidSpawnPosition_Coroutine(System.Action<bool, Vector3> callback)
    {

        yield return new WaitForFixedUpdate();

        Vector3 finalPos = Vector3.zero;
        bool found = false;

        int maxAttempts = balancer.maxSpawningAttempts;
        float radius = balancer.spawnAttemptRadius;

        for (int i = 0; i < maxAttempts; i++)
        {
            float y = Random.Range(verticalBounds.BottomLimit, verticalBounds.TopLimit);
            Vector3 tryPos = new(spawnX, y, 0f);

            // DEBUG: Draw circle for every attempt
            bool hit = Physics2D.OverlapCircle(tryPos, radius, obstacleMask);
            // DEBUG: Create debug marker
            CreateDebugMarker(tryPos, radius, hit ? Color.red : Color.green);

            if (!hit)
            {
                finalPos = tryPos;
                found = true;
                break;
            }
        }

        callback(found, finalPos);
    }

    private void CreateDebugMarker(Vector3 pos, float radius, Color color)
    {
        GameObject marker = new("SpawnDebugCircle");
        marker.transform.position = pos;

        var circle = marker.AddComponent<DebugMovingCircle>();
        circle.radius = radius;
        circle.color = color;

        // Auto-destroy to avoid clutter
        Destroy(marker, 5f);
    }

    private GameBalancer.OrbSpawnConfig RollOrbs()
    {
        var table = balancer.orbSpawnTable;
        var active = table.Where(e => e.enabled).ToList();

        if (active.Count == 0)
            return null;
        
        return active[0];
    }
}