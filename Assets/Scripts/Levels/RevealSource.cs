using UnityEngine;
using UnityEngine.Rendering.Universal;

public class RevealSource : MonoBehaviour
{
    [Tooltip("Optional: copy position, radius and falloff from this light.")]
    public Light2D matchLight;

    [Header("Used when no light is set")]
    public float radius = 2f;
    [Range(0f, 1f)] public float innerFraction = 0f;
    public float falloffPower = 1.5f;

    [Header("Behind-wall check")]
    [Tooltip("Offset from this object's pivot to its feet.")]
    public Vector2 feetOffset;

    [HideInInspector] public float strength; // 0 = no hole, 1 = full hole

    public Vector2 Center => matchLight ? (Vector2)matchLight.transform.position : (Vector2)transform.position;
    public Vector2 Feet => (Vector2)transform.position + feetOffset;
    public float Radius => matchLight ? matchLight.pointLightOuterRadius : radius;
    public float Inner => matchLight
        ? matchLight.pointLightInnerRadius / Mathf.Max(matchLight.pointLightOuterRadius, 0.0001f)
        : innerFraction;
    public float Falloff => matchLight ? Mathf.Lerp(0.5f, 3f, matchLight.falloffIntensity) : falloffPower;

    private void OnEnable()  => WallRevealManager.Register(this);
    private void OnDisable() => WallRevealManager.Unregister(this);

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(Center, Radius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(Feet, 0.05f);
        var m = FindFirstObjectByType<WallRevealManager>();
        if (m) Gizmos.DrawLine(Feet, Feet + Vector2.down * m.CheckDistance);
    }
}