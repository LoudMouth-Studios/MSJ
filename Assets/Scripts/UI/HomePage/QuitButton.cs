using UnityEngine;

// Closes the game (stops Play mode in the Editor).
public class QuitButton : MonoBehaviour
{
    public void OnQuitButtonClicked()
    {
#if UNITY_EDITOR
        // Application.Quit() does nothing in the Editor, so stop Play mode instead
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}