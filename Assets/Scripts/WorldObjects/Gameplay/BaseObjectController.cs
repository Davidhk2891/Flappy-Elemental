using UnityEngine;

public class BaseObjectController : MonoBehaviour
{
    protected GameBalancer balancer;

    protected RunSessionManager run;

    protected virtual void OnEnable()
    {
        // Base behavior to be added when needed.
        balancer = GameSettingsManager.Instance.balancer;
        run = RunSessionManager.Instance;
    }

    protected virtual void Update()
    {
        MoveLeft();
    }
    
    protected void MoveLeft()
    {
        float globalMoveSpeed = balancer.globalWorldSpeed;
        transform.position += globalMoveSpeed * Time.deltaTime * Vector3.left;

        HandleDeadZone();
    }

    protected void HandleDeadZone()
    {
        float globalObjectsDeadZone = balancer.globalObjectsDeadZone;

        if (transform.position.x < globalObjectsDeadZone)
        {
            ReturnObject();
        }
    }

    protected void ReturnObject()
    {
        if (TryGetComponent<PooledReference>(out var pr) && pr.Pool != null)
        {
            pr.Pool.ReturnObject(gameObject);
        }
        else
        {
            DisableObject();   
        }
    }

    protected void DisableObject(){
        gameObject.SetActive(false);
    }
}