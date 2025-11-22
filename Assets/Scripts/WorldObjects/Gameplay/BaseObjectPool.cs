using System.Collections.Generic;
using UnityEngine;
/*
Base pooling class
1) Pre-instantiates a fixed number of objects (poolSize)
2) Keeps them disabled until needed
3) GetObject() returns an inactive one when requested
4) If all are active (pool is empty), it can optionally expand the pool
*/
public class BaseObjectPool : MonoBehaviour
{
    [Header("Pooling settings")]
    public GameObject prefab;
    public int poolSize = 5;

    private Queue<GameObject> pool = new Queue<GameObject>();

    protected virtual void Awake()
    {
        InitializePool();
    }

    private void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(prefab, transform);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    public GameObject GetObject()
    {
        // If pool is empty, expand it
        if (pool.Count == 0)
        {
            GameObject obj = Instantiate(prefab, transform);
            obj.SetActive(false);
            return obj;
        }

        // Dequeue object and return it
        GameObject item = pool.Dequeue();
        item.SetActive(true);
        return item;
    }

    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);
    }
}