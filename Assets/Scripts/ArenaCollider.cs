using UnityEngine;

public class CircularArena : MonoBehaviour
{
    [SerializeField] private float radius = 5f;

    private void Awake()
    {
        // Create the boundary
        CreateCircularBoundary();
    }

    private void CreateCircularBoundary()
    {
        // Add required components
        var rigidbody2D = gameObject.AddComponent<Rigidbody2D>();
        rigidbody2D.bodyType = RigidbodyType2D.Static;

        var compositeCollider = gameObject.AddComponent<CompositeCollider2D>();

        // Create an empty child object for the edge collider
        var edgeObject = new GameObject("CircleBoundary");
        edgeObject.transform.SetParent(transform);

        // Add edge collider
        var edgeCollider = edgeObject.AddComponent<EdgeCollider2D>();
        edgeCollider.usedByComposite = true;

        // Generate points for the circle
        Vector2[] points = new Vector2[33]; // 32 segments + 1 to close the loop
        for (int i = 0; i < 33; i++)
        {
            float angle = (i * 360f / 32f) * Mathf.Deg2Rad;
            points[i] = new Vector2(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius);
        }

        edgeCollider.points = points;
    }
}