using UnityEngine;

public class FloorCollision : MonoBehaviour
{
    [Header("Collision")]
    [SerializeField] private GameObject collisionBottom;
    [SerializeField] private GameObject collisionTop;

    public void SetFloor(int floor)
    {
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