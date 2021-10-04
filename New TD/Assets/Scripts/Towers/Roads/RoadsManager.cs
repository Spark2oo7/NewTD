using UnityEngine;

public class RoadsManager : MonoBehaviour // отвечает за дороги
{
    public bool[,] grid;

    void Start()
    {
        grid = new bool[PlayerStats.gridSize*2, PlayerStats.gridSize*2];
    }

    public void InstantRoad(Vector3Int position)
    {
        grid[position.x + PlayerStats.gridSize, position.y + PlayerStats.gridSize] = true;
    }
}
