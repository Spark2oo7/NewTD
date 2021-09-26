using UnityEngine;
using UnityEngine.Tilemaps;

public class Tower : MonoBehaviour
{
    public float hp;
    public Tilemap map_t;
    public Vector3Int CellPosition;

    void Start()
    {
        CellPosition = map_t.WorldToCell(transform.position);
        InvokeRepeating("Delete", 0f, 1f);
    }

    void Delete()
    {
        if (!map_t.GetTile(CellPosition))
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
            map_t.SetTile(CellPosition, null);
            Destroy(gameObject);
            return;
        }
    }
}
