using UnityEngine;

/*
A run session exists only while the player is alive.
So the manager needs to track per-run stats, and wipe them clean when the run ends
*/
public class RunSessionManager : MonoBehaviour
{
    public static RunSessionManager Instance { get; private set; }

    public int orbsCollected { get; private set; }
    public float distanceTraveled { get; private set; }
    public int enemiesPassed { get; private set; }

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
        orbsCollected = 0;
        distanceTraveled = 0f;
        enemiesPassed = 0;
    }

    // Add orb
    public void AddOrb(int amount = 1)
    {
        orbsCollected += amount;
    }

    // Add distance
    public void AddDistance(float amount)
    {
        distanceTraveled += amount;
    }

    // Add enemy pass
    public void AddEnemyPass()
    {
        enemiesPassed++;
    }
}