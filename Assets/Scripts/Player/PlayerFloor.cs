using UnityEngine;

public class PlayerFloor : MonoBehaviour
{
    public bool IsOnTopFloor => playerCollision.layer == topLayer;
    
    [Header("Player Collision")]
    [SerializeField] private GameObject playerCollision;

    [Header("Collision Layers")]
    [SerializeField] private int topLayer = 6;
    [SerializeField] private int bottomLayer = 7;

    public bool IsOnTopFloor { get; private set; }
    
    private void Start()
    {
        // Player starts on the top floor.
        SetTopFloor();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
   
        
        if (other.CompareTag("TopTrigger"))
        {
            Debug.unityLogger.Log("Top");
            SetTopFloor();
        }
        else if (other.CompareTag("BottomTrigger"))
        {
            Debug.unityLogger.Log("Bottom");
            SetBottomFloor();
        }
    }

    private void SetTopFloor()
    {
        playerCollision.layer = topLayer;
        IsOnTopFloor = true;
    }

    private void SetBottomFloor()
    {
        playerCollision.layer = bottomLayer;
        IsOnTopFloor = false;
    }
}