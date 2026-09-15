using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    readonly List<IInteractable> targetsInRange = new List<IInteractable>();

    public bool HasTarget => targetsInRange.Count > 0;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out IInteractable interactable) && !targetsInRange.Contains(interactable))
            targetsInRange.Add(interactable);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out IInteractable interactable))
            targetsInRange.Remove(interactable);
    }

    public void TryInteract()
    {
        if (targetsInRange.Count > 0)
            targetsInRange[0].Interact();
    }
}