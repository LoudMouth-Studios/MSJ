using UnityEngine;

// Trigger at the player's feet. Forwards contacts to PlayerMovement, which draws the player
// behind props marked with the 'sortcol' tag.
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