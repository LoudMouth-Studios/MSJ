using UnityEngine;

public class ExitDoor : MonoBehaviour, IInteractable
{
    public bool CanInteract => GameManager.Instance != null && GameManager.Instance.HasDiamond;

    public void Interact()
    {
        if (!CanInteract)
            return;

        GameManager.Instance.FinishLevel();
    }
}