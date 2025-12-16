using UnityEngine;

[CreateAssetMenu(fileName = "GameBalancer", menuName = "Settings/Game")]
public class GameBalancer : ScriptableObject
{
    [Header("World movement")]
    [Tooltip("Values can be overriden by enemie's individual speed")]
    public float globalWorldSpeed = 4f; //-----------------------------> IN PLACE
    public float enemySpawnDistance = 6f;
    public float pipeSpawnDistance = 12f;
    public float globalObjectSpawnZone = 10f; //-----------------------> IN PLACE
    public float globalObjectsDeadZone = -10f; //----------------------> IN PLACE

    [Header("Floor & Ceiling (FC)")]
    public int fcTilesOnScreen = 12; //--------------------------------> IN PLACE
    public float fcYPositionFromCenter = 11.5f; //---------------------> IN PLACE

    [Header("Player Settings")]
    public float playerJumpForce = 10f; //-----------------------------> IN PLACE
    public float playerGravity = 2f; //--------------------------------> IN PLACE
    public float playerSpriteJumpDuration = 0.15f; //------------------> IN PLACE

    [Header("Checkpoint settings")]
    public float pipeHeightOffset = 3.5f; //---------------------------> IN PLACE
    public float checkpointGap = 3f; //--------------------------------> IN PLACE

    [Header("Global Enemy settings")]
    public int globalEnemiesPerSegment = 20; //------------------------> IN PLACE 
    public float globalDelayBeforeEnemies = 3.5f; //-------------------> IN PLACE
    public float globalEnemySpawnInterval = 1f; //---------------------> IN PLACE
    public float globalDelayAfterEnemies = 1f; //----------------------> IN PLACE

    [System.Serializable]
    public class EnemySpawnConfig
    {
        public string enemyName;
        public bool enabled = true;
        public int weight = 1;
    }
    
    public EnemySpawnConfig[] enemySpawnTable; //----------------------> IN PLACE

    [Header("- Batty settings")]
    public float battyFlapDuration = 1f; //----------------------------> IN PLACE
    public float battySpeedMultiplier = 1.5f; //-----------------------> IN PLACE

    [Header("- Waller settings")]
    public float wallerStartingAngle = 0f; //--------------------------> IN PLACE
    public float wallerEndingAngle = 360f; //--------------------------> IN PLACE

    [Header("- Bouncer settings")]
    public float bouncerTopLimit = 8f; //------------------------------> IN PLACE
    public float bouncerBottomLimit = -10f; //-------------------------> IN PLACE
    public float bouncerVerticalSpeed = 8f; //-------------------------> IN PLACE

    [Header("- Turner settings")]
    public float turnerRotationSpeed = 180f; //------------------------> IN PLACE
    public Vector3 turnerPivotOffset; //-------------------------------> IN PLACE

    [Header("- Pulsar settings")]
    public float pulsarBaseScale = 0.5f; //----------------------------> IN PLACE
    public float pulsarMinPulseScale = 0.5f; //------------------------> IN PLACE
    public float pulsarMaxPulseScale = 1f; //--------------------------> IN PLACE
    public float pulsarGrowSpeed = 10f; //-----------------------------> IN PLACE
    public float pulsarShrinkSpeed = 1.5f; //--------------------------> IN PLACE
    public float pulsarHoldDuration = 1f; //---------------------------> IN PLACE

    [Header("Global difficuly (Not built yet)")]
    [Range(1f, 5f)]
    public float difficultyMultiplier = 1f;
}
