using UnityEngine;
public static class ShapeDrawer
{
    
    public static void DrawDebugCircle(Vector3 center, float radius, Color color, float duration)
    {
        float angle = 0f;

        int segments = 8;

        for (int i = 0; i < segments; i++)
        {
            float nextAngle = angle + (360f / segments);

            Vector3 p1 = center + new Vector3(
                Mathf.Cos(angle * Mathf.Deg2Rad) * radius,
                Mathf.Sin(angle * Mathf.Deg2Rad) * radius,
                0f);

            Vector3 p2 = center + new Vector3(
                Mathf.Cos(nextAngle * Mathf.Deg2Rad) * radius,
                Mathf.Sin(nextAngle * Mathf.Deg2Rad) * radius,
                0f);

            Debug.DrawLine(p1, p2, color, duration);
            angle = nextAngle;
        }
    }
}