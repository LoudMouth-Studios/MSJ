using UnityEngine;

public class ExitDoor : MonoBehaviour, IInteractable
{
    [SerializeField] private LevelTimer levelTimer;

    public bool CanInteract =>
        GameManager.Instance != null &&
        GameManager.Instance.HasDiamond;

    public void Interact()
    {
        if (!CanInteract)
            return;
        
        int stars = levelTimer.StopTimer();

        Debug.Log("Level completed with " + stars + " stars.");
        
        GameManager.Instance.FinishLevel();
    }
}