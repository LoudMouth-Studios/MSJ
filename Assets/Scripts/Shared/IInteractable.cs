// Anything the player can use with the interact button (doors, minigames, notes).
public interface IInteractable
{
    bool CanInteract { get; }
    void Interact();
}