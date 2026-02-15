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
    public int PoolSize {get; set;} = 5;

    private readonly Queue<GameObject> pool = new();

    protected virtual void Start()
    {
        InitializePool();
    }

    private void InitializePool()
    {
        for (int i = 0; i < PoolSize; i++)
        {
            GameObject obj = Instantiate(prefab, transform);
            
            GetPoolReference(obj);

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

            GetPoolReference(obj);

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


    private void GetPoolReference(GameObject obj)
    {
        if (!obj.TryGetComponent<PooledReference>(out var pr))
                pr = obj.AddComponent<PooledReference>();
            
        // Assign BaseObjectPool reference to object's
        // 'this' refers to the current instance of the class the code is running
        pr.Pool = this;
    }
}