using UnityEngine;

public class PowerInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject[] diamondLights;

    public void Interact()
    {
        foreach (GameObject light in diamondLights)
        {
            if (light != null)
                light.SetActive(false);
        }
    }
}