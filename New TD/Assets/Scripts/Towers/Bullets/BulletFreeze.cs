using UnityEngine;

public class BulletFreeze : MonoBehaviour
{
    public Transform target = null;
    public string enemyTag;
    public float speed;
    public float range;
    public float damage;
    public float freezTime;
    
    void Start()
    {
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

        if (transform.position.x > PlayerStats.gridSize || transform.position.x < -PlayerStats.gridSize || transform.position.y > PlayerStats.gridSize || transform.position.y < -PlayerStats.gridSize)
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
            Enemy enemyScript = collision.gameObject.GetComponent<Enemy>();
            enemyScript.Attack(damage);
            enemyScript.Freeze(freezTime);
            Destroy(gameObject);
            return;
        }
    }
}
