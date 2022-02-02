using UnityEngine;
using UnityEngine.Tilemaps;

public class TowerParameters : MonoBehaviour
{
    public TileBase tile;
    public Put[] price;
    public Sprite icon;
    public int index;
    public GameObject towerObject;

    public enum Type
    {
        tower, 
        road,
        delete
    };
    public Type type;
}
