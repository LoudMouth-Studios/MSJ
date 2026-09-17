using UnityEngine;

public class Diamond : MonoBehaviour, IInteractable
{
    bool collected;

    public void Interact()
    {
        if (collected) return;
        collected = true;

        // TODO: show a "carrying diamond" icon on the Player here.
        gameObject.SetActive(false); // removes the diamond tile from the level
    }
}