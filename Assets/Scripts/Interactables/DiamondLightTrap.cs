using UnityEngine;

// Light trap around the diamond: the player loses as soon as they touch it.
public class DiamondLightTrap : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        GameManager.Instance?.TriggerDefeat();
    }
}