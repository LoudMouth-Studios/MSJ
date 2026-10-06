using UnityEngine;
using UnityEngine.SceneManagement;

// Persistent game state and scene flow: diamond pickup, level stars, pause/cheat flags, defeat and restart.
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    string currentLevelName;
    public bool HasDiamond { get; private set; }
    public static bool IsPaused { get; set; }
    public static bool CheatsEnabled { get; set; }
    public int LastLevelStars { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        // Duplicates are destroyed in Awake before subscribing, so only the active instance unsubscribes.
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            Instance = null;
        }
    }

    // Every newly loaded scene (level, restart, menu) starts without the diamond.
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        HasDiamond = false;
    }

    public void CollectDiamond()
    {
        HasDiamond = true;
    }
    
    public void SetLevelStars(int stars)
    {
        LastLevelStars = stars;
    }

    public void FinishLevel()
    {
        SceneManager.LoadScene("Complete");
    }
    
    // Single entry point for losing a level (guards, traps and minigames). Remembers the
    // current level so the Restart button on the Defeat screen knows what to reload.
    public void TriggerDefeat()
    {
        if (CheatsEnabled)
            return; // cheats on: getting caught is ignored

        currentLevelName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("Defeat");
    }

    public void RestartLevel()
    {
        // currentLevelName is set in TriggerDefeat(). It is empty when the Defeat scene is
        // opened directly in the Editor, and LoadScene with an empty name would throw.
        if (string.IsNullOrEmpty(currentLevelName))
        {
            Debug.LogWarning("GameManager: no level to restart. Was the Defeat scene opened directly?");
            return;
        }

        SceneManager.LoadScene(currentLevelName);
    }
}