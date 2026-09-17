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

    [Header("Target")]
    [SerializeField] Transform target;            // drag the Player here

    Light2D flashlight;
    Vector2 facing = Vector2.down;

    public bool CanSeeTarget { get; private set; }

    void Awake()
    {
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

        bool nowSeesTarget = target != null && HasLineOfSight(target);

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