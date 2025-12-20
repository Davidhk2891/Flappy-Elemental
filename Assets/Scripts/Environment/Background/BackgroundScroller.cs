using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    [Header("Background prefab")]
    [SerializeField] private GameObject backgroundTilePrefab;
    [Header("Parent")]
    [SerializeField] private Transform backgroundParent;

    private float tileWidth;
    private GameObject[] bgTiles;
    private GameBalancer balancer;

    private void OnEnable()
    {
        balancer = GameSettingsManager.Instance.balancer;       
    }

    private void Start()
    {
        if (!balancer.enableBackground) return;

        // Get tile width based on sprite bounds
        SpriteRenderer sr = backgroundTilePrefab.GetComponentInChildren<SpriteRenderer>();
        tileWidth = sr.bounds.size.x;

        // Calculate left camera edge
        Camera camera = Camera.main;
        float halfWidth = camera.orthographicSize * camera.aspect;
        float leftEdgeX = camera.transform.position.x - halfWidth;

        // Create two tiles side by side
        SpawnInitialBgTiles(leftEdgeX);
    }

    private void SpawnInitialBgTiles(float leftEdgeX)
    {
        bgTiles = new GameObject[balancer.bgTilesOnScreen];

        for (int i = 0; i < balancer.bgTilesOnScreen; i++)
        {
            // Background
            bgTiles[i] = Instantiate(
                backgroundTilePrefab,
                new Vector3(
                    leftEdgeX + i * tileWidth,
                    0,
                    0
                ),
                Quaternion.identity,
                backgroundParent
            );
        }
    }

    private void Update()
    {
        ScrollBgTiles(bgTiles);
    }

    private void ScrollBgTiles(GameObject[] bgTiles)
    {
        for (int i = 0; i < bgTiles.Length; i++)
        {
            bgTiles[i].transform.Translate(balancer.bgSpeed * Time.deltaTime * Vector3.left);

            // If background tile has moved completely off-screen to the left, recycle it
            if (bgTiles[i].transform.position.x <= balancer.globalObjectsDeadZone - 
                balancer.bgDeadZoneBuffer)
            {
                ResetPosition(bgTiles, i);                
            }
                   
        }
    }

    private void ResetPosition(GameObject[] bgTiles, int position)
    {
        float rightMostX = GetRightmostTileX(bgTiles);
        bgTiles[position].transform.position = new Vector3(rightMostX + tileWidth, 0, 0);
    }

    private float GetRightmostTileX(GameObject[] bgTiles)
    {
        float max = float.MinValue;
        foreach(var t in bgTiles)
            max = Mathf.Max(max, t.transform.position.x);
        return max;
    }
}