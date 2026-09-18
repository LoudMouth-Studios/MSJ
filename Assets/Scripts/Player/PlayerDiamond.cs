using UnityEngine;

public class Diamond : MonoBehaviour, IInteractable
{
    bool collected;

    public bool CanInteract => !collected;

    public void Interact()
    {
        if (collected) return;
        collected = true;

        GameManager.Instance?.CollectDiamond();
        gameObject.SetActive(false);
    }
}