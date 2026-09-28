using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WallRevealManager : MonoBehaviour
{
    private const int Max = 8;
    private static readonly List<RevealSource> sources = new();
    public static void Register(RevealSource s) { if (!sources.Contains(s)) sources.Add(s); }
    public static void Unregister(RevealSource s) => sources.Remove(s);
    private readonly List<Collider2D> wallShapes = new();

    [Tooltip("Walls that should become see-through (use the WallReveal material on these).")]
    [SerializeField] private Tilemap[] walls;
    [SerializeField] private int checkSteps = 6;
    [SerializeField] private float stepSize = 0.25f;
    [SerializeField] private float fadeSpeed = 4f;

    public float CheckDistance => checkSteps * stepSize;
    
    [SerializeField] private Material revealMaterial; // drag WallReveal_Mat in here (works in a prefab)

    private void Awake()
    {
        if (walls == null || walls.Length == 0)
        {
            var found = new List<Tilemap>();
            foreach (var r in FindObjectsByType<TilemapRenderer>(FindObjectsSortMode.None))
            {
                if (r.sharedMaterial == revealMaterial)
                    found.Add(r.GetComponent<Tilemap>());
            }
            walls = found.ToArray();
        }

        wallShapes.Clear();
        foreach (Tilemap w in walls)
        {
            if (w && w.TryGetComponent(out TilemapCollider2D shape))
                wallShapes.Add(shape);
            else if (w)
                Debug.LogWarning($"WallReveal: '{w.name}' has no TilemapCollider2D, so it can't detect characters behind it.", w);
        }
    }

    private static readonly int PointsId = Shader.PropertyToID("_RevealPoints");
    private static readonly int ShapeId  = Shader.PropertyToID("_RevealShape");
    private static readonly int CountId  = Shader.PropertyToID("_RevealCount");
    private readonly Vector4[] points = new Vector4[Max];
    private readonly Vector4[] shape  = new Vector4[Max];

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

    // Looks straight down (on screen) from the feet: a wall tile there means the wall is in front.
    private bool IsBehindWall(Vector2 feet)
    {
        foreach (Collider2D shape in wallShapes)
        {
            if (shape.OverlapPoint(feet)) return true;
        }
        return false;
    }

    private void OnDisable() => Shader.SetGlobalFloat(CountId, 0);
}