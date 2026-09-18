using UnityEngine;

public class PlayerBottomTrigger : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;

    private void OnTriggerEnter2D(Collider2D other)
    {
        playerMovement.BottomTriggerEnter(other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        playerMovement.BottomTriggerExit(other);
    }
}