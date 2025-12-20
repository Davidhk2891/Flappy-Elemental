using UnityEngine;

public class BackgroundTileController : MonoBehaviour
{
    [SerializeField] private Sprite[] tileSprites;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private void OnEnable()
    {
        // Pick a random tile
        spriteRenderer.sprite = tileSprites[Random.Range(0, tileSprites.Length)];
    }
}