using UnityEngine;

/*
A run session exists only while the player is alive.
So the manager needs to track per-run stats, and wipe them clean when the run ends
*/
public class RunSessionManager : MonoBehaviour
{
    public static RunSessionManager Instance { get; private set; }

    public int OrbsCollected { get; private set; }
    public float DistanceTraveled { get; private set; }
    public float BestDistanceTraveled { get; private set; }
    public int EnemiesPassed { get; private set; }

    private void Awake()
    {
        // Standard Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Called when the game starts or is restarted
    public void ResetSession()
    {
        OrbsCollected = 0;
        DistanceTraveled = 0f;
        BestDistanceTraveled = SaveManager.LoadBestDistance();
        EnemiesPassed = 0;
    }

    // Add orb
    public void AddOrb(int amount = 1)
    {
        OrbsCollected += amount;
    }

    // Add distance
    public void AddDistance(float amount)
    {
        DistanceTraveled += amount;
    }

    // Add enemy pass
    public void AddEnemyPass()
    {
        EnemiesPassed++;
    }
}