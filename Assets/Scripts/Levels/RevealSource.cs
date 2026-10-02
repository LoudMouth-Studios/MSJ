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

    [Tooltip("PlayerMovement van de player. Laat leeg om automatisch te zoeken.")]
    [SerializeField] private PlayerMovement playerMovement;

    [Tooltip("Hoeveel sorting orders de player achter de muur komt.")]
    [SerializeField] private int sortingOffset = 1;
    
    [Tooltip("Voor objecten zonder PlayerMovement (bijv. guards). Laat leeg om automatisch te zoeken.")]
    [SerializeField] private SpriteRenderer spriteRenderer;

    private int defaultSortingOrder;

    public bool HasFloorInfo => playerMovement != null;
    public bool IsOnTopFloor => playerMovement != null && playerMovement.IsOnTopFloor;


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
        if (playerMovement == null)
        {
            playerMovement =
                GetComponentInParent<PlayerMovement>();
        }

        if (playerMovement == null)
        {
            if (spriteRenderer == null)
                spriteRenderer = GetComponentInChildren<SpriteRenderer>();

            if (spriteRenderer == null)
                spriteRenderer = GetComponentInParent<SpriteRenderer>();

            if (spriteRenderer != null)
            {
                defaultSortingOrder = spriteRenderer.sortingOrder;
            }
            else
            {
                Debug.LogWarning(
                    $"RevealSource '{name}' heeft geen PlayerMovement of SpriteRenderer gevonden.",
                    this
                );
            }
        }
    }


    public void SetBehindWall(int wallSortingOrder)
    {
        int sortingOrder =
            wallSortingOrder - sortingOffset;

        if (playerMovement != null)
        {
            playerMovement.SetRevealSorting(sortingOrder);
            return;
        }

        if (spriteRenderer != null)
            spriteRenderer.sortingOrder = sortingOrder;
    }


    public void SetNormalSorting()
    {
        if (playerMovement != null)
        {
            playerMovement.ClearRevealSorting();
            return;
        }

        if (spriteRenderer != null)
            spriteRenderer.sortingOrder = defaultSortingOrder;
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

        var manager =
            FindFirstObjectByType<WallRevealManager>();

        if (manager != null)
        {
            Gizmos.DrawLine(
                Feet,
                Feet + Vector2.down * manager.WallHeight
            );
        }
    }
}
