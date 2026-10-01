using UnityEngine;

public class Diamond : MonoBehaviour, IInteractable
{
    private bool collected;

    public bool CanInteract => !collected;

    public void Interact()
    {
        if (collected)
            return;

        if (GlassBreakMinigame.Instance != null)
        {
            GlassBreakMinigame.Instance.Open(this);
        }
        else
        {
            Debug.LogWarning("GlassBreakMinigame is missing from the scene.");
        }
    }

    public void Collect()
    {
        if (collected)
            return;

        collected = true;

        GameManager.Instance?.CollectDiamond();

        gameObject.SetActive(false);
    }
}