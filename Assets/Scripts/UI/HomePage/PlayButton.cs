using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    public void OnPlayButtonClicked()
    {
        Debug.Log("Play button clicked, switching to Level Select scene.");
        SceneManager.LoadScene("LevelSelect");
    }
}
