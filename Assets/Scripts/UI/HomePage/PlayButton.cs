using UnityEngine;

public class PlayButton : MonoBehaviour
{
    [SerializeField] Spotlight spotlight;
    [SerializeField] float sceneFadeDuration = 0.6f;

    void Awake()
    {
        if (spotlight == null)
        {
            spotlight = FindFirstObjectByType<Spotlight>();
        }
    }

    public void OnPlayButtonClicked()
    {
        Debug.Log("Play button clicked, switching to Level Select scene.");

        if (spotlight != null)
        {
            spotlight.TurnOff(LoadLevelSelect);
        }
        else
        {
            LoadLevelSelect();
        }
    }

    void LoadLevelSelect()
    {
        SceneTransitionManager.GetInstance().LoadSceneWithZoom("LevelSelect", null, fadeDuration: sceneFadeDuration);
    }
}

