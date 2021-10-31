using UnityEngine;
using UnityEngine.Tilemaps;

public class Drill : Factory
{
    public Tilemap map_o;

    public override void Start()
    {
        Vector3Int CellPosition = map_o.WorldToCell(transform.position);
        TileBase tile = map_o.GetTile(CellPosition);
        if (tile)
        {
            if (map_o.GetTile(CellPosition) is OreTale)
            {
                output[0].resource = ((OreTale)map_o.GetTile(CellPosition)).resource;
                InvokeRepeating("Сonversion", 1f, 1f);
            }
        }
    }
}
