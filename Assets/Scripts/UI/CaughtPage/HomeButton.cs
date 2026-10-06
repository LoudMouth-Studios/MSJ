using UnityEngine;
using UnityEngine.SceneManagement;

// Button that goes back to the home screen.
public class HomeButton : MonoBehaviour
{
    public void OnHomeButtonClicked()
    {
        // Unpause first: timeScale and IsPaused are global and survive a scene load.
        Time.timeScale = 1f;
        GameManager.IsPaused = false;
        SceneManager.LoadScene("HomeScreen");
    }
}
