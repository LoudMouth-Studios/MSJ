using UnityEngine;
using UnityEngine.UI;

// On-screen interact button. Only clickable while something usable is in range.
public class InteractButtonHandler : MonoBehaviour
{
    [SerializeField] PlayerInteractor interactor;
    [SerializeField] Button button;

    void Reset()
    {
        button = GetComponent<Button>();
    }

    void Update()
    {
        if (interactor != null && button != null)
            button.interactable = interactor.HasTarget;
    }

    public void OnInteractClicked()
    {
        interactor?.TryInteract();
    }
}