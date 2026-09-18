using UnityEngine;

public class PlayerFloor : MonoBehaviour
{
    [Header("Player Collision")]
    [SerializeField] private GameObject playerCollision;

    [Header("Collision Layers")]
    [SerializeField] private int topLayer = 6;
    [SerializeField] private int bottomLayer = 7;

    private void Start()
    {
        // Player starts on the top floor.
        SetTopFloor();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("TopTrigger"))
        {
            SetTopFloor();
        }
        else if (other.CompareTag("BottomTrigger"))
        {
            SetBottomFloor();
        }
    }

    private void SetTopFloor()
    {
        playerCollision.layer = topLayer;
        Debug.Log("Player is now on TOP floor");
    }

    private void SetBottomFloor()
    {
        playerCollision.layer = bottomLayer;
        Debug.Log("Player is now on BOTTOM floor");
    }
}