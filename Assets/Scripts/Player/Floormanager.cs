using UnityEngine;

public class FloorManager : MonoBehaviour
{
    public static FloorManager Instance;

    public int currentFloor { get; private set; }

    private PlayerFloor playerFloor;
    private FloorCollision[] floorObjects;

    private void Awake()
    {
        Instance = this;

        playerFloor = FindFirstObjectByType<PlayerFloor>();

        floorObjects = FindObjectsByType<FloorCollision>(
            FindObjectsInactive.Include,
            FindObjectsSortMode.None
        );
    }

    private void Start()
    {
        SetFloor(0);
    }

    public void SetFloor(int floor)
    {
        currentFloor = floor;

        // Change player's collision
        if (playerFloor != null)
        {
            playerFloor.SetFloor(floor);
        }

        // Change all map collisions
        foreach (FloorCollision floorObject in floorObjects)
        {
            floorObject.SetFloor(floor);
        }
    }

    public void ToggleFloor()
    {
        if (currentFloor == 0)
        {
            SetFloor(1);
        }
        else
        {
            SetFloor(0);
        }
    }
}