using UnityEngine;

public class DiamondLightTrap : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        GameManager.Instance?.TriggerDefeat();
    }
}