using UnityEngine;

public enum PatrolMode { Loop, PingPong }

[RequireComponent(typeof(Rigidbody2D))]
public class GuardPatrol : MonoBehaviour
{
    [Header("Route")]
    [SerializeField] Transform[] waypoints;
    [SerializeField] PatrolMode mode = PatrolMode.Loop;
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] float waypointTolerance = 0.05f;
    [SerializeField] float waitTimeAtPoint = 0.5f;
    [SerializeField] GuardVision vision; // optional, wired in Step 5

    Rigidbody2D rb;
    int currentIndex = 0;
    int direction = 1; // +1 forward, -1 backward (PingPong only)
    float waitTimer;

    public Vector2 FacingDirection { get; private set; } = Vector2.down;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (waypoints == null || waypoints.Length == 0) return;

        if (waitTimer > 0f)
        {
            waitTimer -= Time.fixedDeltaTime;
            return;
        }

        Transform target = waypoints[currentIndex];
        Vector2 toTarget = (Vector2)target.position - rb.position;

        if (toTarget.magnitude <= waypointTolerance)
        {
            waitTimer = waitTimeAtPoint;
            AdvanceIndex();
            return;
        }

        Vector2 step = toTarget.normalized * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + step);
        FacingDirection = toTarget.normalized;

        if (vision != null)
            vision.SetFacing(FacingDirection);
    }

    void AdvanceIndex()
    {
        if (waypoints.Length == 1) return;

        if (mode == PatrolMode.Loop)
        {
            currentIndex = (currentIndex + 1) % waypoints.Length;
        }
        else // PingPong
        {
            if (currentIndex + direction >= waypoints.Length || currentIndex + direction < 0)
                direction *= -1;

            currentIndex += direction;
        }
    }

    void OnDrawGizmosSelected()
    {
        if (waypoints == null) return;

        Gizmos.color = Color.yellow;
        for (int i = 0; i < waypoints.Length; i++)
        {
            if (waypoints[i] == null) continue;
            Gizmos.DrawWireSphere(waypoints[i].position, 0.15f);

            Transform next = mode == PatrolMode.Loop
                ? waypoints[(i + 1) % waypoints.Length]
                : (i + 1 < waypoints.Length ? waypoints[i + 1] : null);

            if (next != null)
                Gizmos.DrawLine(waypoints[i].position, next.position);
        }
    }
}