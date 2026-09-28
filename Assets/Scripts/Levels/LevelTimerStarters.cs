using UnityEngine;

public class LevelTimerStarter : MonoBehaviour
{
    [SerializeField] private LevelTimer levelTimer;

    private void Start()
    {
        levelTimer.StartTimer();
    }
}