using UnityEngine;
using System.Collections.Generic;

public class BouncerPool : MonoBehaviour
{
    [Header("Pool settings")]
    public GameObject bouncerPrefab;
    public int poolSize = 10;
    // Parent is Gameplay/Obstacles
    public Transform parentTransform;

    private List<GameObject> bouncerPool = new List<GameObject>();

    void Awake()
    {
        // Pre-create all bouncers
        for (int i = 0; i < poolSize; i++)
        {
            GameObject bouncer = Instantiate(bouncerPrefab, parentTransform);
            bouncer.SetActive(false);
            bouncerPool.Add(bouncer);
        }
    }

    public GameObject GetBouncer()
    {
        // Return bouncer if there isn't one active in hierarchy
        foreach (var bouncer in bouncerPool)
        {
            if (!bouncer.activeInHierarchy)
            {
                return bouncer;
            }
        }

        // Optional: Expand pool if all bouncers are active (unlikely but safe)
        GameObject newBouncer = Instantiate(bouncerPrefab, parentTransform);
        newBouncer.SetActive(false);
        bouncerPool.Add(newBouncer);
        return newBouncer;
    }
}