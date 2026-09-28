using UnityEngine;

public class RestartButtonHandler : MonoBehaviour
{
    public void OnRestartClicked()
    {
        Debug.Log("restart button clicked.");
        GameManager.Instance.RestartLevel();
    }
}