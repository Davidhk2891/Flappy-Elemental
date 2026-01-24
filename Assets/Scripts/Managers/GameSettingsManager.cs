using UnityEngine;

/*
Any script in the game can access this singleton instance via:
-> GameSettingsManager.Instance.balancer.<property> 
*/
public class GameSettingsManager : MonoBehaviour
{
    public static GameSettingsManager Instance { get; private set; }

    [Header("Core Balancing")]
    public GameBalancer balancer;
    private void Awake()
    {
        // Singleton class
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
}
