using UnityEngine;

// Persistent singleton holding music/SFX volume (0-1), saved across sessions via PlayerPrefs.
public class VolumeManager : MonoBehaviour
{
    const string MusicVolumeKey = "MusicVolume";
    const string SFXVolumeKey = "SFXVolume";

    public static VolumeManager Instance { get; private set; }

    public static event System.Action<float> OnMusicVolumeChanged;
    public static event System.Action<float> OnSFXVolumeChanged;

    public float MusicVolume { get; private set; } = 1f;
    public float SFXVolume { get; private set; } = 1f;

    // Auto-creates the singleton if it doesn't exist yet, so any script can safely use it first.
    public static VolumeManager GetInstance()
    {
        if (Instance == null)
        {
            var go = new GameObject("VolumeManager");
            Instance = go.AddComponent<VolumeManager>();
        }
        return Instance;
    }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        MusicVolume = PlayerPrefs.GetFloat(MusicVolumeKey, 1f);
        SFXVolume = PlayerPrefs.GetFloat(SFXVolumeKey, 1f);
    }

    // Hook these up to the pause menu's music/SFX Slider.onValueChanged events.
    public void SetMusicVolume(float value)
    {
        MusicVolume = value;
        PlayerPrefs.SetFloat(MusicVolumeKey, value);
        OnMusicVolumeChanged?.Invoke(value);
    }

    // PlayerPrefs are only written to disk automatically on a clean quit. On mobile the OS can
    // kill a backgrounded app without quitting, so settings are also saved when the app is paused.
    void OnApplicationPause(bool paused)
    {
        if (paused)
            PlayerPrefs.Save();
    }

    void OnApplicationQuit()
    {
        PlayerPrefs.Save();
    }

    public void SetSFXVolume(float value)
    {
        SFXVolume = value;
        PlayerPrefs.SetFloat(SFXVolumeKey, value);
        OnSFXVolumeChanged?.Invoke(value);
    }
}

