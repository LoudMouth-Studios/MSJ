using UnityEngine;
using UnityEngine.SceneManagement;

// Attach to a GameObject with an AudioSource playing the menu music, in the HomeScreen scene only.
[RequireComponent(typeof(AudioSource))]
public class MenuMusic : MonoBehaviour
{
    // Scenes the music should keep playing through; it stops on any other scene (e.g. gameplay levels).
    [SerializeField] private string[] menuSceneNames = { "HomeScreen", "LevelSelect" };

    private static MenuMusic instance;
    private AudioSource audioSource;

    private void Awake()
    {
        // If music is already playing (e.g. we came back from another menu scene), don't spawn a second copy.
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        bool isMenuScene = System.Array.IndexOf(menuSceneNames, scene.name) >= 0;

        if (isMenuScene)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            audioSource.Stop();
            instance = null;
            Destroy(gameObject);
        }
    }
}
