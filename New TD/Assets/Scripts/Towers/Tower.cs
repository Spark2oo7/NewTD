using UnityEngine;
using UnityEngine.Tilemaps;

public class Tower : MonoBehaviour
{
    public float hp;
    public Tilemap map;
    public Vector3Int cellPosition;
    public TowerParameters parameters;

    void Start() 
    {
        if (map != null)
        {
            cellPosition = map.WorldToCell(transform.position);
        }
    }

    public void SetAll(Tilemap target_map, TowerParameters towerParameters)
    {
        parameters = towerParameters;
        map = target_map;
        if (transform.position != Vector3.zero)
        {
            cellPosition = map.WorldToCell(transform.position);
        }
    }

    public void Attack(float damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            this.DeleteTower();
        }
    }

    public void DeleteTower()
    {
        map.SetTile(cellPosition, null);
        Destroy(gameObject);
    }
}
