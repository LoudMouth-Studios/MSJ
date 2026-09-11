using UnityEngine;

public class StairTrigger : MonoBehaviour
{
    public int targetFloor;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;

        PlayerFloor playerFloor = collision.GetComponent<PlayerFloor>();

        if (playerFloor != null)
        {
            playerFloor.SetFloor(targetFloor);
        }
    }
}