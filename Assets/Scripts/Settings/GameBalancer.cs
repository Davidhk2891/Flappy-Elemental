using UnityEngine;

[CreateAssetMenu(fileName = "GameBalancer", menuName = "Settings/Game")]
public class GameBalancer : ScriptableObject
{
    [Header("World movement")]
    [Tooltip("Values can be overriden by enemie's individual speed")]
    public float globalWorldSpeed = 4f; //------------------------------> IN PLACE
    public float globalObjectSpawnZone = 10f; //------------------------> IN PLACE
    public float globalObjectsDeadZone = -10f; //-----------------------> IN PLACE

    [Header("Checkpoint settings")]
    public float pipeHeightOffset = 3.5f; //----------------------------> IN PLACE
    public float checkpointGap = 3f; //---------------------------------> IN PLACE

    [Header("Global Enemy settings")]
    public int globalEnemiesPerSegment = 20; //------------------------> IN PLACE 
    public float globalDelayBeforeEnemies = 3.5f; //-------------------> IN PLACE
    public float globalEnemySpawnInterval = 1f; //---------------------> IN PLACE
    public float globalDelayAfterEnemies = 1f; //----------------------> IN PLACE

    [Header("- Batty")]
    public float battyFlapDuration = 1f; //----------------------------> IN PLACE
    public float battySpeedMultiplier = 1.5f; //-----------------------> IN PLACE

    [Header("- Waller")]
    public float wallerStartingAngle = 0f; //--------------------------> IN PLACE
    public float wallerEndingAngle = 360f; //--------------------------> IN PLACE

    [Header("Player Settings")]
    public float playerJumpForce = 10f; //-----------------------------> IN PLACE
    public float playerGravity = 2f; //--------------------------------> IN PLACE
    public float playerSpriteJumpDuration = 0.15f; //------------------> IN PLACE

    [Header("Global difficuly")]
    [Range(1f, 5f)]
    public float difficultyMultiplier = 1f;
}
