using UnityEngine;
using UnityEngine.UI;

// Shows on the Complete screen as many stars as the finished level earned.
public class CompletionStars : MonoBehaviour
{
    [SerializeField] private Image[] stars;

    private void Start()
    {
        int starCount = GameManager.Instance.LastLevelStars;

        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].gameObject.SetActive(i < starCount);
        }
    }
}