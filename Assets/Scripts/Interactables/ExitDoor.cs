using UnityEngine;

// Exit of the level. Only usable once the player has the diamond; stores the star rating and finishes the level.
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

        GameManager.Instance.SetLevelStars(stars);

        GameManager.Instance.FinishLevel();
    }
}