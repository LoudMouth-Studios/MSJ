using UnityEngine;

public class MainMenuButtonHandler : MonoBehaviour
{
    public void OnMainClicked()
    {
        GameManager.Instance.StartScreen();
    }
}