using UnityEngine;

[CreateAssetMenu(fileName = "GameBalancer", menuName = "Settings/Game")]
public class GameBalancer : ScriptableObject
{
    [Header("World movement")]
    [Tooltip("Values can be overriden by enemie's individual speed")]
    public float globalWorldSpeed = 5f;
    public float globalEnemySpawnZone = 10f;
    public float globalCollectibleSpawnZone = 10f;
    public float globalObjectsDeadZone = -10f;

    [Header("Floor & Ceiling (FC)")]
    public int fcTilesOnScreen = 12;
    public float fcYPositionFromCenter = 11.3f;

    [Header("Background")]
    public bool enableBackground = true;
    public int bgTilesOnScreen = 8;
    public float bgSpeed = 2f;
    public float bgDeadZoneBuffer = 16f;

    [Header("Player Settings")]
    public float playerJumpForce = 9f;
    public float playerGravity = 3f;
    public float playerSpriteJumpDuration = 0.15f;

    [Header("Checkpoint settings")]
    public float pipeSetSpawnDistance = 10f;
    public float pipeHeightOffset = 3.5f;
    public float checkpointGap = 3f;

    [Header("Global orb settings")]
    public int maxSpawningAttempts = 10;
    public float spawnAttemptRadius = 0.5f;
    public float globalOrbSpawnDistance = 10f;
    public float shineDuration = 1f;
    [System.Serializable]
    public class OrbSpawnConfig
    {
        public string orbName;
        public bool enabled = true;
    }
    public OrbSpawnConfig[] orbSpawnTable;

    [Header("Global enemy settings")]
    public int globalEnemiesPerSegment = 25;
    public float globalEnemySpawnDistance = 5f;
    public float globalDelayBeforeEnemies = 2f;
    public float globalDelayAfterEnemies = 2f;

    [System.Serializable]
    public class EnemySpawnConfig
    {
        public string enemyName;
        public bool enabled = true;
        public bool canRepeat = false;
        public int weight = 1;
    }
    
    public EnemySpawnConfig[] enemySpawnTable;

    [Header("- Batty settings")]
    public float battyFlapDuration = 0.5f;
    public float battySpeedMultiplier = 1.5f;

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
    public float[] bouncerTopLimit = { 1f, 5f, 10f };
    public float[] bouncerBottomLimit = { -9f, -5f, -2f };
    public float bouncerChewDuration = 0.1f;
    public float bouncerVerticalSpeed = 8f;

    [Header("- Turner settings")]
    public float turnerRotationSpeed = 180f;
    public Vector3 turnerPivotOffset;
    public float turnerTailAnimDuration = 0.3f;

    [Header("- Pulsar settings")]
    public float pulsarBaseScale = 0.5f;
    public float pulsarMinPulseScale = 0.5f;
    public float pulsarMaxPulseScale = 1f;
    public float pulsarGrowSpeed = 10f;
    public float pulsarShrinkSpeed = 1.5f;
    public float pulsarHoldDuration = 1f;

    [Header("Global difficuly (Not built yet)")]
    [Range(1f, 5f)]
    public float difficultyMultiplier = 1f;
}
