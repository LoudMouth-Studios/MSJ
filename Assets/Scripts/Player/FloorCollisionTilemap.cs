using UnityEngine;
using UnityEngine.Tilemaps;

public class FloorCollisionTilemap : MonoBehaviour
{
    [SerializeField] private int floor;

    private TilemapCollider2D tilemapCollider;

    private void Awake()
    {
        tilemapCollider = GetComponent<TilemapCollider2D>();
    }

    public void SetFloor(int currentFloor)
    {
        tilemapCollider.enabled = floor == currentFloor;
    }
}