using UnityEngine;
using UnityEngine.UI;

public class Diamond : MonoBehaviour
{
    [SerializeField] private Button interactButton;

    private bool playerInRange = false;

    private void Start()
    {
        interactButton.interactable = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            interactButton.interactable = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            interactButton.interactable = false;
        }
    }

    public void Interact()
    {
        if (!playerInRange)
            return;

        GameManager.Instance?.FinishLevel();
    }
}