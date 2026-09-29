using UnityEngine;

public class PowerInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject[] diamondLights;

    private bool used;

    public bool CanInteract => !used;

    public void Interact()
    {
        if (used)
            return;

        if (WireCutMinigame.Instance != null)
        {
            WireCutMinigame.Instance.Open(this);
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
                light.SetActive(false);
        }
    }
}