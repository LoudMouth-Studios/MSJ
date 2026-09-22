using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectButtonHandler : MonoBehaviour
{
    public void OnTutorialClicked()
    {
        SceneManager.LoadScene("Tutorial");
    }
    
    public void OnLevel1Clicked()
    {
        SceneManager.LoadScene("Level_1");
    }
}