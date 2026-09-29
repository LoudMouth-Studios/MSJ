using UnityEngine;
using UnityEngine.Rendering.Universal;

public class RevealSource : MonoBehaviour
{
    [Tooltip("The see-through circle copies this light's position, radius and falloff.")]
    public Light2D matchLight;

    [Tooltip("Offset from this object's pivot to its feet.")]
    public Vector2 feetOffset;

    [HideInInspector] public float strength; // 0 = no hole, 1 = full hole

    public Vector2 Center => matchLight.transform.position;
    public Vector2 Feet => (Vector2)transform.position + feetOffset;
    public float Radius => matchLight.pointLightOuterRadius;
    public float Inner => matchLight.pointLightInnerRadius / Mathf.Max(matchLight.pointLightOuterRadius, 0.0001f);
    public float Falloff => Mathf.Lerp(0.5f, 3f, matchLight.falloffIntensity);

    private void OnEnable()  => WallRevealManager.Register(this);
    private void OnDisable() => WallRevealManager.Unregister(this);

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(Feet, 0.05f);
    }
}
