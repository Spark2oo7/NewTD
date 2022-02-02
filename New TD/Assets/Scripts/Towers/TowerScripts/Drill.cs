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
                Resource resource = ((OreTale)map_o.GetTile(CellPosition)).resource;
                output[0].resource = resource;
                warehouse.exports[0].resource = resource;
                warehouse.Inscribe();
                InvokeRepeating("Сonversion", 1f, 1f);
            }
        }
    }
}
