using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Transform target = null;
    public string enemyTag;
    public float speed;
    public float range;
    public float damage;
    private float gridSize;
    
    void Start()
    {
        gridSize = PlayerStats.gridSize + 0.5f;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }
        if (nearestEnemy == null)
        {
            Destroy(gameObject);
            return;
        }
        target = nearestEnemy.transform;

        transform.LookAt(target);
        transform.Rotate(Random.Range(-range, range), 0, 0);
    }


    void Update()
    {

        if (transform.position.x > gridSize || transform.position.x < -gridSize || transform.position.y > gridSize || transform.position.y < -gridSize)
        {
            Destroy(gameObject);
            return;
        }

        transform.Translate(transform.right * speed * Time.deltaTime * -transform.rotation.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == enemyTag)
        {
            collision.gameObject.GetComponent<Enemy>().Attack(damage);
            Destroy(gameObject);
            return;
        }
    }
}
