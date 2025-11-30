using UnityEngine;

public class TileScroller : MonoBehaviour
{
    [Header("Tile prefabs")]
    [SerializeField] private GameObject floorTilePrefab;
    [SerializeField] private GameObject ceilingTilePrefab;

    [Header("Scrolling settings")]
    [SerializeField] private int tilesOnScreen = 12;
    [SerializeField] private float scrollSpeed = 4f;
    [SerializeField] private float positionFromCenter = 11.5f;
    [SerializeField] private float tileRecycleOffset = -10f;

    [Header("Parents")]
    [SerializeField] private Transform floorParent;
    [SerializeField] private Transform ceilingParent;

    private float tileWidth;
    private GameObject[] floorTiles;
    private GameObject[] ceilingTiles;

    private void Start()
    {
        // Get tile width based on the sprite bounds
        var sr = floorTilePrefab.GetComponentInChildren<SpriteRenderer>();
        tileWidth = sr.bounds.size.x;

        // Build row of tiles
        SpawnInitialTiles();
    }


    private void Update()
    {
        ScrollTiles(floorTiles);
        ScrollTiles(ceilingTiles);
    }

    private void SpawnInitialTiles()
    {
        floorTiles = new GameObject[tilesOnScreen];
        ceilingTiles = new GameObject[tilesOnScreen];

        for (int i = 0; i < tilesOnScreen; i++)
        {
            // Floor
            floorTiles[i] = Instantiate(
                floorTilePrefab,
                new Vector3(i * tileWidth, -positionFromCenter, 0f),
                Quaternion.identity,
                floorParent
            );

            // Ceiling
            ceilingTiles[i] = Instantiate(
                ceilingTilePrefab,
                new Vector3(i * tileWidth, positionFromCenter, 0),
                Quaternion.identity,
                ceilingParent
            );
        }
    }

    private void ScrollTiles(GameObject[] tiles)
    {
        for (int i = 0; i < tiles.Length; i++)
        {
            tiles[i].transform.Translate(Vector3.left * scrollSpeed * Time.deltaTime);

            // If tile moved fully off-screen to the left
            if (tiles[i].transform.position.x < tileRecycleOffset)
            {
                float rightMostX = GetRightmostTileX(tiles);
                tiles[i].transform.position = new Vector3(rightMostX + tileWidth, tiles[i].transform.position.y, 0);   
            }
        }
    }

    private float GetRightmostTileX(GameObject[] tiles)
    {
        float max = float.MinValue;
        foreach(var t in tiles)
            max = Mathf.Max(max, t.transform.position.x);
        return max;
    }
}