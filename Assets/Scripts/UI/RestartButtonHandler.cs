using UnityEngine;

public class RestartButtonHandler : MonoBehaviour
{
    public void OnRestartClicked()
    {
        GameManager.Instance.RestartLevel();
    }
}