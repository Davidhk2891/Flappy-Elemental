using UnityEngine;
using TMPro;

public class HUDController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text textCurrentDistance;
    [SerializeField] private TMP_Text textBestDistance;
    [SerializeField] private TMP_Text textOrbsInRun;

    private RunSessionManager run;

    void Start()
    {
        run = RunSessionManager.Instance;

        // On new run, show zeros
        UpdateHUD(0f, 0f, 0);
    }
    
    void Update()
    {
        UpdateHUD(
            run.DistanceTraveled,
            run.BestDistanceTraveled,
            run.OrbsCollected
        );
    }

    private void UpdateHUD(float distance, float bestDistance, float orbsInRun)
    {
        textCurrentDistance.text = $"{Mathf.FloorToInt(distance):0000}M";
        textBestDistance.text = $"BEST:{Mathf.FloorToInt(bestDistance)}M";
        textOrbsInRun.text = orbsInRun.ToString("000");
    }
}