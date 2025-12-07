using UnityEngine;

public class BattyPool : BaseObjectPool
{
    protected override void Start()
    {
        base.Start();
        PoolSize = 15;
    }
}