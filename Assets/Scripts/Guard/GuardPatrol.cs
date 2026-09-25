using System.Security.Cryptography;
using UnityEngine;

public enum PatrolMode { Loop, PingPong }

[System.Serializable]
public class TurnPoint
{
    public Transform point;                          // must be one of this guard's waypoints
    [Range(0f, 100f)] public float turnChance = 50f; // % chance to turn around here
}

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
    [SerializeField] Animator animator;
    
    [Header("Turn-around Points")]
    [SerializeField] TurnPoint[] turnPoints;

    Rigidbody2D rb;
    int currentIndex = 0;
    int direction = 1; // +1 forward, -1 backward (PingPong only)
    float waitTimer;
    string currentAnimation;

    public Vector2 FacingDirection { get; private set; } = Vector2.down;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (animator == null)
            animator = GetComponentInChildren<Animator>();
        
    }
    
    float GetTurnChance(Transform waypoint)
    {
        if (turnPoints == null) return 0f;

        foreach (TurnPoint tp in turnPoints)
            if (tp != null && tp.point == waypoint)
                return tp.turnChance;

        return 0f; // not a turn point, never turns here
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

            float chance = GetTurnChance(target);
            if (chance > 0f && Random.value * 100f <= chance)
                direction *= -1; // turn around: the next waypoint is the one we came from

            AdvanceIndex();
            return;
        }

        Vector2 step = toTarget.normalized * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + step);
        FacingDirection = toTarget.normalized;

        if (vision != null)
            vision.SetFacing(FacingDirection);

        UpdateAnimation(FacingDirection);
    }

    void UpdateAnimation(Vector2 dir)
    {
        if (animator == null) return;

        // corner states are named for the diagonal the guard is walking towards
        string stateName = dir.y >= 0f
            ? (dir.x >= 0f ? "top_right" : "top_left")
            : (dir.x >= 0f ? "bottom_right" : "bottom_left");

        if (stateName == currentAnimation) return;

        animator.Play(stateName);
        currentAnimation = stateName;
    }

    void AdvanceIndex()
    {
        if (waypoints.Length == 1) return;

        if (mode == PatrolMode.Loop)
        {
            currentIndex = (currentIndex + direction + waypoints.Length) % waypoints.Length;
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
        if (turnPoints != null)
        {
            Gizmos.color = Color.red;
            foreach (TurnPoint tp in turnPoints)
                if (tp != null && tp.point != null)
                    Gizmos.DrawSphere(tp.point.position, 0.1f);
        }
    }
}