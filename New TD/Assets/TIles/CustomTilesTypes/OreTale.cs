using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Tiles/Ore")]
public class OreTale : Tile 
{
    public Resource resource; 

    public override void GetTileData(Vector3Int location, ITilemap tilemap, ref TileData tileData)
    {
        
        tileData.sprite = sprite;
        tileData.color = Color.white;
        var m = tileData.transform;
        m.SetTRS(Vector3.zero, GetRotation(), Vector3.one);
        tileData.transform = m;
        tileData.flags = TileFlags.LockTransform;
        tileData.colliderType = ColliderType.None;
    }

    private Quaternion GetRotation()
    {
        int random = Random.Range(0, 3);
        return Quaternion.Euler(0f, 0f, 90f * random);
    }
}
