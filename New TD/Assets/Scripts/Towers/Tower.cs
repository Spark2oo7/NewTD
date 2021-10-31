using UnityEngine;
using UnityEngine.Tilemaps;

public class Tower : MonoBehaviour
{
    public float hp;
    public Tilemap map;
    public Vector3Int CellPosition;

    void Start()
    {
        CellPosition = map.WorldToCell(transform.position);
        InvokeRepeating("Delete", 0f, 1f);
    }

    void Delete()
    {
        if (!map.GetTile(CellPosition))
        {
            Destroy(gameObject);
            return;
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
        map.SetTile(CellPosition, null);
        Destroy(gameObject);
    }
}
