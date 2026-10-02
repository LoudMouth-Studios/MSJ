using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class PauzeMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Button cheatButton;
    [SerializeField] private TMP_Text cheatButtonText;

    private bool isPaused;

    void Awake()
    {
        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(false);

        if (musicSlider != null)
        {
            musicSlider.SetValueWithoutNotify(VolumeManager.GetInstance().MusicVolume);
            musicSlider.onValueChanged.AddListener(VolumeManager.GetInstance().SetMusicVolume);
        }

        if (sfxSlider != null)
        {
            sfxSlider.SetValueWithoutNotify(VolumeManager.GetInstance().SFXVolume);
            sfxSlider.onValueChanged.AddListener(VolumeManager.GetInstance().SetSFXVolume);
        }
        
        if (cheatButton != null)
            cheatButton.onClick.AddListener(ToggleCheats);
        UpdateCheatText();
    }

    public void ToggleCheats()
    {
        GameManager.CheatsEnabled = !GameManager.CheatsEnabled;
        UpdateCheatText();
    }

    private void UpdateCheatText()
    {
        if (cheatButtonText != null)
            cheatButtonText.text = GameManager.CheatsEnabled ? "Cheats: ON" : "Cheats: OFF";
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
        Debug.Log("Game paused");

        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(true);
    }

    public void Resume()
    {
        isPaused = false;
        GameManager.IsPaused = false;
        Time.timeScale = 1f;
        Debug.Log("Game resumed");

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
