using UnityEngine;

public class BaseObjectController : MonoBehaviour
{
    [Header("Common Movement")]
    public float moveSpeedX = 2f;
    public float deadZone = -5f;

    protected virtual void Update()
    {
        MoveLeft();
    }
    
    protected void MoveLeft()
    {
        transform.position += Vector3.left * moveSpeedX * Time.deltaTime;

        if (transform.position.x < deadZone)
        {
            gameObject.SetActive(false);
        }
    }
}