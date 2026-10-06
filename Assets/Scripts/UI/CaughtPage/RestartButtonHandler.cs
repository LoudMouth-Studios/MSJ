using UnityEngine;

// Defeat screen button: replays the level the player lost.
public class RestartButtonHandler : MonoBehaviour
{
    public void OnRestartClicked()
    {
        GameManager.Instance.RestartLevel();
    }
}