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
    [SerializeField, Range(0f, 1f)] float innerAngleRatio = 0.47f; // soft inner edge, purely cosmetic
    [SerializeField] bool castShadows = true;

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
        CanSeeTarget = target != null && HasLineOfSight(target);
    }

    void AimFlashlight()
    {
        float angle = Mathf.Atan2(facing.y, facing.x) * Mathf.Rad2Deg;
        // Light2D's cone points along local +Y at zero rotation; our angle is
        // measured from +X, so it needs a -90 offset. See Step 4 if it looks off.
        flashlight.transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
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