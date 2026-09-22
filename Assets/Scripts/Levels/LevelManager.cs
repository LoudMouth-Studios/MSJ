using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private LevelTimer levelTimer;

    private bool levelStarted = false;

    private void Start()
    {
        dialogueManager.StartDialogue();
    }

    public void StartLevelTimer()
    {
        if (levelStarted)
            return;

        levelStarted = true;

        levelTimer.StartTimer();
    }

    public void CompleteLevel()
    {
        int stars = levelTimer.StopTimer();

        Debug.Log(
            "FINAL RESULT: " +
            levelTimer.ElapsedTime.ToString("F2") +
            " seconds - " +
            stars +
            " stars"
        );

        GameManager.Instance?.FinishLevel();
    }
}