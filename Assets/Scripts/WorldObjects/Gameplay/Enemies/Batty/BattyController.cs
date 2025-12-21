using System.Collections;
using UnityEngine;

public class BattyController : BaseObjectController, IEnemy
{
    [Header("Batty settings")]
    [SerializeField] private SpawnBounds spawnBounds;
    public Sprite[] battyAnimation = new Sprite[4];
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private float randomY;
    private float battySpeedMultiplier;
    private Coroutine battyAnimCoroutine;

    public void OnSpawn(SpawnBounds bounds)
    {
        spawnBounds = bounds;

        randomY = Random.Range(spawnBounds.BottomLimit, spawnBounds.TopLimit);

        var spawnPosition = new Vector3(transform.position.x, randomY, 0f);

        transform.position = spawnPosition;
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        battySpeedMultiplier = balancer.battySpeedMultiplier;

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

    protected override void Update()
    {
        // Override global speed
        float battySpeed = balancer.globalWorldSpeed * battySpeedMultiplier;
        transform.position += Vector3.left * battySpeed * Time.deltaTime;

        HandleDeadZone();
    }

    private IEnumerator BattyMovementAnimation()
    {
        float flapDuration = balancer.battyFlapDuration;
        float frameTime = flapDuration / battyAnimation.Length;

        while (true)
        {
            for (int i = 1; i < battyAnimation.Length; i++)
            {
                sr.sprite = battyAnimation[i];
                yield return new WaitForSeconds(frameTime);
            }   
        }
    }
}
