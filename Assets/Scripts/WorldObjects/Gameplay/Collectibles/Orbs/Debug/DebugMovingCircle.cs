using UnityEngine;

public class DebugMovingCircle : BaseObjectController
{
    public float radius = 0.5f;
    public Color color = Color.green;

    private void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}