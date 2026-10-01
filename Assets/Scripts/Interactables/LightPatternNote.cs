using UnityEngine;

public class LightPatternNote : MonoBehaviour, IInteractable
{
    [SerializeField] private bool[] correctPattern =
    {
        true,
        false,
        true,
        false,
        false
    };

    public bool CanInteract => true;

    public bool[] GetPattern()
    {
        return correctPattern;
    }

    public void Interact()
    {
        if (LightPatternNotePopup.Instance != null)
        {
            LightPatternNotePopup.Instance.Open(correctPattern);
        }
    }
}