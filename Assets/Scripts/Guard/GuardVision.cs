using UnityEngine;

public class GuardVision : MonoBehaviour
{
    [Header("Cone Shape")]
    [SerializeField] float viewDistance = 5f;
    [SerializeField] float viewAngle = 70f;       // full cone angle, in degrees
    [SerializeField] int rayCount = 40;           // mesh resolution
    [SerializeField] LayerMask obstacleMask;      // the Obstacles layer from Step 2

    [Header("Rendering")]
    [SerializeField] Color coneColor = new Color(1f, 0.9f, 0.45f, 0.35f);

    [Header("Target")]
    [SerializeField] Transform target;            // drag the Player here

    Mesh mesh;
    Vector2 facing = Vector2.down;

    public bool CanSeeTarget { get; private set; }

    void Awake()
    {
        var go = new GameObject("VisionCone");
        go.transform.SetParent(transform, false);

        var meshFilter = go.AddComponent<MeshFilter>();
        var meshRenderer = go.AddComponent<MeshRenderer>();
        meshRenderer.material = new Material(Shader.Find("Sprites/Default")) { color = coneColor };
        meshRenderer.sortingLayerID = 0;
        meshRenderer.sortingOrder = 10;

        mesh = new Mesh();
        meshFilter.mesh = mesh;
    }

    // Called every FixedUpdate by GuardPatrol with its current movement direction
    public void SetFacing(Vector2 direction)
    {
        if (direction.sqrMagnitude > 0.0001f)
            facing = direction.normalized;
    }

    void LateUpdate()
    {
        DrawConeMesh();
        CanSeeTarget = target != null && HasLineOfSight(target);
    }

    void DrawConeMesh()
    {
        float baseAngle = Mathf.Atan2(facing.y, facing.x) * Mathf.Rad2Deg;
        float startAngle = baseAngle - viewAngle / 2f;
        float angleStep = viewAngle / rayCount;

        var vertices = new Vector3[rayCount + 2];
        var triangles = new int[rayCount * 3];
        vertices[0] = Vector3.zero;

        for (int i = 0; i <= rayCount; i++)
        {
            float angle = startAngle + angleStep * i;
            Vector2 dir = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

            float dist = viewDistance;
            var hit = Physics2D.Raycast(transform.position, dir, viewDistance, obstacleMask);
            if (hit.collider != null) dist = hit.distance;

            vertices[i + 1] = transform.InverseTransformDirection((Vector3)(dir * dist));
        }

        for (int i = 0; i < rayCount; i++)
        {
            triangles[i * 3] = 0;
            triangles[i * 3 + 1] = i + 1;
            triangles[i * 3 + 2] = i + 2;
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }

    bool HasLineOfSight(Transform t)
    {
        Vector2 toTarget = (Vector2)t.position - (Vector2)transform.position;
        float distance = toTarget.magnitude;
        if (distance > viewDistance) return false;

        float angle = Vector2.Angle(facing, toTarget);
        if (angle > viewAngle / 2f) return false;

        var hit = Physics2D.Raycast(transform.position, toTarget.normalized, distance, obstacleMask);
        return hit.collider == null; // nothing blocking the way
    }
}