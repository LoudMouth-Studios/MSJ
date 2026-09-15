using UnityEngine;

public class FloorTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.transform.root.CompareTag("Player"))
            return;

        FloorManager.Instance.ToggleFloor();
    }
}