using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeButton : MonoBehaviour
{
    public void OnHomeButtonClicked()
    {
        Debug.Log("Home button clicked.");
        SceneManager.LoadScene("HomeScreen");
        Time.timeScale = 1f;
        GameManager.IsPaused = false;
        SceneManager.LoadScene("HomeScreen");
    }
}
