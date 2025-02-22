using UnityEngine;

public class CircularBoundary : MonoBehaviour
{
    [SerializeField] private float radius = 10f;
    [SerializeField] private int edgeColliderPoints = 32;
    private EdgeCollider2D edgeCollider;
    
    private void Awake()
    {
        edgeCollider = gameObject.GetComponent<EdgeCollider2D>();
        if (edgeCollider == null)
        {
            edgeCollider = gameObject.AddComponent<EdgeCollider2D>();
        }

        UpdateCollider();
    }

    private void UpdateCollider()
    {
        Vector2[] points = new Vector2[edgeColliderPoints + 1];

        // Generate points in reverse order for outside collision
        for (int i = 0; i <= edgeColliderPoints; i++)
        {
            float angle = ((edgeColliderPoints - i) * 360f / edgeColliderPoints) * Mathf.Deg2Rad;
            points[i] = new Vector2(
                radius * Mathf.Cos(angle),
                radius * Mathf.Sin(angle)
            );
        }

        edgeCollider.points = points;
    }

    private void OnValidate()
    {
        if (edgeCollider != null)
        {
            UpdateCollider();
        }
    }

    private void OnDrawGizmos()
    {
        // Draw the circle in the editor
        Gizmos.color = new Color(1f, 0f, 0.03f);
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    public float Radius
    {
        get => radius;
        set
        {
            radius = value;
            UpdateCollider();
        }
    }
}