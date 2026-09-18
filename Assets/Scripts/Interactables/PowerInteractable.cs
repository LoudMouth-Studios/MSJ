using UnityEngine;

public class PowerInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject[] diamondLights;

    bool used;

    public bool CanInteract => !used;

    public void Interact()
    {
        if (used) return;
        used = true;

        foreach (GameObject light in diamondLights)
            if (light != null)
                light.SetActive(false);
    }
}