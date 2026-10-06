using UnityEngine;

// Tracks whether the player is on the top or bottom floor by putting the player's collision
// object on the matching physics layer, so it only collides with that floor's walls.
public class PlayerFloor : MonoBehaviour
{
    public bool IsOnTopFloor => playerCollision.layer == topLayer;
    
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

    // Floor triggers sit on the stairs: walking through one moves the player to that floor.
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("TopTrigger"))
            SetTopFloor();
        else if (other.CompareTag("BottomTrigger"))
            SetBottomFloor();
    }

    private void SetTopFloor()
    {
        playerCollision.layer = topLayer;
    }

    private void SetBottomFloor()
    {
        playerCollision.layer = bottomLayer;
    }
}