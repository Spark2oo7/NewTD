using UnityEngine;
using UnityEngine.Tilemaps;

public class Mina : MonoBehaviour
{
    public Tower tower;
    public string enemyTag;
    public float maxDistance;
    public AnimationCurve damageFromDistance;
    public float delay;
    public GameObject explosionParticles;
    public bool active = false;
    public Tilemap map_t;
    public Vector3Int CellPosition;
    public TileBase activeTile;

    void Start()
    {
        map_t = tower.map;
        CellPosition = map_t.WorldToCell(transform.position);
        Invoke("Activation", delay);
    }

    private void Activation()
    {
        active = true;
        map_t.SetTile(CellPosition, activeTile);
    }

    private void Explosion()
    {
        tower.Attack(10000000);
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        
        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < maxDistance)
            {
                Enemy enemyScript = enemy.gameObject.GetComponent<Enemy>();
                enemyScript.Attack(damageFromDistance.Evaluate(distanceToEnemy));
            }
        }
        
        if (PlayerStats.particlesEnabled)
            Instantiate(explosionParticles, transform.position, transform.rotation);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (active)
        {
            if (collision.gameObject.tag == enemyTag)
            {
                Explosion();
            }
        }
    }
}
