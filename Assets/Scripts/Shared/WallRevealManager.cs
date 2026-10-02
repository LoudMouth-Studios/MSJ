using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WallRevealManager : MonoBehaviour
{
    private const int Max = 8;

    private static readonly List<RevealSource> sources = new();


    public static void Register(RevealSource s)
    {
        if (!sources.Contains(s))
            sources.Add(s);
    }


    public static void Unregister(RevealSource s)
    {
        sources.Remove(s);
    }


    [Header("Wall Reveal")]

    [Tooltip("Walls using this material become see-through.")]
    [SerializeField] private Material revealMaterial;

    [Tooltip("A wall hides a character when the wall's base is at most this far below the character's feet.")]
    [SerializeField] private float wallHeight = 3f;

    [SerializeField] private float fadeSpeed = 4f;


    public float WallHeight => wallHeight;


    private class Wall
    {
        public TilemapRenderer renderer;
        public readonly float[] strength = new float[Max];
        public bool inFront;
    }


    private static readonly int PointsId =
        Shader.PropertyToID("_RevealPoints");

    private static readonly int ShapeId =
        Shader.PropertyToID("_RevealShape");

    private static readonly int CountId =
        Shader.PropertyToID("_RevealCount");

    private static readonly int WallAId =
        Shader.PropertyToID("_RevealWallA");

    private static readonly int WallBId =
        Shader.PropertyToID("_RevealWallB");


    private readonly Vector4[] points =
        new Vector4[Max];

    private readonly Vector4[] shape =
        new Vector4[Max];

    private readonly List<Wall> walls =
        new();

    private readonly Dictionary<Collider2D, Wall> wallByCollider =
        new();

    private readonly List<Collider2D> colliderBuffer =
        new();

    private readonly RaycastHit2D[] hits =
        new RaycastHit2D[32];


    private MaterialPropertyBlock block;
    private ContactFilter2D solidOnly;


    private void Awake()
    {
        block = new MaterialPropertyBlock();

        solidOnly = ContactFilter2D.noFilter;
        solidOnly.useTriggers = false;

        foreach (
            var r in FindObjectsByType<TilemapRenderer>(
                FindObjectsSortMode.None
            )
        )
        {
            if (r.sharedMaterial != revealMaterial)
                continue;

            var wall = new Wall
            {
                renderer = r
            };

            walls.Add(wall);

            bool hasSolidCollider = false;

            colliderBuffer.Clear();

            r.GetComponents(colliderBuffer);

            foreach (Collider2D c in colliderBuffer)
            {
                if (c.isTrigger)
                    continue;

                wallByCollider[c] = wall;
                hasSolidCollider = true;
            }

            if (!hasSolidCollider)
            {
                Debug.LogWarning(
                    $"WallReveal: '{r.name}' has no solid collider, so it can't detect characters behind it.",
                    r
                );
            }
        }
    }


    private void Update()
    {
        int n = Mathf.Min(
            sources.Count,
            Max
        );

        float step =
            fadeSpeed *
            Time.deltaTime;


        for (int i = 0; i < Max; i++)
        {
            if (i < n)
            {
                RevealSource source =
                    sources[i];

                Vector2 center =
                    source.Center;

                points[i] =
                    new Vector4(
                        center.x,
                        center.y,
                        source.Radius,
                        0f
                    );

                shape[i] =
                    new Vector4(
                        source.Inner,
                        source.Falloff,
                        0f,
                        0f
                    );

                Wall frontWall =
                    MarkWallsInFront(
                        source.Feet
                    );

                if (frontWall != null)
                {
                    int wallSortingOrder =
                        frontWall.renderer.sortingOrder;

                    source.SetBehindWall(
                        wallSortingOrder
                    );
                }
                else
                {
                    source.SetNormalSorting();
                }
            }
            else
            {
                points[i] = Vector4.zero;
                shape[i] = Vector4.zero;
            }


            foreach (Wall wall in walls)
            {
                wall.strength[i] =
                    Mathf.MoveTowards(
                        wall.strength[i],
                        wall.inFront ? 1f : 0f,
                        step
                    );

                wall.inFront = false;
            }
        }


        Shader.SetGlobalVectorArray(
            PointsId,
            points
        );

        Shader.SetGlobalVectorArray(
            ShapeId,
            shape
        );

        Shader.SetGlobalFloat(
            CountId,
            n
        );


        foreach (Wall wall in walls)
        {
            float[] strength =
                wall.strength;

            wall.renderer.GetPropertyBlock(
                block
            );

            block.SetVector(
                WallAId,
                new Vector4(
                    strength[0],
                    strength[1],
                    strength[2],
                    strength[3]
                )
            );

            block.SetVector(
                WallBId,
                new Vector4(
                    strength[4],
                    strength[5],
                    strength[6],
                    strength[7]
                )
            );

            wall.renderer.SetPropertyBlock(
                block
            );
        }
    }


    private Wall MarkWallsInFront(Vector2 feet)
    {
        int count =
            Physics2D.Raycast(
                feet,
                Vector2.down,
                solidOnly,
                hits,
                wallHeight
            );

        Wall closestWall = null;
        float closestDistance =
            float.MaxValue;


        for (int h = 0; h < count; h++)
        {
            if (!wallByCollider.TryGetValue(
                    hits[h].collider,
                    out Wall wall))
            {
                continue;
            }

            wall.inFront = true;

            if (hits[h].distance < closestDistance)
            {
                closestDistance =
                    hits[h].distance;

                closestWall = wall;
            }
        }


        return closestWall;
    }


    private void OnDisable()
    {
        Shader.SetGlobalFloat(
            CountId,
            0
        );

        foreach (RevealSource source in sources)
        {
            if (source != null)
                source.SetNormalSorting();
        }
    }
}