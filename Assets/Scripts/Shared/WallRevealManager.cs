using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WallRevealManager : MonoBehaviour
{
    private const int Max = 8;
    private static readonly List<RevealSource> sources = new();
    public static void Register(RevealSource s) { if (!sources.Contains(s)) sources.Add(s); }
    public static void Unregister(RevealSource s) => sources.Remove(s);

    [Tooltip("Walls using this material become see-through.")]
    [SerializeField] private Material revealMaterial;
    [SerializeField] private float fadeSpeed = 4f;

    private static readonly int PointsId = Shader.PropertyToID("_RevealPoints");
    private static readonly int ShapeId  = Shader.PropertyToID("_RevealShape");
    private static readonly int CountId  = Shader.PropertyToID("_RevealCount");
    private readonly Vector4[] points = new Vector4[Max];
    private readonly Vector4[] shape  = new Vector4[Max];
    private readonly List<Collider2D> wallShapes = new();

    private void Awake()
    {
        foreach (var r in FindObjectsByType<TilemapRenderer>(FindObjectsSortMode.None))
        {
            if (r.sharedMaterial != revealMaterial) continue;

            if (r.TryGetComponent(out TilemapCollider2D wallShape))
                wallShapes.Add(wallShape);
            else
                Debug.LogWarning($"WallReveal: '{r.name}' has no TilemapCollider2D, so it can't detect characters behind it.", r);
        }
    }

    private void Update()
    {
        int n = Mathf.Min(sources.Count, Max);
        for (int i = 0; i < n; i++)
        {
            RevealSource s = sources[i];
            float target = IsBehindWall(s.Feet) ? 1f : 0f;
            s.strength = Mathf.MoveTowards(s.strength, target, fadeSpeed * Time.deltaTime);

            Vector2 c = s.Center;
            points[i] = new Vector4(c.x, c.y, s.Radius, s.strength);
            shape[i]  = new Vector4(s.Inner, s.Falloff, 0f, 0f);
        }
        for (int i = n; i < Max; i++) { points[i] = Vector4.zero; shape[i] = Vector4.zero; }

        Shader.SetGlobalVectorArray(PointsId, points);
        Shader.SetGlobalVectorArray(ShapeId, shape);
        Shader.SetGlobalFloat(CountId, n);
    }

    // Behind the wall = the feet are inside the wall sprite's shape (the wall is drawn over them).
    private bool IsBehindWall(Vector2 feet)
    {
        foreach (Collider2D wallShape in wallShapes)
        {
            if (wallShape.OverlapPoint(feet)) return true;
        }
        return false;
    }

    private void OnDisable() => Shader.SetGlobalFloat(CountId, 0);
}
