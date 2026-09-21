using UnityEngine;
using UnityEngine.Rendering.Universal; // Light2D lives here

public class GuardVision : MonoBehaviour
{
    [Header("Cone Shape")]
    [SerializeField] float viewDistance = 5f;
    [SerializeField] float viewAngle = 70f;       // full cone angle, in degrees
    [SerializeField] LayerMask obstacleMask;      // still used by the detection raycast below

    [Header("Flashlight")]
    [SerializeField] Color flashlightColor = new Color(1f, 0.92f, 0.75f, 1f);
    [SerializeField] float intensity = 1.2f;
    [SerializeField, Range(0f, 1f)] float innerAngleRatio = 0.47f;
    [SerializeField] bool castShadows = true;

    [Header("Flashlight Position (per facing)")]
    [SerializeField] Vector2 offsetTopRight = new Vector2(-0.5f, -0.5f);       
    [SerializeField] Vector2 offsetTopLeft = new Vector2(-0.6f, -0.8f);      
    [SerializeField] Vector2 offsetBottomRight = new Vector2(0.7f, -0.4f);
    [SerializeField] Vector2 offsetBottomLeft = new Vector2(0.4f, -0.8f);
    
    [Header("Hiding (feet-to-feet check)")]
    [SerializeField] Collider2D playerFeet;          // drag the player's BottomCollider here
    [SerializeField] PlayerFloor playerFloor;        // drag the Player (object with PlayerFloor) here
    [SerializeField] LayerMask topFloorObstacles;    // obstacles that hide the player on the top floor
    [SerializeField] LayerMask bottomFloorObstacles; // obstacles that hide the player on the bottom floor
    [SerializeField] bool checkFeetEdges = true;     // also test left/right edge of the player's feet

    [Header("Target")]
    [SerializeField] Transform target;            // drag the Player here

    Light2D flashlight;
    Collider2D guardFeet;
    Vector2 facing = Vector2.down;

    public bool CanSeeTarget { get; private set; }

    void Awake()
    {
        guardFeet = GetComponent<Collider2D>();
        
        if (playerFloor == null)
            playerFloor = FindFirstObjectByType<PlayerFloor>();

        if (playerFeet == null && playerFloor != null)
            playerFeet = playerFloor.GetComponent<Collider2D>();

        if (playerFeet == null || playerFloor == null)
            Debug.LogWarning("GuardVision: could not find player feet/floor.", this);
        
        var go = new GameObject("VisionFlashlight");
        go.transform.SetParent(transform, false);

        flashlight = go.AddComponent<Light2D>();
        flashlight.lightType = Light2D.LightType.Point;
        flashlight.color = flashlightColor;
        flashlight.intensity = intensity;

        // outerAngle is tied to viewAngle on purpose: the beam's edge
        // is always exactly where detection stops. No more drift like GuardUp had.
        flashlight.pointLightOuterAngle = viewAngle;
        flashlight.pointLightInnerAngle = viewAngle * innerAngleRatio;
        flashlight.pointLightOuterRadius = viewDistance;
        flashlight.pointLightInnerRadius = 0f;

        flashlight.shadowsEnabled = castShadows;
        flashlight.shadowIntensity = 0.75f;
        flashlight.shadowSoftness = 0.3f;
    }

    // Called every FixedUpdate by GuardPatrol with its current movement direction
    public void SetFacing(Vector2 direction)
    {
        if (direction.sqrMagnitude > 0.0001f)
            facing = direction.normalized;
    }

    void LateUpdate()
    {
        AimFlashlight();

        bool nowSeesTarget = playerFeet != null && HasLineOfSight();
        
        if (nowSeesTarget && !CanSeeTarget)
            GameManager.Instance?.TriggerDefeat();

        CanSeeTarget = nowSeesTarget;
    }

    void AimFlashlight()
    {
        flashlight.transform.localPosition = GetOffsetForFacing(facing);

        float angle = Mathf.Atan2(facing.y, facing.x) * Mathf.Rad2Deg;
        flashlight.transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
    }

    Vector2 GetOffsetForFacing(Vector2 dir)
    {
        return dir.y >= 0f
            ? (dir.x >= 0f ? offsetTopRight : offsetTopLeft)
            : (dir.x >= 0f ? offsetBottomRight : offsetBottomLeft);
    }

    bool HasLineOfSight()
    {
        Vector2 from = guardFeet.bounds.center;
        Bounds pb = playerFeet.bounds;

        // Points on the player's feet to test. Caught if ANY of them is visible.
        Vector2 center = pb.center;
        if (IsPointVisible(from, center)) return true;

        if (checkFeetEdges)
        {
            if (IsPointVisible(from, new Vector2(pb.min.x, pb.center.y))) return true;
            if (IsPointVisible(from, new Vector2(pb.max.x, pb.center.y))) return true;
        }
        return false;
    }

    bool IsPointVisible(Vector2 from, Vector2 to)
    {
        Vector2 toTarget = to - from;
        float distance = toTarget.magnitude;

        // 1. inside the cone?
        if (distance > viewDistance) return false;
        if (Vector2.Angle(facing, toTarget) > viewAngle / 2f) return false;

        // 2. anything between the two pairs of feet?
        LayerMask mask = (playerFloor != null && playerFloor.IsOnTopFloor)
            ? topFloorObstacles
            : bottomFloorObstacles;

        RaycastHit2D[] hits = Physics2D.LinecastAll(from, to, mask);

        foreach (RaycastHit2D h in hits)
        {
            // skip the player's own feet and the guard's own collider
            if (h.collider == playerFeet || h.collider == guardFeet) continue;

            // the first other collider on the line blocks the view
            Debug.DrawLine(from, h.point, Color.red);
            return false;
        }

        Debug.DrawLine(from, to, Color.green);
        return true;
    }
}