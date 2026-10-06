using UnityEngine;
using TMPro;

// Level stopwatch shown in the HUD. The finish time decides the star rating (0-3).
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
    }

    public int StopTimer()
    {
        if (!isRunning)
            return CurrentStars;

        elapsedTime = Time.time - startTime;
        isRunning = false;

        CurrentStars = CalculateStars(elapsedTime);

        UpdateTimerDisplay();

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

    // Reused buffer for the timer text. It changes every frame, and SetCharArray updates the
    // text without creating a new string each frame (no garbage collection stutter on mobile).
    private readonly char[] timerChars = new char[16];

    private void UpdateTimerDisplay()
    {
        // Format "mm:ss.ff", built from whole hundredths so it can never show "00:60.00".
        int totalHundredths = Mathf.RoundToInt(elapsedTime * 100f);
        int minutes = totalHundredths / 6000;
        int seconds = (totalHundredths / 100) % 60;
        int hundredths = totalHundredths % 100;

        int length = WriteNumber(minutes, 0);
        timerChars[length++] = ':';
        length = WriteNumber(seconds, length);
        timerChars[length++] = '.';
        length = WriteNumber(hundredths, length);

        timerText.SetCharArray(timerChars, 0, length);
    }

    // Writes value with at least 2 digits (leading zero) into timerChars at 'start'; returns the next free index.
    private int WriteNumber(int value, int start)
    {
        int digits = 2;
        for (int v = value; v >= 100; v /= 10)
            digits++;

        for (int i = digits - 1; i >= 0; i--)
        {
            timerChars[start + i] = (char)('0' + value % 10);
            value /= 10;
        }

        return start + digits;
    }
}