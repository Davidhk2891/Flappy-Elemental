using System.Collections;
using UnityEngine;

public class BattyController : BaseObjectController, IEnemy
{
    [Header("Batty settings")]
    [SerializeField] private int packNumberMin = 1;
    [SerializeField] private int packNumberMax = 5;
    [SerializeField] private SpawnBounds spawnBounds;
    [SerializeField] private float flapDuration = 1f;
    public Sprite[] battyAnimation = new Sprite[2];
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private float randomY;
    private Coroutine battyAnimCoroutine;

    public void onSpawn(SpawnBounds bounds)
    {
        spawnBounds = bounds;

        randomY = Random.Range(spawnBounds.BottomLimit, spawnBounds.TopLimit);

        var spawnPosition = new Vector3(transform.position.x, randomY, 0f);

        transform.position = spawnPosition;
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        rb = GetComponent<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
        sr.sprite = battyAnimation[0];

        if (battyAnimCoroutine != null)
            StopCoroutine(battyAnimCoroutine);
        
        battyAnimCoroutine = StartCoroutine(BattyMovementAnimation());
    }

    private void OnDisable()
    {
        if (battyAnimCoroutine != null)
        {
            StopCoroutine(battyAnimCoroutine);
            battyAnimCoroutine = null;
        }
    }

    private void Start()
    {
    
    }

    protected override void Update()
    {
        base.Update();
    }

    private IEnumerator BattyMovementAnimation()
    {
        float frameTime = flapDuration / battyAnimation.Length;

        for (int i = 1; i < battyAnimation.Length; i++)
        {
            sr.sprite = battyAnimation[i];
            yield return new WaitForSeconds(frameTime);
        }

        while(true)
        {
            sr.sprite = battyAnimation[0];
            yield return new WaitForSeconds(frameTime);

            sr.sprite = battyAnimation[1];
            yield return new WaitForSeconds(frameTime);
        }
    }

    private void spawnBatchOfBats()
    {
        int batch = Random.Range(packNumberMin, packNumberMax);
    }
}
