using UnityEngine;
using UnityEngine.InputSystem;

public class PauzeMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuPanel;

    private bool isPaused;

    void Awake()
    {
        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(false);
    }

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Pause()
    {
        isPaused = true;
        GameManager.IsPaused = true;
        Time.timeScale = 0f;

        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(true);
    }

    public void Resume()
    {
        isPaused = false;
        GameManager.IsPaused = false;
        Time.timeScale = 1f;

        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(false);
    }

    public void QuitToMenu()
    {
        Time.timeScale = 1f;
        GameManager.IsPaused = false;
        GameManager.Instance.StartScreen();
    }
}
