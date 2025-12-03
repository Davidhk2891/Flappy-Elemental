using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    [Header("Background settings")]
    [SerializeField] private GameObject backgroundTilePrefab;
    [SerializeField] private Transform backgroundParent;

    [Header("Scrolling settings")]
    [SerializeField] private float scrollSpeed = 2f;

    [Header("Debug")]
    [SerializeField] private bool enableBackground = true;

    private float tileWidth;
    private GameObject[] bgTiles;

    private void Start()
    {
        if (!enableBackground)
        {
            enabled = false;
            return;
        } 

        // Measure prefab width
        SpriteRenderer sr = backgroundTilePrefab.GetComponentInChildren<SpriteRenderer>();
        tileWidth = sr.bounds.size.x;

        // Create two tiles side by side
        SpawnInitialBgTiles();
    }

    private void SpawnInitialBgTiles()
    {
        bgTiles = new GameObject[2];
        bgTiles[0] = Instantiate(backgroundTilePrefab, new Vector3(0, 0, 0), Quaternion.identity, backgroundParent);
        bgTiles[1] = Instantiate(backgroundTilePrefab, new Vector3(tileWidth, 0, 0), Quaternion.identity, backgroundParent);
    }

    private void Update()
    {
        ScrollBackgroundTiles();
    }

    private void ScrollBackgroundTiles()
    {
        for (int i = 0; i < bgTiles.Length; i++)
        {
            bgTiles[i].transform.Translate(scrollSpeed * Time.deltaTime * Vector3.left);

            // If background tile has moved completely off-screen, recycle it
            if (bgTiles[i].transform.position.x <= -tileWidth)
                ResetPosition(i);   
        }
    }

    private void ResetPosition(int position)
    {
        float rightMostX = GetRightmostTileX();
        bgTiles[position].transform.position = new Vector3(rightMostX + tileWidth, 0, 0);
    }

    private float GetRightmostTileX()
    {
        return Mathf.Max(bgTiles[0].transform.position.x, bgTiles[1].transform.position.x);
    }
}