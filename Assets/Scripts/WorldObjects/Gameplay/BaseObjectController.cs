using UnityEngine;

public class BaseObjectController : MonoBehaviour
{
    [Header("Common Movement")]
    public float moveSpeedX = 4f;
    public float deadZone = -10f;

    protected virtual void OnEnable()
    {
        // Nothing yet. Override
    }

    protected virtual void Update()
    {
        MoveLeft();
    }
    
    protected void MoveLeft()
    {
        transform.position += moveSpeedX * Time.deltaTime * Vector3.left;

        if (transform.position.x < deadZone)
        {
            gameObject.SetActive(false);
        }
    }
}