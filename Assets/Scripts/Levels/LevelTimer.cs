using UnityEngine;
using TMPro;

public class LevelTimer : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text timerText;

    [Header("Star Times")]
    [SerializeField] private float threeStarTime = 15f;
    [SerializeField] private float twoStarTime = 20f;
    [SerializeField] private float oneStarTime = 25f;

    private float startTime;
    private float elapsedTime;

    private bool isRunning = false;

    public int CurrentStars { get; private set; } = 0;

    public float ElapsedTime => elapsedTime;

    private void Start()
    {
        elapsedTime = 0f;
        CurrentStars = 0;

        UpdateTimerDisplay();
    }

    private void Update()
    {
        if (!isRunning)
            return;

        elapsedTime = Time.time - startTime;

        UpdateTimerDisplay();
    }

    public void StartTimer()
    {
        if (isRunning)
            return;

        startTime = Time.time;
        elapsedTime = 0f;
        isRunning = true;

        Debug.Log("Level timer started!");
    }

    public int StopTimer()
    {
        if (!isRunning)
            return CurrentStars;

        elapsedTime = Time.time - startTime;
        isRunning = false;

        CurrentStars = CalculateStars(elapsedTime);

        UpdateTimerDisplay();

        Debug.Log(
            "Level completed in " +
            elapsedTime.ToString("F2") +
            " seconds. Stars: " +
            CurrentStars
        );

        return CurrentStars;
    }

    private int CalculateStars(float time)
    {
        if (time <= threeStarTime)
        {
            return 3;
        }

        if (time <= twoStarTime)
        {
            return 2;
        }

        if (time <= oneStarTime)
        {
            return 1;
        }

        return 0;
    }

    private void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        float seconds = elapsedTime % 60f;

        timerText.text = $"{minutes:00}:{seconds:00.00}";
    }
}