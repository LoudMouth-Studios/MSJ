using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    readonly List<IInteractable> targetsInRange = new List<IInteractable>();

    public bool HasTarget => targetsInRange.Exists(t => t.CanInteract);

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
        IInteractable target = targetsInRange.Find(t => t.CanInteract);
        target?.Interact();
    }
}