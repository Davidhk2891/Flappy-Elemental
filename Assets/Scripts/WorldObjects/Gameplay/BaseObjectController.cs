using UnityEngine;

public class BaseObjectController : MonoBehaviour
{
    [Header("Common Movement")]
    public float moveSpeedX = 10f;
    public float deadZone = -10f;

    protected virtual void OnEnable()
    {
        // Nothing yet. Override
    }

    protected virtual void Update()
    {
        Debug.Log($"{gameObject.name} speed: {moveSpeedX}");
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