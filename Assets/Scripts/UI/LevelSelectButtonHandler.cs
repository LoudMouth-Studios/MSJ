using UnityEngine;
using UnityEngine.UI;

public class LevelSelectButtonHandler : MonoBehaviour
{
    [SerializeField] float zoomScale = 4f;
    [SerializeField] float zoomDuration = 0.45f;
    [SerializeField] float fadeDuration = 0.3f;

    public void OnTutorialClicked()
    {
        StartTransition("Tutorial");
    }
    
    public void OnLevel1Clicked()
    {
        StartTransition("Level_1");
    }

    void StartTransition(string sceneName)
    {
        var button = GetComponent<Button>();
        if (button != null)
        {
            button.interactable = false;
        }

        var rectTransform = GetComponent<RectTransform>();
        SceneTransitionManager.GetInstance().LoadSceneWithZoom(sceneName, rectTransform, zoomScale, zoomDuration, fadeDuration);
    }
}