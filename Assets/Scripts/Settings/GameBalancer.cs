using UnityEngine;

[CreateAssetMenu(fileName = "GameBalancer", menuName = "Settings/Game")]
public class GameBalancer : ScriptableObject
{
    [Header("World movement")]
    [Tooltip("Values can be overriden by enemie's individual speed")]
    public float globalWorldSpeed = 4f; //-----------------------------> IN PLACE
    public float globalObjectSpawnZone = 10f; //-----------------------> IN PLACE
    public float globalObjectsDeadZone = -10f; //----------------------> IN PLACE

    [Header("Floor & Ceiling (FC)")]
    public int fcTilesOnScreen = 12; //--------------------------------> IN PLACE
    public float fcYPositionFromCenter = 11.3f; //---------------------> IN PLACE

    [Header("Background")]
    public bool enableBackground = true; //----------------------------> IN PLACE
    public int bgTilesOnScreen = 3; //---------------------------------> IN PLACE
    public float bgSpeed = 2f; //--------------------------------------> IN PLACE
    public float bgDeadZoneBuffer = 7f; //-----------------------------> IN PLACE

    [Header("Player Settings")]
    public float playerJumpForce = 10f; //-----------------------------> IN PLACE
    public float playerGravity = 2f; //--------------------------------> IN PLACE
    public float playerSpriteJumpDuration = 0.15f; //------------------> IN PLACE

    [Header("Checkpoint settings")]
    public float pipeSetSpawnDistance = 3f; //-------------------------> IN PLACE
    public float pipeHeightOffset = 3.5f; //---------------------------> IN PLACE
    public float checkpointGap = 3f; //--------------------------------> IN PLACE

    [Header("Global orb settings")]
    public int maxSpawningAttempts = 15; //----------------------------> IN PLACE
    public float spawnAttemptRadius = 1f; //-------------------------> IN PLACE
    public float globalOrbSpawnDistance = 10f; //----------------------> IN PLACE
    public float shineDuration = 1f; //--------------------------------> IN PLACE
    [System.Serializable]
    public class OrbSpawnConfig
    {
        public string orbName;
        public bool enabled = true;
        public int spawnFrequency = 1;
    }
    public OrbSpawnConfig[] orbSpawnTable; //--------------------------> IN PLACE

    [Header("Global enemy settings")]
    public int globalEnemiesPerSegment = 20; //------------------------> IN PLACE 
    public float globalEnemySpawnDistance = 6f; //---------------------> IN PLACE
    public float globalDelayBeforeEnemies = 2f; //---------------------> IN PLACE
    public float globalDelayAfterEnemies = 2f; //----------------------> IN PLACE

    [System.Serializable]
    public class EnemySpawnConfig
    {
        public string enemyName;
        public bool enabled = true;
        public bool canRepeat = false;
        public int weight = 1;
    }
    
    public EnemySpawnConfig[] enemySpawnTable; //----------------------> IN PLACE

    [Header("- Batty settings")]
    public float battyFlapDuration = 1f; //----------------------------> IN PLACE
    public float battySpeedMultiplier = 1.5f; //-----------------------> IN PLACE

    public enum WallerSizeType
    {
        Short,
        Medium,
        Long
    }
    [System.Serializable]
    public class WallerVariantWeight
    {
        public WallerSizeType size;
        public int weight = 1;
    }
    [Header("- Waller settings")]
    public WallerVariantWeight[] wallerVariantWeights = new WallerVariantWeight[3];

    [Header("- Bouncer settings")]
    public float[] bouncerTopLimit = { 1f, 5f, 10f }; //---------------> IN PLACE
    public float[] bouncerBottomLimit = { -9f, -5f, -2f }; //----------> IN PLACE
    public float bouncerChewDuration = 1f;
    public float bouncerVerticalSpeed = 8f; //-------------------------> IN PLACE

    [Header("- Turner settings")]
    public float turnerRotationSpeed = 180f; //------------------------> IN PLACE
    public Vector3 turnerPivotOffset; //-------------------------------> IN PLACE
    public float turnerTailAnimDuration = 0.3f; //---------------------> IN PLACE

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
