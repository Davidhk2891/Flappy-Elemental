using UnityEngine;
using System.Collections.Generic;

/*
1) Pre-instantiates a fixed number of pipes (poolSize)
2) Keeps them disabled until needed
3) GetPipe() returns an inactive one when requested
4) If all are active, it can optionally expand the pool

LEFT IN STEP 3
*/
public class PipePool : MonoBehaviour
{

    [Header("Pipe pool settings")]
    public GameObject pipePrefab;
    public int poolSize = 10;
    private List<GameObject> pipePool = new List<GameObject>();

    public Transform parentTransform;

    void Awake()
    {
        // Pre-creates all pipes
        for (int i = 0; i < poolSize; i++)
        {
            GameObject pipe = Instantiate(pipePrefab, parentTransform);
            pipe.SetActive(false);
            pipePool.Add(pipe);
        }
    }

    public GameObject GetPipe()
    {
        // Return pipe if there isn't one active in hierarchy
        foreach (var pipe in pipePool)
        {
            if (!pipe.activeInHierarchy)
            {
                return pipe;
            }
        }

        // Optional: Expand pool if all pipes are active (unlikely but safe)
        GameObject newPipe = Instantiate(pipePrefab);
        newPipe.SetActive(false);
        pipePool.Add(newPipe);
        return newPipe;
    }
}
