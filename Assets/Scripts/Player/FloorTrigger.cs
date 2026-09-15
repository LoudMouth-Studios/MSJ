using UnityEngine;

public class FloorTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        PlayerFloor playerFloor = other.GetComponent<PlayerFloor>();

        if (playerFloor == null)
            return;
        
        if (playerFloor.playerHeight == 0)
        {
            playerFloor.SetFloor(1);
        }
        else
        {
            playerFloor.SetFloor(0);
        }
    }
}