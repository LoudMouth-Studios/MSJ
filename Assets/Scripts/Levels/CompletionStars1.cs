using UnityEngine;
using UnityEngine.UI;

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