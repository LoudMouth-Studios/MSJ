using UnityEngine;

public class PlayerFloor : MonoBehaviour
{
    public GameObject collisionBottom;
    public GameObject collisionTop;

    public int playerHeight = 0;

    void Start()
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