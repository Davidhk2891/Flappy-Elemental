using UnityEngine;

public static class SaveManager
{
    private const string BestDistanceKey = "best_distance";

    public static void SaveBestDistance(float distance)
    {
        PlayerPrefs.SetFloat(BestDistanceKey, distance);
        PlayerPrefs.Save();
    }

    public static float LoadBestDistance()
    {
        return PlayerPrefs.GetFloat(BestDistanceKey, 0f);
    }
}