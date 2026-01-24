using System.Collections;
using UnityEngine;

public class OrbController : BaseObjectController
{
    [Header("Orb settings")]
    [SerializeField] private SpawnBounds spawnBounds;
    public Sprite[] orbAnimation = new Sprite[6];
    private SpriteRenderer sr;
    private Coroutine orbAnimCoroutine;

    protected override void Update()
    {
        base.Update();
    }

    /*
    > Orb grabbing logic
      - Use trigger
      - Copy from Jetpack Joyride
      - Simply collect them for now
      - Only save on memory
      - No persistance
    */

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ReturnObject();
            run.AddOrb();
            Debug.Log($"Orbs collected this run: {run.OrbsCollected}");
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        sr = GetComponentInChildren<SpriteRenderer>();
        sr.sprite = orbAnimation[0];

        if (orbAnimCoroutine != null)
            StopCoroutine(orbAnimCoroutine);
        
        orbAnimCoroutine = StartCoroutine(OrbShineAnimation());
    }

    private void OnDisable()
    {
        if (orbAnimCoroutine != null)
        {
            StopCoroutine(orbAnimCoroutine);
            orbAnimCoroutine = null;
        }
    }

    private IEnumerator OrbShineAnimation()
    {
        float shineDuration = balancer.shineDuration;
        float frameTime = shineDuration / orbAnimation.Length;

        while (true)
        {
            for (int i = 1; i < orbAnimation.Length; i++)
            {
                sr.sprite = orbAnimation[i];
                yield return new WaitForSeconds(frameTime);
            }
        }
    }
}