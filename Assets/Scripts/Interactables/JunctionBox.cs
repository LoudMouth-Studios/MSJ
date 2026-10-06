using UnityEngine;

// Opens the light-pattern minigame. Once it's solved, the linked diamond lights turn off.
public class JunctionBox : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject[] diamondLights;

    private bool used;

    public bool CanInteract => !used;

    public void Interact()
    {
        if (used)
            return;

        if (LightPatternMinigame.Instance != null)
        {
            LightPatternMinigame.Instance.Open(this);
        }
        else
        {
            Debug.LogWarning("LightPatternMinigame is missing from the scene.");
        }
    }

    public void MinigameCompleted()
    {
        if (used)
            return;

        used = true;

        foreach (GameObject light in diamondLights)
        {
            if (light != null)
            {
                light.SetActive(false);
            }
        }
    }
}