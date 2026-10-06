using UnityEngine;

// Starts the level timer as soon as the scene starts.
public class LevelTimerStarter : MonoBehaviour
{
    [SerializeField] private LevelTimer levelTimer;

    private void Start()
    {
        levelTimer.StartTimer();
    }
}