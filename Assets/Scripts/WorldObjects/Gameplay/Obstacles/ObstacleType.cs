[System.Serializable]
public class ObstacleType
{
    // For inspector clarity
    public string name;
    // Spawn chance
    public int weight = 1;
    // Which pool to pull from
    public BaseObjectPool pool;
}