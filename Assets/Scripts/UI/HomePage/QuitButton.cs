using UnityEngine;

public class QuitButton : MonoBehaviour
{
    public void OnQuitButtonClicked()
    {
        Debug.Log("Quit button clicked, closing the game.");

#if UNITY_EDITOR
        // Application.Quit() does nothing in the Editor, so stop Play mode instead
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}