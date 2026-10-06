using UnityEngine;

// Home screen Play button: fades the spotlight out, then transitions to Level Select.
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

