using UnityEngine;
using UnityEngine.Rendering.Universal;

public class RevealSource : MonoBehaviour
{
    [Header("Reveal Light")]

    [Tooltip("The see-through circle copies this light's position, radius and falloff.")]
    [SerializeField] private Light2D matchLight;

    [Tooltip("Offset from this object's pivot to its feet.")]
    [SerializeField] private Vector2 feetOffset;

    [Header("Player Sorting")]

    [Tooltip("SpriteRenderer van de player. Laat leeg om automatisch een SpriteRenderer in de children te zoeken.")]
    [SerializeField] private SpriteRenderer playerRenderer;

    [Tooltip("Hoeveel sorting orders de player achter de muur komt.")]
    [SerializeField] private int sortingOffset = 1;

    private int normalSortingOrder;


    public Vector2 Center
    {
        get
        {
            if (matchLight == null)
                return transform.position;

            return matchLight.transform.position;
        }
    }

    public Vector2 Feet
    {
        get
        {
            return (Vector2)transform.position + feetOffset;
        }
    }

    public float Radius
    {
        get
        {
            if (matchLight == null)
                return 0f;

            return matchLight.pointLightOuterRadius;
        }
    }

    public float Inner
    {
        get
        {
            if (matchLight == null)
                return 0f;

            return matchLight.pointLightInnerRadius /
                   Mathf.Max(
                       matchLight.pointLightOuterRadius,
                       0.0001f
                   );
        }
    }

    public float Falloff
    {
        get
        {
            if (matchLight == null)
                return 0.5f;

            return Mathf.Lerp(
                0.5f,
                3f,
                matchLight.falloffIntensity
            );
        }
    }


    private void Awake()
    {
        if (playerRenderer == null)
        {
            playerRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        if (playerRenderer != null)
        {
            normalSortingOrder = playerRenderer.sortingOrder;
        }
        else
        {
            Debug.LogWarning(
                $"RevealSource '{name}' heeft geen SpriteRenderer gevonden.",
                this
            );
        }

        if (matchLight == null)
        {
            Debug.LogWarning(
                $"RevealSource '{name}' heeft geen Light2D ingesteld.",
                this
            );
        }
    }


    public void SetBehindWall(int wallSortingOrder)
    {
        if (playerRenderer == null)
            return;

        playerRenderer.sortingOrder =
            wallSortingOrder - sortingOffset;
    }


    public void SetNormalSorting()
    {
        if (playerRenderer == null)
            return;

        playerRenderer.sortingOrder =
            normalSortingOrder;
    }


    private void OnEnable()
    {
        WallRevealManager.Register(this);
    }


    private void OnDisable()
    {
        WallRevealManager.Unregister(this);
        SetNormalSorting();
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawSphere(
            Feet,
            0.05f
        );

        var manager = FindFirstObjectByType<WallRevealManager>();

        if (manager != null)
        {
            Gizmos.DrawLine(
                Feet,
                Feet + Vector2.down * manager.WallHeight
            );
        }
    }
}