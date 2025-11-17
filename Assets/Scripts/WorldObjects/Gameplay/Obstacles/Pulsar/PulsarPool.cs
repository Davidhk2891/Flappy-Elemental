using System.Collections.Generic;
using UnityEngine;

public class PulsarPool : MonoBehaviour
{

    public GameObject pulsarPrefab;
    public int poolSize = 10;
    private List<GameObject> pulsarPool;

    public Transform parentTransform;

    private void Awake()
    {
        pulsarPool = new List<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject pulsar = Instantiate(pulsarPrefab, parentTransform);
            pulsar.SetActive(false);
            pulsarPool.Add(pulsar);
        }
    }

    public GameObject GetPulsar()
    {
        // Return pulsar if there isn't one in the hierarchy
        foreach(GameObject pulsar in pulsarPool)
        {
            if (!pulsar.activeInHierarchy)
            {
                return pulsar;
            }
        }

        // OPTIONAL: Expand pool if needed
            GameObject newPulsar = Instantiate(pulsarPrefab, parentTransform);
            newPulsar.SetActive(false);
            pulsarPool.Add(newPulsar);
            return newPulsar;
    }
}
