using UnityEngine;

public class BaseObjectController : MonoBehaviour
{
    protected GameBalancer balancer;

    protected virtual void OnEnable()
    {
        // Base behavior to be added when needed.
        balancer = GameSettingsManager.Instance.balancer;
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
            gameObject.SetActive(false);
        }
    }

    protected void DisableObject(){
        gameObject.SetActive(false);
    }
}