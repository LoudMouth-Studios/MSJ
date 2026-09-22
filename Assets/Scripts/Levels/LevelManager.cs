using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private DialogueManager dialogueManager;

    private void Start()
    {
        dialogueManager.StartDialogue();
    }
}