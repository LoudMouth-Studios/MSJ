using UnityEngine;

public class JunctionBox : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject[] diamondLights;

    private bool used;

    public bool CanInteract => !used;

    public void Interact()
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