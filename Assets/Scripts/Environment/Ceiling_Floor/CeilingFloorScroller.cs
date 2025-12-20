using UnityEngine;

public class CeilingFloorScroller : MonoBehaviour
{
    [Header("Tile prefabs")]
    [SerializeField] private GameObject floorTilePrefab;
    [SerializeField] private GameObject ceilingTilePrefab;

    [Header("Parents")]
    [SerializeField] private Transform floorParent;
    [SerializeField] private Transform ceilingParent;

    private float tileWidth;
    private GameObject[] floorTiles;
    private GameObject[] ceilingTiles;
    private GameBalancer balancer;

    private void OnEnable()
    {
        balancer = GameSettingsManager.Instance.balancer;
    }

    private void Start()
    {
        // Get tile width based on the sprite bounds
        var sr = floorTilePrefab.GetComponentInChildren<SpriteRenderer>();
        tileWidth = sr.bounds.size.x;

        // Calculate left edge of camera
        Camera cam = Camera.main;
        float halfWidth = cam.orthographicSize * cam.aspect;
        float leftEdgeX = cam.transform.position.x - halfWidth;

        // Build row of tiles
        SpawnInitialFcTiles(leftEdgeX);
    }

    private void SpawnInitialFcTiles(float leftEdgeX)
    {
        floorTiles = new GameObject[balancer.fcTilesOnScreen];
        ceilingTiles = new GameObject[balancer.fcTilesOnScreen];

        for (int i = 0; i < balancer.fcTilesOnScreen; i++)
        {
            // Floor
            floorTiles[i] = Instantiate(
                floorTilePrefab,
                new Vector3(leftEdgeX + i * tileWidth, -balancer.fcYPositionFromCenter, 0f),
                Quaternion.identity,
                floorParent
            );

            // Ceiling
            ceilingTiles[i] = Instantiate(
                ceilingTilePrefab,
                new Vector3(
                    leftEdgeX + i * tileWidth,
                    balancer.fcYPositionFromCenter,
                    0),
                Quaternion.identity,
                ceilingParent
            );
        }
    }

    private void Update()
    {
        ScrollFcTiles(floorTiles);
        ScrollFcTiles(ceilingTiles);
    }

    private void ScrollFcTiles(GameObject[] tiles)
    {
        for (int i = 0; i < tiles.Length; i++)
        {
            tiles[i].transform.Translate(balancer.globalWorldSpeed * Time.deltaTime * Vector3.left);

            // If tile moved fully off-screen to the left, recycle it
            if (tiles[i].transform.position.x < balancer.globalObjectsDeadZone)
            {
                ResetPosition(tiles, i);
            }
        }
    }

    private void ResetPosition(GameObject[] tiles, int position)
    {
        float rightMostX = GetRightmostTileX(tiles);
        tiles[position].transform.position = new Vector3(rightMostX + tileWidth, tiles[position].transform.position.y, 0);   
    }

    private float GetRightmostTileX(GameObject[] tiles)
    {
        float max = float.MinValue;
        foreach(var t in tiles)
            max = Mathf.Max(max, t.transform.position.x);
        return max;
    }
}