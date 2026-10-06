using UnityEngine;

// Note in the level that shows the solution for the light-pattern minigame.
public class LightPatternNote : MonoBehaviour, IInteractable
{
    public bool CanInteract => true;

    public void Interact()
    {
        if (LightPatternMinigame.Instance == null)
        {
            Debug.LogWarning("LightPatternMinigame is missing from the scene.");
            return;
        }

        bool[] pattern = LightPatternMinigame.Instance.GetCurrentPattern();

        if (pattern == null)
        {
            Debug.LogWarning("No light pattern has been generated yet.");
            return;
        }

        if (LightPatternNotePopup.Instance != null)
        {
            LightPatternNotePopup.Instance.Open(pattern);
        }
    }
}