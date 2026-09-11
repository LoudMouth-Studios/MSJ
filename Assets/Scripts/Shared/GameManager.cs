using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    string currentLevelName;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
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