using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PatrolMover : MonoBehaviour
{
    [SerializeField] Transform[] waypoints;
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] float waypointRadius = 0.05f;
    [SerializeField] bool pingPong = true; // false = loop back to Waypoint 1

    Rigidbody2D rb;
    SpriteRenderer sr;
    int targetIndex = 0;
    int direction = 1;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        if (waypoints == null || waypoints.Length == 0) return;

        Vector2 target = waypoints[targetIndex].position;
        Vector2 toTarget = target - rb.position;

        if (toTarget.magnitude <= waypointRadius)
        {
            AdvanceWaypoint();
            return;
        }

        Vector2 delta = toTarget.normalized * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + delta);

        if (sr != null && Mathf.Abs(toTarget.x) > 0.01f)
            sr.flipX = toTarget.x < 0;
    }

    void AdvanceWaypoint()
    {
        if (pingPong)
        {
            if (targetIndex == waypoints.Length - 1) direction = -1;
            else if (targetIndex == 0) direction = 1;
            targetIndex += direction;
        }
        else
        {
            targetIndex = (targetIndex + 1) % waypoints.Length;
        }
    }

    void OnDrawGizmos()
    {
        if (waypoints == null) return;
        Gizmos.color = Color.yellow;
        for (int i = 0; i < waypoints.Length; i++)
        {
            if (waypoints[i] == null) continue;
            Gizmos.DrawSphere(waypoints[i].position, 0.08f);
            if (i < waypoints.Length - 1 && waypoints[i + 1] != null)
                Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
        }
    }
}