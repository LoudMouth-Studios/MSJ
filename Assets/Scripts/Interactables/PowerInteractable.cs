using UnityEngine;

public class PowerInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject diamondLight;

    public void Interact()
    {
        if (diamondLight != null)
            diamondLight.SetActive(false);
    }
}