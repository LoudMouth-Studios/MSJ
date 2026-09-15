using UnityEngine;

public class PlayerFloor : MonoBehaviour
{
    [Header("Player Collision")]
    [SerializeField] private GameObject collisionBottom;
    [SerializeField] private GameObject collisionTop;

    public int playerHeight { get; private set; }

    private void Start()
    {
        SetFloor(0);
    }

    public void SetFloor(int floor)
    {
        playerHeight = floor;

        if (floor == 0)
        {
            collisionBottom.SetActive(true);
            collisionTop.SetActive(false);
        }
        else if (floor == 1)
        {
            collisionBottom.SetActive(false);
            collisionTop.SetActive(true);
        }
    }
}