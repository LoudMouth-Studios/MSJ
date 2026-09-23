using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    string currentLevelName;
    public bool HasDiamond { get; private set; }
    public static bool IsPaused { get; set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += (scene, mode) => HasDiamond = false;
    }

    public void CollectDiamond()
    {
        HasDiamond = true;
    }

    public void StartScreen()
    {
        SceneManager.LoadScene("StartScene");
    }

    public void FinishLevel()
    {
        SceneManager.LoadScene("Diamond stolen");
    }
    
    public void TriggerDefeat()
    {
        currentLevelName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("Defeat");
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(currentLevelName);
    }
}