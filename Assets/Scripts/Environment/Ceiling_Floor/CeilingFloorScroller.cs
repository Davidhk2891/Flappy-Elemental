using System;
using UnityEngine;

public class CeilingFloorScroller : MonoBehaviour
{
    [Header("Tile prefabs")]
    [SerializeField] private GameObject floorTilePrefab;
    [SerializeField] private GameObject ceilingTilePrefab;

    [Header("Scrolling settings")]
    [SerializeField] private int tilesOnScreen = 12;
    [SerializeField] private float scrollSpeed = 4f;
    [SerializeField] private float yPositionFromCenter = 11.5f;
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

        // Calculate left edge of camera
        Camera cam = Camera.main;
        float halfWidth = cam.orthographicSize * cam.aspect;
        float leftEdgeX = cam.transform.position.x - halfWidth;

        // Build row of tiles
        SpawnInitialTiles(leftEdgeX);
    }


    private void Update()
    {
        ScrollTiles(floorTiles);
        ScrollTiles(ceilingTiles);
    }

    private void SpawnInitialTiles(float leftEdgeX)
    {
        floorTiles = new GameObject[tilesOnScreen];
        ceilingTiles = new GameObject[tilesOnScreen];

        for (int i = 0; i < tilesOnScreen; i++)
        {
            // Floor
            floorTiles[i] = Instantiate(
                floorTilePrefab,
                new Vector3(leftEdgeX + i * tileWidth, -yPositionFromCenter, 0f),
                Quaternion.identity,
                floorParent
            );

            // Ceiling
            ceilingTiles[i] = Instantiate(
                ceilingTilePrefab,
                new Vector3(leftEdgeX + i * tileWidth, yPositionFromCenter, 0),
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