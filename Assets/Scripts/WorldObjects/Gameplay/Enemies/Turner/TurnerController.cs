using System.Collections;
using UnityEngine;

public class TurnerController : BaseObjectController
{
    [SerializeField] Sprite[] turnerAnimation = new Sprite[4];
    private SpriteRenderer sr;
    private Transform sprite;
    private Coroutine turnerAnimCoroutine;

    protected override void OnEnable()
    {
        base.OnEnable();

        sprite = transform.GetChild(0);
        sr = GetComponentInChildren<SpriteRenderer>();
        sr.sprite = turnerAnimation[0];

        if (turnerAnimCoroutine != null)
            StopCoroutine(turnerAnimCoroutine);
        
        turnerAnimCoroutine = StartCoroutine(TurnerMovementAnimation());
    }

    private void OnDisable()
    {
        if (turnerAnimCoroutine != null)
        {
            StopCoroutine(turnerAnimCoroutine);
            turnerAnimCoroutine = null;
        }
    }

    protected override void Update()
    {
        base.Update();
        RotateTurner();
    }

    private void RotateTurner()
    {
        // Move spirte to pivot offset
        sprite.localPosition = balancer.turnerPivotOffset;

        // Rotate the parent (so rotation affects collider)    
        transform.Rotate(0f, 0f, balancer.turnerRotationSpeed * Time.deltaTime);
    }

    private IEnumerator TurnerMovementAnimation()
    {
        float burnDuration = balancer.turnerTailAnimDuration;
        float frameTime = burnDuration / turnerAnimation.Length;

        while (true)
        {
            for (int i = 0; i < turnerAnimation.Length; i++)
            {
                sr.sprite = turnerAnimation[i];
                yield return new WaitForSeconds(frameTime);
            }
        }
    }
}